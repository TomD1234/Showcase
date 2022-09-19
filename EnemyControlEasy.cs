using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlEasy : MonoBehaviour
{
    [SerializeField] private List<GameObject> unitTypes;

    [SerializeField] private int coinMineUpgradePrice;
    [SerializeField] private int knifeManPrice;
    [SerializeField] private int axeManPrice;
    [SerializeField] private int shieldManPrice;
    [SerializeField] private int bowManPrice;
    [SerializeField] private int wizardManPrice;

    private float timeBetweenCoins = 0.17f;
    private float lastCoinTime;
    private int coinCounter = 100;
    private int coinMineLvl = 1;
    private GameObject newUnit;
    private int randomGetal;

    private void Update()
    {
        if (lastCoinTime == 0 | Time.time - lastCoinTime > timeBetweenCoins & coinCounter < 450)
        {
            coinCounter++;
            lastCoinTime = Time.time;
            randomGetal = Mathf.FloorToInt(Random.Range(0, 5));
        }

        if (coinCounter>=100 & randomGetal==0)
        {
            spawnKnifeMan();
        }
        if (coinCounter >= 100 & randomGetal == 1)
        {
            spawnAxeMan();
        }
        if (coinCounter >= 100 & randomGetal == 2)
        {
            spawnShieldMan();
        }
        if (coinCounter >= 100 & randomGetal == 3)
        {
            spawnBowMan();
        }
        if (coinCounter >= 100 & randomGetal == 4)
        {
            spawnWizardMan();
        }


    }
    public void spawnKnifeMan()
    {
        if (coinCounter >= knifeManPrice)
        {
            coinCounter = coinCounter - knifeManPrice;
            newUnit= Instantiate(unitTypes[0], GameObject.Find("TowerEnemy").transform.position + new Vector3(-1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
            newUnit.layer = LayerMask.NameToLayer("Enemy");
            newUnit.GetComponent<UnitBehaviourScript>().unitSpeed = -newUnit.GetComponent<UnitBehaviourScript>().unitSpeed;
            newUnit.transform.eulerAngles = new Vector3(0, 180, 0);
            newUnit.transform.Find("redBar").transform.position = new Vector3(newUnit.transform.Find("redBar").transform.position.x, newUnit.transform.Find("redBar").transform.position.y, 1);
        }
    }

    public void spawnAxeMan()
    {
        if (coinCounter >= axeManPrice)
        {
            coinCounter = coinCounter - axeManPrice;
            newUnit = Instantiate(unitTypes[1], GameObject.Find("TowerEnemy").transform.position + new Vector3(-1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
            newUnit.layer = LayerMask.NameToLayer("Enemy");
            newUnit.GetComponent<UnitBehaviourScript>().unitSpeed = -newUnit.GetComponent<UnitBehaviourScript>().unitSpeed;
            newUnit.transform.eulerAngles = new Vector3(0, 180, 0);
            newUnit.transform.Find("redBar").transform.position = new Vector3(newUnit.transform.Find("redBar").transform.position.x, newUnit.transform.Find("redBar").transform.position.y, 1);
        }
    }
    public void spawnShieldMan()
    {
        if (coinCounter >= shieldManPrice)
        {
            coinCounter = coinCounter - shieldManPrice;
            newUnit = Instantiate(unitTypes[2], GameObject.Find("TowerEnemy").transform.position + new Vector3(-1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
            newUnit.layer = LayerMask.NameToLayer("Enemy");
            newUnit.GetComponent<UnitBehaviourScript>().unitSpeed = -newUnit.GetComponent<UnitBehaviourScript>().unitSpeed;
            newUnit.transform.eulerAngles = new Vector3(0, 180, 0);
            newUnit.transform.Find("redBar").transform.position = new Vector3(newUnit.transform.Find("redBar").transform.position.x, newUnit.transform.Find("redBar").transform.position.y, 1);
        }
    }
    public void spawnBowMan()
    {
        if (coinCounter >= bowManPrice)
        {
            coinCounter = coinCounter - bowManPrice;
            newUnit = Instantiate(unitTypes[3], GameObject.Find("TowerEnemy").transform.position + new Vector3(-1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
            newUnit.layer = LayerMask.NameToLayer("Enemy");
            newUnit.GetComponent<RangedUnitBehaviourScript>().unitSpeed = -newUnit.GetComponent<RangedUnitBehaviourScript>().unitSpeed;
            newUnit.transform.eulerAngles = new Vector3(0, 180, 0);
            newUnit.transform.Find("redBar").transform.position = new Vector3(newUnit.transform.Find("redBar").transform.position.x, newUnit.transform.Find("redBar").transform.position.y, 1);
        }
    }
    public void spawnWizardMan()
    {
        if (coinCounter >= wizardManPrice)
        {
            coinCounter = coinCounter - wizardManPrice;
            newUnit = Instantiate(unitTypes[4], GameObject.Find("TowerEnemy").transform.position + new Vector3(-1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
            newUnit.layer = LayerMask.NameToLayer("Enemy");
            newUnit.GetComponent<RangedUnitBehaviourScript>().unitSpeed = -newUnit.GetComponent<RangedUnitBehaviourScript>().unitSpeed;
            newUnit.transform.eulerAngles = new Vector3(0, 180, 0);
            newUnit.transform.Find("redBar").transform.position = new Vector3(newUnit.transform.Find("redBar").transform.position.x, newUnit.transform.Find("redBar").transform.position.y, 1);
        }
    }
    public void upgradeCoinMines()
    {
        if (coinCounter >= coinMineUpgradePrice)
        {
            coinCounter = coinCounter - coinMineUpgradePrice;
            timeBetweenCoins = timeBetweenCoins - 0.25f;
            coinMineLvl++;
        }
    }
}
