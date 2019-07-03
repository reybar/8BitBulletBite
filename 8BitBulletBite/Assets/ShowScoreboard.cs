using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowScoreboard : MonoBehaviour {

    [SerializeField]
    private GameObject scoreboard;

    public Scoreboard scores;

	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            scoreboard.SetActive(true);
            scores.UpdateScoreboard();
        } else if (Input.GetKeyUp(KeyCode.Tab)) {
            scoreboard.SetActive(false);
        }
	}
}
