using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour {

    public SetupLocalPlayer setupLocalPlayer;

    [SyncVar]
    public string name = "Player";

    [SyncVar]
    public int kills = 0;

    [SyncVar]
    public int deaths = 0;

	void Start () {
        name = setupLocalPlayer.pname;
	}
	
	void Update () {
		
	}
}
