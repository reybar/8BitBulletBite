using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LayerMask laserCollision;
    public Transform laser;
    public LineRenderer laserRenderer;

    void Update()
    {
        LaserSight();
    }

    public void LaserSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, transform.right, 30, laserCollision);
        if (hit.collider) {
            laserRenderer.SetPosition(0, laser.transform.position);
            laserRenderer.SetPosition(1, hit.point);
        } else {
            laserRenderer.SetPosition(0, laser.transform.position);
            laserRenderer.SetPosition(1, laser.transform.position + transform.right * 30);
        }
    }
}
