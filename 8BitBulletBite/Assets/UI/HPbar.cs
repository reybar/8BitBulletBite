using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPbar : MonoBehaviour {

    public GameObject fill;
    public Health health;
    private float currHealth;
    private float maxHealth;
    private float healthFill;

    void Start () {
        maxHealth = health.maxHealth;
    }
	
	void Update () {
        currHealth = health.currHealth;
        healthFill = Mathf.Clamp(currHealth / maxHealth, 0, maxHealth);
        fill.transform.localScale = new Vector2(healthFill, fill.transform.localScale.y);
	}
}
