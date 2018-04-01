using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner : NetworkBehaviour {

    public GameObject pistol;
    public GameObject sniper;

    
    public override void  OnStartServer() {
        foreach(Transform weapon in transform) {
            if(weapon.tag=="PistolPickup") {
                GameObject item = Instantiate(pistol, weapon.transform.position, Quaternion.identity) as GameObject;
                //item.transform.parent = weapon.transform;
                NetworkServer.Spawn(item);
            }
            if(weapon.tag=="SniperPickup") {
                GameObject item = Instantiate(sniper, weapon.transform.position, Quaternion.identity) as GameObject;
                NetworkServer.Spawn(item);
                //item.transform.parent = weapon.transform;
            }

        }
    }

    
}
