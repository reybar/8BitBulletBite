using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject weaponThrown;
    public GameObject bullet;
    public float fireRate;
    public int magSize;
    public int ammo;
    public float bulletSpeed;
    public int bulletDamage;
    public int throwSpeed = 30;
    public int throwDamage = 2;
    public AudioClip shootingClip;

    private GameObject player;
    private ShootingNET shootingNet;
    private bool reloading = false;
    private bool ready = true;
    private float lastShot;
    private float projectileRotation;
    private Vector2 projectileDirection;

    private void Start()
    {
        player = transform.root.gameObject;
        EquipWeapon();
        ammo = magSize;
    }

    private void OnEnable()
    {
        player = transform.root.gameObject;
        EquipWeapon();
        reloading = false;
        if (ammo <= 0) {
            StartCoroutine(Reload());
        } else {
            ready = false;
            StartCoroutine(Setup());
        }
    }

    private void EquipWeapon()
    {
        if (player.GetComponent<ShootingNET>()) {
            shootingNet = player.GetComponent<ShootingNET>();
            shootingNet.bullet = bullet;
            shootingNet.bulletSpeed = bulletSpeed;
            shootingNet.bulletDamage = bulletDamage;
            shootingNet.weapon = this;
            shootingNet.weaponThrown = weaponThrown;
            shootingNet.throwSpeed = throwSpeed;
            shootingNet.throwDamage = throwDamage;
        }
    }

    private void Update()
    {
        if (!player.GetComponent<NetworkIdentity>().isLocalPlayer) {
            return;
        }
        Aim();
        InputManager();
    }

    private void Aim()
    {
        projectileDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        projectileDirection.Normalize();
        projectileRotation = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
    }

    private void InputManager()
    {
        if (Input.GetButtonDown("Fire1") && (Time.time > (fireRate + lastShot)) && ammo > 0 && !reloading && ready) {
            Shoot();
        }
        if (Input.GetButtonDown("Throw")) {
            Throw();
        }
        if (Input.GetButtonDown("Reload") && ammo < magSize && !reloading) {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        shootingNet.CmdShoot(firePoint.position, projectileDirection, projectileRotation);
        lastShot = Time.time;
        ammo--;
        if (ammo <= 0) {
            StartCoroutine(Reload());
        }
    }

    void Throw()
    {
        shootingNet.CmdThrow(firePoint.position, projectileDirection, projectileRotation);
    }

    IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(2);
        ammo = magSize;
        reloading = false;
    }

    IEnumerator Setup()
    {
        yield return new WaitForSeconds(0.14f);
        ready = true;
    }
}
