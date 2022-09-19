using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviourScript : MonoBehaviour
{
    public float health=100;
    public float timeBetweenHits=1;
    public float deathAnimLength;
    public float unitSpeed;
    [SerializeField] int hitDamage=10;

    private Rigidbody2D rb;
    private float lastHitTime;
    private Transform healthBar;
    private float maxHealth;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float deathTimer;
    private bool checkDeathTimer=true;
    private int collisionCounter;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1 * unitSpeed, 0);
        healthBar = gameObject.transform.Find("HealthbarOrigin");
        animator = gameObject.transform.Find("knifeMan").GetComponent<Animator>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionCounter++;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time-lastHitTime>timeBetweenHits)
        {
            if (collision.gameObject.tag=="Tower")
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
            lastHitTime = Time.time;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collisionCounter--;
    }

    private void Update()
    {
        healthBar.localScale = new Vector3(health / maxHealth, 1, 1);
        if (health<=0)
        {
            health = 0;
            animator.SetBool("IsDead", true);
            boxCollider.enabled = false;
            rb.velocity = new Vector2(0, 0);
            if (checkDeathTimer)
            {
                deathTimer = Time.time;
                checkDeathTimer = false;
            }

            if (Time.time-deathTimer>deathAnimLength)
            {
                Destroy(gameObject);
            }
        }

        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        if (collisionCounter>0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //rb.velocity = new Vector2(0, 0);
            animator.SetBool("IsAttacking", true);
        }
        if (collisionCounter==0 & health>0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(1 * unitSpeed, 0);
            animator.SetBool("IsAttacking", false);
        }
    }
}
