using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sniper : Weapon {

    public LayerMask laserCollision;
    public Transform laser;
    public Transform laserHit;
    private LineRenderer laserRenderer;


    // Use this for initialization
    public override void Start() {
        base.Start();

        laser = transform.Find("Laser");
        if(laser.GetComponent<LineRenderer>()) {
            laserRenderer = laser.GetComponent<LineRenderer>();
        }

        laserRenderer.useWorldSpace = true;

    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, transform.right, 30, laserCollision);

        if(hit.collider) {
            laserRenderer.SetPosition(0, laser.transform.position);
            laserRenderer.SetPosition(1, hit.point);
        }
        else {
            laserRenderer.SetPosition(0, laser.transform.position);
            laserRenderer.SetPosition(1, laser.transform.position + transform.right * 30);

            //TODO laser end at screen edge


        }

    }


}
