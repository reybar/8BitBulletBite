using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickupBehaviour : NetworkBehaviour
{
    public float timer = 5f;

    private BoxCollider2D coll;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Disable()
    {
        RpcDisable();
        Invoke("Enable", timer);
    }

    private void Enable()
    {
        RpcEnable();
    }

    [ClientRpc]
    private void RpcDisable()
    {
        coll.enabled = false;
        spriteRenderer.enabled = false;
    }
    [ClientRpc]
    private void RpcEnable()
    {
        coll.enabled = true;
        spriteRenderer.enabled = true;
    }
}
