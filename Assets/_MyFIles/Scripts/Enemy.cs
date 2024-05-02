using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void OnDead(GameObject owner);
    public OnDead onDead;

    [SerializeField] private float moveSpeed = 10f;

    private bool isMoving = false;
    public bool GetIsMoving() { return isMoving; }

    [SerializeField] Animator animator;

    private void Awake()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator MoveTo(Vector2 targetLocation)
    {
        isMoving = true;

        while (Vector2.Distance(transform.position, targetLocation) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetLocation, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        float waitTime = Random.Range(0.1f, 0.25f);

        yield return new WaitForSeconds(waitTime);

        Vector2 nextPosition = RandomSpawnLocation();

        if (nextPosition.x == transform.position.x)
        {
            int direction = nextPosition.y - transform.position.y >= 0 ? 1 : -1;
            animator.SetFloat("xVel", 0);
            animator.SetFloat("yVel", direction);
        }
        else
        {
            int direction = nextPosition.x - transform.position.x >= 0 ? 1 : -1;
            animator.SetFloat("xVel", direction);
            animator.SetFloat("yVel", 0);
        }

        StartCoroutine(MoveTo(nextPosition));
    }

    private Vector2 RandomSpawnLocation()
    {
        Vector2 bounds = GameManager.Instance.GetSpawner().GetBounds();

        int rand = Mathf.CeilToInt(Random.Range(0, 2));
        Vector2 nextPosition;
        if (rand == 0)
        {
            nextPosition = new Vector2
            (
                Random.Range(-bounds.x + 0.5f, bounds.x - 0.5f),
                transform.position.y
            );
        }
        else
        {
            nextPosition = new Vector2
            (
                transform.position.x,
                Random.Range(-bounds.y, bounds.y)
            );
        }
        
        return nextPosition;
    }

}
