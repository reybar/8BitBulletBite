using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weapon : MonoBehaviour {

    public Transform firePoint;
    public GameObject player;
    public GameObject weaponThrown;
    public GameObject bullet;

    public float fireRate;
    public int magSize;
    public int ammo;

    public float bulletSpeed;
    public int bulletDamage;

    public int throwSpeed = 30;
    public int throwDamage = 2;

    protected Vector2 mousePosition;

    public bool gotAmmo = true;
    public bool reloading = false;
    public bool ready = true;

    private float lastShot;
    private float rotation;
    private Vector2 direction;

    public ShootingNET shoot;

    public bool exists = true;


    public virtual void Start() {
        player = transform.root.gameObject;
        firePoint = transform.Find("FirePoint");
        if(player.GetComponent<ShootingNET>()) {
            shoot = player.GetComponent<ShootingNET>();

            shoot.bullet = bullet;
            shoot.bulletSpeed = bulletSpeed;
            shoot.bulletDamage = bulletDamage;
            shoot.weap = this.gameObject;

            shoot.weaponThrown = weaponThrown;
            shoot.throwSpeed = throwSpeed;
            shoot.throwDamage = throwDamage;
        }
        ammo = magSize;
    }

    public virtual void Update() {
        CheckAmmo();
        getDirections();

        Reload();

        if(player.GetComponent<NetworkIdentity>().isLocalPlayer) {
            Throw();
            Shoot();
        }
    }

    private void OnEnable() {
        player = transform.root.gameObject;
        firePoint = transform.Find("FirePoint");
        if(player.GetComponent<ShootingNET>()) {
            shoot = player.GetComponent<ShootingNET>();

            shoot.bullet = bullet;
            shoot.bulletSpeed = bulletSpeed;
            shoot.bulletDamage = bulletDamage;
            shoot.weap = this.gameObject;

            shoot.weaponThrown = weaponThrown;
            shoot.throwSpeed = throwSpeed;
            shoot.throwDamage = throwDamage;
        }
        reloading = false;
        if(ammo <= 0) {
            StartCoroutine(IReload());
        }
        else {
            ready = false;
            StartCoroutine(ISetup());
        }
    }

    void getDirections() {
        mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        direction.Normalize();
        rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void Shoot() {

        if(Input.GetButton("Fire1") && (Time.time > (fireRate + lastShot)) && ammo > 0 && !reloading && ready) {
            shoot.CmdShoot(firePoint.position, direction, rotation);

            lastShot = Time.time;
            ammo--;
            if(ammo <= 0) {
                gotAmmo = false;
            }
        }
    }

    void Throw() {
        if(Input.GetButtonDown("Throw")) {
            shoot.CmdThrow(firePoint.position, direction, rotation);

        }
    }

    void Reload() {
        if(Input.GetButtonDown("Reload") && ammo < magSize && !reloading) {
            StartCoroutine(IReload());
        }
    }


    void CheckAmmo() {
        if(!gotAmmo) {
            gotAmmo = true;
            StartCoroutine(IReload());

        }
    }

    IEnumerator IReload() {
        reloading = true;
        yield return new WaitForSeconds(2);
        ammo = magSize;
        reloading = false;
    }

    IEnumerator ISetup() {
        yield return new WaitForSeconds(0.5f);
        ready = true;
    }
}
