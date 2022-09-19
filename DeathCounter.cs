using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    public static int deathCounts;
    [SerializeField] private GameObject deathCounterText;
    // Update is called once per frame
    void Update()
    {
        deathCounterText.GetComponent<Text>().text = "Deaths: " + deathCounts;
    }
}
