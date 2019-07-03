using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

    [SerializeField]
    private GameObject[] players;

    [SerializeField]
    private GameObject scoreboardItem;

    [SerializeField]
    private Transform scoreboardContainer;


    public void UpdateScoreboard() {

        foreach (Transform child in scoreboardContainer) {
            Destroy(child.gameObject);
        }

        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players) {
            GameObject item = Instantiate(scoreboardItem, scoreboardContainer);
            ScoreboardItem sbItem = item.GetComponent<ScoreboardItem>();
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            sbItem.name.text = playerStats.name;
            sbItem.kills.text = playerStats.kills.ToString();
            sbItem.deaths.text = playerStats.deaths.ToString();
        }
    }
}
