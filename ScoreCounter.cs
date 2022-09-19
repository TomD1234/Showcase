using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static int scoreCounter;
    [SerializeField] private GameObject scoreText;

    private void Update()
    {
        scoreText.GetComponent<Text>().text = "Score: " + scoreCounter;
    }
}
