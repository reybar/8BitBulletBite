using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

    public int damage;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isServer) {
            return;
        }

        if (coll.tag == "Player") {
            coll.GetComponent<Health>().currHealth -= damage;
            Destroy(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
