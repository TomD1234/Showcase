using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private int hitDamage;
    private float projectileSpeed;
    private float unitSpeed;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitDamage = GetComponentInParent<RangedUnitBehaviourScript>().hitDamage;
        unitSpeed = GetComponentInParent<RangedUnitBehaviourScript>().unitSpeed;
        projectileSpeed= GetComponentInParent<RangedUnitBehaviourScript>().projectileSpeed;
        Physics2D.IgnoreLayerCollision(gameObject.layer,gameObject.layer);
        Physics2D.IgnoreLayerCollision(gameObject.layer, GetComponentInParent<RangedUnitBehaviourScript>().gameObject.layer);
        rb.velocity = new Vector2(projectileSpeed*Mathf.Sign(unitSpeed),0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            collision.gameObject.GetComponent<TowerBehaviourScript>().health = collision.gameObject.GetComponent<TowerBehaviourScript>().health - hitDamage;
        }
        if (collision.gameObject.tag == "Melee")
        {
            collision.gameObject.GetComponent<UnitBehaviourScript>().health = collision.gameObject.GetComponent<UnitBehaviourScript>().health - hitDamage;
        }
        if (collision.gameObject.tag == "Ranged")
        {
            collision.gameObject.GetComponent<RangedUnitBehaviourScript>().health = collision.gameObject.GetComponent<RangedUnitBehaviourScript>().health - hitDamage;
        }
        Destroy(gameObject);
    }
}
