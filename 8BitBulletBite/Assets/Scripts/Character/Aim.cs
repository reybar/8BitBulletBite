using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Aim : NetworkBehaviour {

    [SerializeField] private Transform pivot;
    [SerializeField] private Transform weaponLoc;

    [SyncVar(hook = "OnUpdateScale")]
    private float scale;



    private void Start() {
        pivot = transform.Find("Weapon pivot");
        weaponLoc = pivot.transform.Find("Weapon");
        scale = weaponLoc.transform.localScale.y;

    }

    private void Update() {
        if(!isLocalPlayer) {
            return;
        }
        Rotate();
    }

    void Rotate() {

        Vector2 distance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivot.transform.position;
        distance.Normalize();
        float rotation = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0, 0, rotation);

        if(rotation > 90 || rotation < -90) {
            weaponLoc.transform.localScale = new Vector2(1, -1);
        }
        else {
            weaponLoc.transform.localScale = new Vector2(1, 1);
        }

        CmdScale(weaponLoc.transform.localScale.y);

    }

    [Command]
    void CmdScale(float newScale) {
        scale = newScale;
    }

    void OnUpdateScale(float newScale) {
        scale = newScale;
        weaponLoc.transform.localScale = new Vector2(1, scale);
    }

}
