using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShootingNET : NetworkBehaviour
{
    public Weapon weapon;
    public GameObject bullet;
    public GameObject weaponThrown;
    public float bulletSpeed;
    public int bulletDamage;
    public int throwSpeed;
    public int throwDamage;
    public LineRenderer lineRenderer;

    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private AudioClip thrownClip;

    private WeaponSync weaponSync;

    private void Start()
    {
        weaponSync = GetComponent<WeaponSync>();
    }

    [Command]
    public void CmdShoot(Vector2 firePoint, Vector2 direction, float rotation)
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint, direction, 100);

        if (hit.transform.gameObject.tag == "Player") {
            if (hit.transform.gameObject.GetComponent<Health>().currHealth <= bulletDamage) {
                playerStats.kills++;
                hit.transform.gameObject.GetComponent<PlayerStats>().deaths--;
            }
            hit.transform.gameObject.GetComponent<Health>().currHealth -= bulletDamage;
        }
        if (hit.collider == null) {
            RpcShoot(firePoint, direction, hit.point, false);
        } else {
            RpcShoot(firePoint, direction, hit.point, true);
        }
    }

    [ClientRpc]
    private void RpcShoot(Vector2 firePoint, Vector2 direction, Vector2 hitPoint, bool targetHit)
    {
        AudioSource.PlayClipAtPoint(weapon.shootingClip, firePoint);
        weapon.animator.Play("Recoil");
        if (targetHit) {
            lineRenderer.SetPosition(0, firePoint);
            lineRenderer.SetPosition(1, hitPoint);
        } else {
            lineRenderer.SetPosition(0, firePoint);
            lineRenderer.SetPosition(1, firePoint + direction * 100);
        }
        StartCoroutine(BulletTrail());
    }

    IEnumerator BulletTrail()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
    }

    [Command]
    public void CmdThrow(Vector2 firePoint, Vector2 direction, float rotation)
    {
        if (weaponSync.weapon1 != null && weaponSync.weapon1.activeSelf) {
            weaponSync.slot1 = 0;
        } else if (weaponSync.weapon2 != null && weaponSync.weapon2.activeSelf) {
            weaponSync.slot2 = 0;
        }

        GameObject thrown = Instantiate(weaponThrown, firePoint, Quaternion.Euler(0, 0, rotation)) as GameObject;
        Physics2D.IgnoreCollision(thrown.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        thrown.GetComponent<Rigidbody2D>().velocity = direction * throwSpeed;
        thrown.GetComponent<Projectile>().damage = throwDamage;
        if (rotation > 90 || rotation < -90) {
            thrown.transform.localScale = new Vector2(thrown.transform.localScale.x, thrown.transform.localScale.y * -1);
        }
        NetworkServer.Spawn(thrown);
        Destroy(thrown, 1);
        RpcThrow(firePoint);
    }

    [ClientRpc]
    private void RpcThrow(Vector2 firePoint){
        AudioSource.PlayClipAtPoint(thrownClip, firePoint);
    }
}
