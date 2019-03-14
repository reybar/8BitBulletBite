using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShootingNET : NetworkBehaviour
{
    public GameObject weapon;
    public GameObject bullet;
    public GameObject weaponThrown;
    public float bulletSpeed;
    public int bulletDamage;
    public int throwSpeed;
    public int throwDamage;

    private WeaponPickup weaponPickup;

    private void Start()
    {
        weaponPickup = GetComponent<WeaponPickup>();
    }

    [Command]
    public void CmdShoot(Vector2 firePoint, Vector2 direction, float rotation)
    {
        GameObject pallet = Instantiate(bullet, firePoint, Quaternion.Euler(0, 0, rotation + 90)) as GameObject;
        Physics2D.IgnoreCollision(pallet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        pallet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        pallet.GetComponent<Projectile>().damage = bulletDamage;
        NetworkServer.Spawn(pallet);
        Destroy(pallet, 1);
    }

    [Command]
    public void CmdThrow(Vector2 firePoint, Vector2 direction, float rotation)
    {
        if (weaponPickup.weapon1 != null && weaponPickup.weapon1.activeSelf) {
            weaponPickup.slot1 = 0;
        } else if (weaponPickup.weapon2 != null && weaponPickup.weapon2.activeSelf) {
            weaponPickup.slot2 = 0;
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
    }

}
