using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapTime : NetworkBehaviour
{
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
        timeLeft -= Time.deltaTime;
    }
}
