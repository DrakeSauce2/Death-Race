using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerKillCountText;
    private int playerKillCount = 0;

    public void SetPlayerDelegate(Player currentPlayer)
    {
        currentPlayer.onKill += UpdatePlayerCounter;
    }

    public void UpdatePlayerCounter()
    {
        playerKillCount++;
        playerKillCountText.text = string.Format("{0:00}", playerKillCount);
    }

    public void ResetCounter()
    {
        playerKillCount = 0;

        playerKillCountText.text = string.Format("{0:00}", playerKillCount);

    }

}
