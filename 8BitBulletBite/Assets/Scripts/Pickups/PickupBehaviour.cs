using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickupBehaviour : NetworkBehaviour {

    BoxCollider2D coll;
    SpriteRenderer sRend;

    public float timer = 5f;
    private void Start() {
        coll = GetComponent<BoxCollider2D>();
        sRend = GetComponent<SpriteRenderer>();
    }

    public void DisableObj() {
        RpcDisable();
        Invoke("EnableObj", timer);

    }

    void EnableObj() {
        RpcEnable();
    }

    [ClientRpc]
    void RpcDisable() {
        coll.enabled = false;
        sRend.enabled = false;
    }
    [ClientRpc]
    void RpcEnable() {
        coll.enabled = true;
        sRend.enabled = true;
    }
}
