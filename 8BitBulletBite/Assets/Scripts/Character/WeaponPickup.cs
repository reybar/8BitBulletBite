using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponPickup : NetworkBehaviour
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
            if (Input.GetKeyDown(KeyCode.Alpha1) && weapon1 != null) {
                SwitchWeapon(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && weapon2 != null) {
                SwitchWeapon(2);
            }
            CmdCurrentWeapon(currentWeapon);
        }
        DestroyWeapon();
        EquipWeapon();
        CurrentWeapon();
        isDroped();
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (!isLocalPlayer) {
            return;
        }
        if (weapon1 == null) {
            if (coll.tag == "Pistol") {
                CmdEquipWeapon(1, 1);
                //slot1 = 1;
                CmdDisable(coll.GetComponent<NetworkIdentity>().netId);
            } else if (coll.tag == "Sniper") {
                CmdEquipWeapon(1, 2);
                //slot1 = 2;
                CmdDisable(coll.GetComponent<NetworkIdentity>().netId);
            }
        } else if (weapon2 == null) {
            if (coll.tag == "Pistol") {
                CmdEquipWeapon(2, 1);
                CmdDisable(coll.GetComponent<NetworkIdentity>().netId);
                //slot2 = 1;
            } else if (coll.tag == "Sniper") {
                CmdEquipWeapon(2, 2);
                CmdDisable(coll.GetComponent<NetworkIdentity>().netId);
                //slot2 = 2;
            }
        }
    }

    [Command]
    void CmdDisable(NetworkInstanceId GO)
    {
        GameObject gObj = ClientScene.FindLocalObject(GO);
        gObj.GetComponent<PickupBehaviour>().DisableObj();
    }

    [Command]
    void CmdEquipWeapon(int slotIndex, int weaponIndex)
    {
        if (slotIndex == 1) {
            slot1 = weaponIndex;
        }
        if (slotIndex == 2) {
            slot2 = weaponIndex;
        }
    }


    void EquipWeapon()
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

    void DestroyWeapon()
    {
        if (weapon1 != null && slot1 == 0) {
            Destroy(weapon1);
        }
        if (weapon2 != null && slot2 == 0) {
            Destroy(weapon2);
        }
    }


    /*private void OnTriggerStay2D(Collider2D coll) {
        if(weapon1 == null || weapon2 == null) {
            if(coll.tag == "Pistol") {
                EquipWeapon(weaponType[0]);
                coll.gameObject.SetActive(false);
            }
            else if(coll.tag == "Sniper") {
                EquipWeapon(weaponType[1]);
                coll.gameObject.SetActive(false);
            }
        }
    }

    void EquipWeapon(GameObject weaponType) {
        if(weapon1 == null) {

            weapon1 = Instantiate(weaponType, weaponPosition.transform.position, weaponPosition.transform.rotation) as GameObject;
            weapon1.transform.parent = weaponPosition.transform;
            if(weapon1.transform.localScale.y < 0) {
                weapon1.transform.localScale = new Vector2(weapon1.transform.localScale.x, weapon1.transform.localScale.y * -1);
            }

            SwitchWeapon(1);

        }
        else if(weapon2 == null) {
            weapon2 = Instantiate(weaponType, weaponPosition.transform.position, weaponPosition.transform.rotation) as GameObject;
            weapon2.transform.parent = weaponPosition.transform;
            if(weapon2.transform.localScale.y < 0) {
                weapon2.transform.localScale = new Vector2(weapon2.transform.localScale.x, weapon2.transform.localScale.y * -1);
            }
            SwitchWeapon(2);
        }
    }*/

    private void CurrentWeapon()
    {
        if (currentWeapon == 1 && weapon1 != null) {
            SwitchWeapon(currentWeapon);
        } else if (currentWeapon == 2 && weapon2 != null) {
            SwitchWeapon(currentWeapon);
        }



    }

    private void SwitchWeapon(int num)
    {
        if (num == 1) {

            weapon1.SetActive(true);

            if (weapon2 != null) {

                weapon2.SetActive(false);
            }

        } else if (num == 2) {
            weapon2.SetActive(true);

            if (weapon1 != null) {
                weapon1.SetActive(false);
            }
        }
        currentWeapon = num;
    }

    [Command]
    void CmdCurrentWeapon(int num)
    {
        currentWeapon = num;
    }



    public void isDroped()
    {
        if (weapon2 == null && weapon1 != null && !weapon1.activeSelf) {
            SwitchWeapon(1);
        }
        if (weapon1 == null && weapon2 != null && !weapon2.activeSelf) {
            SwitchWeapon(2);
        }
    }
}
