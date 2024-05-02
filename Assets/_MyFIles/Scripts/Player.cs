using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [Space]
    [SerializeField] private GameObject tombstonePrefab;

    public delegate void OnKill();
    public OnKill onKill;

    Rigidbody2D rBody;
    bool bTurnCooldown = false;

    bool isInitialized = false;

    bool hitStun = false;
    bool hasCollision = false;

    private AudioSource audioSource;
    [Header("Audio Clips")]
    [SerializeField] AudioClip engineRunningSFX;
    [SerializeField] AudioClip engineIdleSFX;

    public void Init()
    {
        rBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        isInitialized = true;
    }

    public void PlayerStart()
    {
        audioSource.Play();
        hasCollision = false;
        hitStun = false;
        bTurnCooldown = false;
    }

    private void Update()
    {
        if (!isInitialized) return;

        if (GameManager.Instance.HasGameStarted() == false)
        {
            rBody.velocity = Vector3.zero;

            audioSource.Stop();

            return;
        }

        if (hitStun)
        {
            return;
        }

        if (rBody.velocity == Vector2.zero || hasCollision)
        {
            PlayAudioClip(engineIdleSFX);
        }
        else
        {
            PlayAudioClip(engineRunningSFX);
        }

        PlayerMove();
    }

    private void PlayerMove()
    {
        float yInput = InputManager.Instance.moveInput.y;
        float forwardSpeed = yInput < 0 ? (moveSpeed / 4) * yInput : moveSpeed * yInput;

        rBody.velocity = transform.up * forwardSpeed;

        if (bTurnCooldown) return;

        StartCoroutine(Turn());
    }

    private IEnumerator Turn()
    {
        bTurnCooldown = true;

        transform.Rotate(-transform.forward, 30f * Mathf.RoundToInt(InputManager.Instance.moveInput.x));

        yield return new WaitForSeconds(0.1f);

        bTurnCooldown = false;
    }

    private IEnumerator HitStun()
    {
        InputManager.Instance.SetInputActive(false);

        yield return new WaitForSeconds(1f);

        InputManager.Instance.SetInputActive(true);
    }

    private void Bump()
    {
        rBody.velocity = Vector2.zero;

        AudioManager.Instance.PlayCollisionSFX();

        StartCoroutine(HitStun());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rBody.velocity.magnitude <= 0) return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.onDead.Invoke(enemy.gameObject);
            onKill.Invoke();

            AudioManager.Instance.PlayDeathSFX();

            GameManager.Instance.AddInstancedTombstone(Instantiate(tombstonePrefab, collision.transform.position, Quaternion.identity));
            Destroy(collision.gameObject);
        }

        hasCollision = true;

        Bump();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hasCollision = false;
    }

    private void PlayAudioClip(AudioClip clipToPlay)
    {
        if (clipToPlay == audioSource.clip)
        {
            Debug.Log(audioSource.clip);
            return;
        }

        audioSource.clip = clipToPlay;
        audioSource.Play();
    }

}
