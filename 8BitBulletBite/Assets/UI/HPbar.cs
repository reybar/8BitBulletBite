using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPbar : MonoBehaviour {

    public GameObject fill;
    public Transform body;
    private float currHealth;
    private float maxHealth;
    private float healthFill;

    // Use this for initialization
    void Start () {
        body = transform.parent;
        maxHealth = body.GetComponent<Health>().maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        currHealth = body.GetComponent<Health>().currHealth;
        healthFill = Mathf.Clamp(currHealth / maxHealth, 0, maxHealth);
        fill.transform.localScale = new Vector2(healthFill, fill.transform.localScale.y);
	}
}
