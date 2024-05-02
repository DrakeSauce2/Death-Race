using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool bHasGameStarted = false;

    private KillCounter killCounter;

    private Timer timer;

    private Spawner spawner;

    [Header("Start Panel")]
    [SerializeField] private GameObject startPanel;

    [Header("Player")]
    [SerializeField] GameObject playerPrefab;
    private Player currentPlayer = null;
    [SerializeField] private Transform spawnPoint;

    List<GameObject> instancedTombstoneObjects = new List<GameObject>();

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        timer = GetComponent<Timer>();
        timer.onTimerEnd += EndGame;

        killCounter = GetComponent<KillCounter>();
        spawner = GetComponent<Spawner>();
    }

    public void StartGame()
    {
        timer.ResetTimer();
        timer.SetPauseTimer(false);

        if (!currentPlayer)
        {
            InitializePlayer();
        }
        else
        {
            currentPlayer.transform.position = spawnPoint.position;
            currentPlayer.transform.localEulerAngles = Vector3.zero;

            currentPlayer.PlayerStart();
        }

        ClearInstancedTombstones();

        killCounter.ResetCounter();

        spawner.StartSpawnerCycle();

        InputManager.Instance.SetInputActive(true);

        startPanel.SetActive(false);
        bHasGameStarted = true;
    }

    private void InitializePlayer()
    {
        Player instancedPlayer = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity).GetComponent<Player>();
        if (instancedPlayer)
        {
            currentPlayer = instancedPlayer;

            currentPlayer.Init();

            killCounter.SetPlayerDelegate(currentPlayer);
        }
    }

    private void EndGame()
    {
        startPanel.SetActive(true);

        spawner.SetSpawnerActive(false);
        spawner.ClearInstancedObjects();

        InputManager.Instance.SetInputActive(false);
        bHasGameStarted = false;
    }

    public void ClearInstancedTombstones()
    {
        if (instancedTombstoneObjects.Count > 0)
        {
            foreach (GameObject objectToDelete in instancedTombstoneObjects)
            {
                Destroy(objectToDelete);
            }

            instancedTombstoneObjects.Clear();
        }
    }

    public void AddInstancedTombstone(GameObject objectToAdd)
    {
        instancedTombstoneObjects.Add(objectToAdd);
    }

    public bool HasGameStarted()
    {
        return bHasGameStarted;
    }

    public Spawner GetSpawner()
    {
        return spawner;
    }

}
