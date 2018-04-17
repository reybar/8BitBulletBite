using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

    public int damage;

    private void OnTriggerEnter2D(Collider2D coll) {
        Debug.Log(coll.gameObject.name, gameObject);
        if(coll.tag == "Player") {
            coll.GetComponent<Health>().currHealth -= damage;
            Destroy(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    //TODO change collision tag to Player when implemented
    //TODO destroy bullets on screen edge

}
