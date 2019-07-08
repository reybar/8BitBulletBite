using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    Scoreboard scoreboard;

    [SerializeField]
    MapTime mapTime;


    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private GameObject pos1;
    [SerializeField]
    private GameObject pos2;
    [SerializeField]
    private GameObject pos3;

    [SerializeField]
    private Text firstPlayerName;
    [SerializeField]
    private Text firstPlayerKD;
    [SerializeField]
    private Text secondPlayerName;
    [SerializeField]
    private Text secondPlayerKD;
    [SerializeField]
    private Text thirdPlayerName;
    [SerializeField]
    private Text thirdPlayerKD;

    private PlayerStats ps1;
    private PlayerStats ps2;
    private PlayerStats ps3;


    void Update()
    {
        if (mapTime == null) {
            mapTime = FindObjectOfType<MapTime>();
        }
        if (mapTime.gameEnded) {
            End();
        }
    }

    public void End()
    {
        scoreboard.UpdateScoreboard();
        panel.SetActive(true);
        pos1.SetActive(true);
        if (scoreboard.players.Length > 1) {
            pos2.SetActive(true);
        }
        if (scoreboard.players.Length > 2) {
            pos3.SetActive(true);
        }

        foreach (PlayerStats ps in FindObjectsOfType<PlayerStats>()) {
            if (ps1 != null && ps.kills >= ps1.kills) {
                ps3 = ps2;
                ps2 = ps1;
                ps1 = ps;
            } else if (ps2 != null && ps.kills >= ps2.kills) {
                ps3 = ps2;
                ps2 = ps;

            } else if (ps3 != null && ps.kills >= ps3.kills) {
                ps3 = ps;
            } else {
                ps1 = ps;
            }
        }

        firstPlayerName.text = ps1.name;
        firstPlayerKD.text = ps1.kills + "K " + ps1.deaths + "D";
        if (ps2 != null) {
            secondPlayerName.text = ps2.name;
            secondPlayerKD.text = ps2.kills + "K " + ps2.deaths + "D";
        }
        if (ps3 != null) {
            thirdPlayerName.text = ps3.name;
            thirdPlayerKD.text = ps3.kills + "K " + ps3.deaths + "D";
        }
    }
}
