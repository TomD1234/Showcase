using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer!=LayerMask.NameToLayer("Collectables"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer==LayerMask.NameToLayer("Breakable"))
        {
            Destroy(collision.gameObject);
        }
    }
}
