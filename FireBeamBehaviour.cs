using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBeamBehaviour : MonoBehaviour
{
    private int hitDamage;
    private float beamStartTime;
    private void Start()
    {
        beamStartTime = Time.time;
        hitDamage = GetComponentInParent<RangedUnitBehaviourScript>().hitDamage;
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        Physics2D.IgnoreLayerCollision(gameObject.layer, GetComponentInParent<RangedUnitBehaviourScript>().gameObject.layer);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            collision.gameObject.GetComponent<TowerBehaviourScript>().health = collision.gameObject.GetComponent<TowerBehaviourScript>().health - hitDamage*Time.deltaTime;
        }
        if (collision.gameObject.tag == "Melee")
        {
            collision.gameObject.GetComponent<UnitBehaviourScript>().health = collision.gameObject.GetComponent<UnitBehaviourScript>().health - hitDamage * Time.deltaTime;
        }
        if (collision.gameObject.tag == "Ranged")
        {
            collision.gameObject.GetComponent<RangedUnitBehaviourScript>().health = collision.gameObject.GetComponent<RangedUnitBehaviourScript>().health - hitDamage * Time.deltaTime;
        }
    }
    private void Update()
    {
        if (Time.time-beamStartTime>1.333f)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
