using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapTime : NetworkBehaviour
{

    public static bool gameEndedStatic = false;

    [SyncVar]
    public bool gameEnded = false;

    [SyncVar]
    public float timeLeft;

    [SerializeField]
    private float startTime;

    public void Start()
    {
        timeLeft = startTime * 60;
    }

    [ServerCallback]
    private void Update()
    {
        if (!gameEnded) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0) {
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        gameEnded = true;
        gameEndedStatic = true;
    }
}
