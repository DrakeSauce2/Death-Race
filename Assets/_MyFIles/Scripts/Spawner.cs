using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject objectToSpawn;
    [Space]
    [SerializeField] private float xBounds = 5.0f;
    [SerializeField] private float yBounds = 5.0f;
    [Space]
    [SerializeField] private float timerDelay = 3.0f;
    [Space]
    [SerializeField] private int maxSpawnCount = 10;

    private List<GameObject> localInstancedObjects = new List<GameObject>();


    private bool isSpawnerActive = false;
    public bool GetIsSpawnerActive() { return isSpawnerActive; }

    public void SetSpawnerActive(bool state)
    {
        isSpawnerActive = state;
    }

    public void StartSpawnerCycle()
    {
        SetSpawnerActive(true);

        StartCoroutine(SpawnCoroutine());
    }

    public Vector2 GetBounds()
    {
        return new Vector2(xBounds, yBounds);
    }

    private IEnumerator SpawnCoroutine()
    {
        GameObject instancedObject = Instantiate(objectToSpawn, GetSpawnLocation(xBounds, yBounds, 0), Quaternion.identity);
        localInstancedObjects.Add(instancedObject);

        Enemy enemy = instancedObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.onDead += RemoveObjectFromList;
        }

        yield return new WaitForSeconds(timerDelay);

        StartNextSpawn();
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(timerDelay);

        StartNextSpawn();
    }

    private void StartNextSpawn()
    {
        if (GetIsSpawnerActive() == false) return;

        if (localInstancedObjects.Count == maxSpawnCount)
        {
            StartCoroutine(WaitCoroutine());
            return;
        }        

        StartCoroutine(SpawnCoroutine());
    }

    public void ClearInstancedObjects()
    {
        foreach (GameObject instancedObject in localInstancedObjects)
        {
            Destroy(instancedObject);
        }

        localInstancedObjects.Clear();
    }

    public void RemoveObjectFromList(GameObject objectToRemove)
    {
        localInstancedObjects.Remove(objectToRemove);
    }

    public Vector3 GetSpawnLocation(float x, float y, float z)
    {
        float xPos = x == 0 ? 0 : Random.Range(-x, x);
        float yPos = y == 0 ? 0 : Random.Range(-y, y);
        float zPos = z == 0 ? 0 : Random.Range(-z, z);

        return new Vector3(xPos, yPos, zPos);
    }

}
