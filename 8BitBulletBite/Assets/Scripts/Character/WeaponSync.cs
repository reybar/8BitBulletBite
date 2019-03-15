using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSync : NetworkBehaviour
{
    public GameObject[] weaponType;
    public GameObject weapon1 = null;
    public GameObject weapon2 = null;
    public Transform weaponPosition;
    [SyncVar]
    public int currentWeapon = 0;
    [SyncVar]
    public int slot1 = 0;
    [SyncVar]
    public int slot2 = 0;

    void Update()
    {
        if (isLocalPlayer) {
            InputManager();
            CmdCurrentWeaponUpdate(currentWeapon);
        }
        CurrentWeaponCheck();
        ActivateWeapon();
        DeactivateWeapon();
        WeaponDropedCheck();
    }

    private void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapon1 != null) {
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapon2 != null) {
            SwitchWeapon(2);
        }
    }

    private void SwitchWeapon(int slot)
    {
        if (slot == 1) {
            weapon1.SetActive(true);
            if (weapon2 != null) {
                weapon2.SetActive(false);
            }
        } else if (slot == 2) {
            weapon2.SetActive(true);
            if (weapon1 != null) {
                weapon1.SetActive(false);
            }
        }
        currentWeapon = slot;
    }

    [Command]
    void CmdCurrentWeaponUpdate(int activeSlot)
    {
        currentWeapon = activeSlot;
    }

    private void CurrentWeaponCheck()
    {
        if (currentWeapon == 1 && weapon1 != null) {
            SwitchWeapon(currentWeapon);
        } else if (currentWeapon == 2 && weapon2 != null) {
            SwitchWeapon(currentWeapon);
        }
    }

    void ActivateWeapon()
    {
        if (weapon1 == null && slot1 != 0) {
            weapon1 = Instantiate(weaponType[slot1 - 1], weaponPosition.transform.position, weaponPosition.transform.rotation) as GameObject;
            weapon1.transform.parent = weaponPosition.transform;
            if (weapon1.transform.localScale.y < 0) {
                weapon1.transform.localScale = new Vector2(weapon1.transform.localScale.x, weapon1.transform.localScale.y * -1);
            }
            SwitchWeapon(1);
        }
        if (weapon2 == null && slot2 != 0) {
            weapon2 = Instantiate(weaponType[slot2 - 1], weaponPosition.transform.position, weaponPosition.transform.rotation) as GameObject;
            weapon2.transform.parent = weaponPosition.transform;
            if (weapon2.transform.localScale.y < 0) {
                weapon2.transform.localScale = new Vector2(weapon2.transform.localScale.x, weapon2.transform.localScale.y * -1);
            }
            SwitchWeapon(2);
        }
    }

    void DeactivateWeapon()
    {
        if (weapon1 != null && slot1 == 0) {
            Destroy(weapon1);
        }
        if (weapon2 != null && slot2 == 0) {
            Destroy(weapon2);
        }
    }

    public void WeaponDropedCheck()
    {
        if (weapon2 == null && weapon1 != null && !weapon1.activeSelf) {
            SwitchWeapon(1);
        }
        if (weapon1 == null && weapon2 != null && !weapon2.activeSelf) {
            SwitchWeapon(2);
        }
    }
}
