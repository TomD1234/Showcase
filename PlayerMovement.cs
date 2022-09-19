using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool spacebarWasPressed;
    [SerializeField] private int jumpPower = 10;
    [SerializeField] private float runSpeed = 10;
    bool dWasPressed;
    bool qWasPressed;
    Rigidbody2D rb;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask playerMask;
    AudioSource[] sounds;
    AudioSource jumpSound;
    AudioSource doubleJumpSound;

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
        sounds = GetComponents<AudioSource>();
        jumpSound = sounds[0];
        doubleJumpSound = sounds[1];
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            spacebarWasPressed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dWasPressed = true;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            qWasPressed = true;
        }


    }

    private void FixedUpdate()
    {

        rb.velocity = new Vector2(0, rb.velocity.y);

        if (dWasPressed)
        {
            rb.velocity = new Vector2(1f*runSpeed, rb.velocity.y);
            dWasPressed = false;
        }

        if (qWasPressed)
        {
            rb.velocity = new Vector2(-1f * runSpeed, rb.velocity.y);
            qWasPressed = false;
        }

        if (Physics2D.OverlapCircleAll(new Vector2(groundCheckTransform.position.x, groundCheckTransform.position.y), 0.1f, playerMask).Length == 0 && StarPowerUpCounter.powerUpsCount ==0) 
        {
            spacebarWasPressed = false;
            return;
        }

        if (Physics2D.OverlapCircleAll(new Vector2(groundCheckTransform.position.x, groundCheckTransform.position.y), 0.1f, playerMask).Length == 0 && StarPowerUpCounter.powerUpsCount > 0 && spacebarWasPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1f * jumpPower);
            spacebarWasPressed = false;
            StarPowerUpCounter.powerUpsCount--;
            doubleJumpSound.Play();
        }

        if (spacebarWasPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1f * jumpPower);
            spacebarWasPressed = false;
            jumpSound.Play();
        }
    }
}