using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Aim : NetworkBehaviour
{
    [SerializeField]
    private Transform pivot;
    [SerializeField]
    private Transform weaponLoc;

    [SyncVar(hook = "OnFlip")]
    private float scale;

    private void Start()
    {
        scale = weaponLoc.transform.localScale.y;
    }

    private void Update()
    {
        if (!isLocalPlayer) {
            return;
        }
        Rotate();
    }

    void Rotate()
    {
        Vector2 distance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivot.transform.position;
        distance.Normalize();
        float rotation = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0, 0, rotation);

        if (rotation > 90 || rotation < -90) {
            weaponLoc.transform.localScale = new Vector2(1, -1);
        } else {
            weaponLoc.transform.localScale = new Vector2(1, 1);
        }
        CmdFlip(weaponLoc.transform.localScale.y);
    }

    [Command]
    void CmdFlip(float newScale)
    {
        scale = newScale;
    }

    void OnFlip(float newScale)
    {
        scale = newScale;
        weaponLoc.transform.localScale = new Vector2(1, scale);
    }

}
