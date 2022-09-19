using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGunScript : MonoBehaviour
{
    Vector3 triangleTopPoint;
    float gunAngle;
    bool leftMouseButtonWasPressed;
    [SerializeField] GameObject bullet;
    GameObject lastBullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float timeBetweenBullets;
    float timer;
    // Update is called once per frame
    void Update()
    {
        aimGun();
        if (Input.GetMouseButton(1))
        {
            leftMouseButtonWasPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (leftMouseButtonWasPressed && Time.time-timer>timeBetweenBullets)
        {
            shootGun();
            timer = Time.time;
        }
        leftMouseButtonWasPressed = false;
    }

    void aimGun()
    {
        triangleTopPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
        //Debug.Log(Mathf.Atan(triangleTopPoint.y / triangleTopPoint.x) * 180 / Mathf.PI);
        gunAngle = Mathf.Atan(triangleTopPoint.y / triangleTopPoint.x) * 180 / Mathf.PI;

        if (triangleTopPoint.x<0)
        {
            gunAngle = (180 - gunAngle)*-1;
        }
        gameObject.transform.eulerAngles = new Vector3(0, 0, gunAngle);
    }

    void shootGun()
    {
        lastBullet=Instantiate(bullet,GameObject.Find("BulletOut").transform.position, Quaternion.identity);
        lastBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(triangleTopPoint.x,triangleTopPoint.y).normalized*bulletSpeed;
    }

}
