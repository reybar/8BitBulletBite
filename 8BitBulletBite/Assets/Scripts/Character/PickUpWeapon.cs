using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpWeapon : NetworkBehaviour
{
    public WeaponSync weaponSync;

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (!isLocalPlayer) {
            return;
        }
        if (coll.GetComponent<WeaponPickup>() && (weaponSync.weapon1 == null || weaponSync.weapon2 == null)) {
            int slot = CheckWeaponSlots();
            int weapon = CheckWeaponType(coll);
            CmdEquipWeapon(slot, weapon);
            CmdDisablePickup(coll.GetComponent<NetworkIdentity>().netId);
        }
    }

    private int CheckWeaponSlots()
    {
        if (weaponSync.weapon1 == null) {
            return 1;
        } else if (weaponSync.weapon2 == null) {
            return 2;
        }
        return 0;
    }

    private int CheckWeaponType(Collider2D coll)
    {
        WeaponPickup pickup = coll.GetComponent<WeaponPickup>();
        for (int i = 0; i < weaponSync.weaponType.Length; i++) {
            if (weaponSync.weaponType[i] == pickup.weaponReference) {
                return i + 1;
            }
        }
        return 0;
    }

    [Command]
    void CmdEquipWeapon(int slotIndex, int weaponIndex)
    {
        if (slotIndex == 1) {
            weaponSync.slot1 = weaponIndex;
        } else if (slotIndex == 2) {
            weaponSync.slot2 = weaponIndex;
        }
    }

    [Command]
    void CmdDisablePickup(NetworkInstanceId pickupID)
    {
        GameObject pickup = ClientScene.FindLocalObject(pickupID);
        pickup.GetComponent<PickupBehaviour>().Disable();
    }
}
