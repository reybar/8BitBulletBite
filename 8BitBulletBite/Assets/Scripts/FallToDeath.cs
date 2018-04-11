using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallToDeath : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D coll) {
        if(coll.gameObject.tag=="Player") {
            coll.gameObject.GetComponent<Health>().currHealth = 0;
        }
    }
}
