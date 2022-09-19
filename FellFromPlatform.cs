using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class FellFromPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            ScoreCounter.scoreCounter = 0;
            StarPowerUpCounter.powerUpsCount = 0; ;
            DeathCounter.deathCounts++;
            //collision.gameObject.transform.position = new Vector3(0f, 3f, 0f);
            StartCoroutine(RestartScene());
        }
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Level0");
    }
}
