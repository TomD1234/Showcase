using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourScript : MonoBehaviour
{
    private float maxHealth;
    public float health=1000;
    private Transform healthBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        healthBar = gameObject.transform.Find("HealthbarOrigin");
    }

    // Update is called once per frame
    void Update()
    {
        if (health<0)
        {
            health = 0;
        }
        healthBar.localScale = new Vector3(health / maxHealth, 1, 1);
    }
}
