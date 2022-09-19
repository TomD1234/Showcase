using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSceneScript : MonoBehaviour
{
    public void restartScene()
    {
        StarPowerUpCounter.powerUpsCount = 0;
        ScoreCounter.scoreCounter = 0;
        SceneManager.LoadScene("Level0");
    }
}
