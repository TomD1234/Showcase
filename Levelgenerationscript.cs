using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelgenerationscript : MonoBehaviour
{
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private Transform levelStart;
    [SerializeField] private GameObject player;
    Transform lastLevelPart;
    Transform randomLevelPart;
    private void Awake()
    {
        lastLevelPart=levelStart;
    }

    private void Update()
    {
        if (lastLevelPart.Find("EndPosition").position.x - player.transform.position.x < 20)
        {
            lastLevelPart = spawnLevelPart(lastLevelPart.Find("EndPosition").position + new Vector3(Random.Range(1,5),Random.Range(-3,4),0));
        }
    }

    private Transform spawnLevelPart(Vector3 spawnPosition)
    {
        randomLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        Transform newLevelPart = Instantiate(randomLevelPart, spawnPosition - randomLevelPart.Find("BeginPosition").localPosition, Quaternion.identity);
        return newLevelPart;
    }
}
