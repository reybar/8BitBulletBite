using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour {

    public GameObject player;
    public Transform weaponLoc;
    public Transform weapon;
    public int ammo;
    public Pistol pistol;
    public Sniper sniper;
    public Text displayAmmo;
    // Use this for initialization
    void Start () {

        player = transform.root.gameObject;
        weaponLoc = player.transform.Find("Weapon pivot").Find("Weapon");
        displayAmmo = GetComponent<Text>();

        
    }
	
	// Update is called once per frame
	void Update () {
        
        displayAmmo.text = " ";
        foreach(Transform child in weaponLoc) {
            if(child.gameObject.activeSelf) {
                weapon = child;
                if(weapon.name == "Pistol(Clone)") {
                    pistol = weapon.GetComponent<Pistol>();
                    ammo = pistol.ammo;
                    displayAmmo.text = ammo.ToString();
                }
                if(weapon.name == "Sniper(Clone)") {
                    sniper = weapon.GetComponent<Sniper>();
                    ammo = sniper.ammo;
                    displayAmmo.text = ammo.ToString();
                }
            }
            
        }
        
    }
}
