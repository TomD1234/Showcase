using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreKeeper : MonoBehaviour
{
    [SerializeField] private GameObject highScoreObject;

    static int previousHighScore;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("HighScore").GetComponent<Text>().text = "High Score: " + previousHighScore;
    }

    // Update is called once per frame
    void Update()
    {

        if (ScoreCounter.scoreCounter > previousHighScore)
        {
            GameObject.Find("HighScore").GetComponent<Text>().text = "High Score: " + ScoreCounter.scoreCounter;
            previousHighScore = ScoreCounter.scoreCounter;
        }

    }
}
