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
        if(laser.GetComponent<LineRenderer>() ) {
            laserRenderer = laser.GetComponent<LineRenderer>();
        }

        laserRenderer.useWorldSpace = false;    

    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        //Vector2 laserOrigin = new Vector2(laser.position.x, laser.position.y);
        RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, transform.right, 30,laserCollision);
        
        if(hit.collider ) {
            Debug.DrawLine(laser.transform.position, hit.point, new Color(0,0,0));
            laserRenderer.SetPosition(0,new Vector2(hit.distance, 0));
            //laserRenderer.SetPosition(0,laser.transform.position);
            //laserRenderer.SetPosition(1, hit.point);
        }
        else {
            laserRenderer.SetPosition(0, new Vector2 (30, 0));
            //TODO laser end at screen edge


        }
        
    }

    
}
