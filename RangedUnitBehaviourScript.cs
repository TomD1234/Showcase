using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnitBehaviourScript : MonoBehaviour
{
    public float health = 100;
    public float timeBetweenHits = 1;
    public float deathAnimLength;
    public float attackRangeSize=6f;
    public float unitSpeed;
    public int hitDamage = 10;
    public float projectileSpeed=3;
    [SerializeField] private GameObject projectile;

    private Rigidbody2D rb;
    private float lastHitTime;
    private Transform healthBar;
    private float maxHealth;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private BoxCollider2D characterBoxCollider;
    private float deathTimer;
    private bool checkDeathTimer = true;
    private int collisionCounter;
    private GameObject newProjectile;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1 * unitSpeed, 0);
        healthBar = gameObject.transform.Find("HealthbarOrigin");
        animator = gameObject.transform.Find("knifeMan").GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

        boxCollider = gameObject.transform.Find("AttackRange").GetComponent<BoxCollider2D>();
        characterBoxCollider = gameObject.GetComponent<BoxCollider2D>();

        boxCollider.size = new Vector2(attackRangeSize,boxCollider.size.y);
        boxCollider.offset = new Vector2(attackRangeSize / 2, boxCollider.offset.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer != collision.gameObject.layer & collision.gameObject.layer != LayerMask.NameToLayer("Projectiles"))
        {
            collisionCounter++;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (lastHitTime == 0 & gameObject.layer!=collision.gameObject.layer & collision.gameObject.layer != LayerMask.NameToLayer("Projectiles"))
        {
            shoot();
            lastHitTime = Time.time;
        }
        if (Time.time - lastHitTime > timeBetweenHits & gameObject.layer != collision.gameObject.layer & collision.gameObject.layer != LayerMask.NameToLayer("Projectiles"))
        {
            shoot();
            lastHitTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.layer != collision.gameObject.layer & collision.gameObject.layer != LayerMask.NameToLayer("Projectiles"))
        {
            collisionCounter--;
        }
    }

    private void Update()
    {
        healthBar.localScale = new Vector3(health / maxHealth, 1, 1);
        if (health <= 0)
        {
            health = 0;
            animator.SetBool("IsDead", true);
            boxCollider.enabled = false;
            characterBoxCollider.enabled = false;
            rb.velocity = new Vector2(0, 0);
            if (checkDeathTimer)
            {
                deathTimer = Time.time;
                checkDeathTimer = false;
            }

            if (Time.time - deathTimer > deathAnimLength)
            {
                Destroy(gameObject);
            }
        }

        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        if (collisionCounter > 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //rb.velocity = new Vector2(0, 0);
            animator.SetBool("IsAttacking", true);
        }
        if (collisionCounter == 0 & health > 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(1 * unitSpeed, 0);
            animator.SetBool("IsAttacking", false);
        }
    }

    private void shoot()
    {
        newProjectile=Instantiate(projectile,gameObject.transform);
        //newProjectile.transform.parent = gameObject.transform;
    }
}
