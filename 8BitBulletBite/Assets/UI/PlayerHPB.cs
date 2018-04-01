using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPB : MonoBehaviour {
    public GameObject fill;
    public GameObject player;
    private float currHealth;
    private float maxHealth;
    private float healthFill;
    
	// Use this for initialization
	void Start () {
        
          
       maxHealth = player.GetComponent<Health>().maxHealth;
        
        
	}
	
	// Update is called once per frame
	void Update () {
        
        currHealth = player.GetComponent<Health>().currHealth;
        healthFill = Mathf.Clamp(currHealth / maxHealth, 0, maxHealth);
        fill.transform.localScale = new Vector2(healthFill, fill.transform.localScale.y);
    }
}
