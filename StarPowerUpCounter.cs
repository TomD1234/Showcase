using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPowerUpCounter : MonoBehaviour
{
    public static int powerUpsCount;
    [SerializeField] private GameObject powerUpText;

    void Update()
    {
        powerUpText.GetComponent<Text>().text = "PowerUps: "+powerUpsCount.ToString();
    }
}
