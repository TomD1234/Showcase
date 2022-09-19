using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            GetComponent<AudioSource>().Play();
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 2f);
            StarPowerUpCounter.powerUpsCount++;
        }
    }
}
