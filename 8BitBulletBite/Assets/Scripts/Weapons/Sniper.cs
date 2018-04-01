using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sniper : Weapon {

    public LayerMask laserCollision;
    public Transform laser;
    private LineRenderer laserRenderer;
    

    // Use this for initialization
    public override void Start() {
        base.Start();
        
        laser = transform.Find("Laser");
        if(laser.GetComponent<LineRenderer>() ) {
            laserRenderer = laser.GetComponent<LineRenderer>();
        }

        laserRenderer.useWorldSpace = false;    

    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        Vector2 laserOrigin = new Vector2(laser.position.x, laser.position.y);
        RaycastHit2D hit = Physics2D.Raycast(laserOrigin, transform.right, 30,laserCollision);
        if(hit.collider ) {
           
            laserRenderer.SetPosition(1, new Vector2(hit.distance, 0));
        }
        else {
            laserRenderer.SetPosition(1, new Vector2(30,0));
            //TODO laser end at screen edge

        
        }
        
    }

    
}
