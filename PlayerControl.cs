using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> unitTypes;

    [SerializeField] private int coinMineUpgradePrice;
    [SerializeField] private int knifeManPrice;
    [SerializeField] private int axeManPrice;
    [SerializeField] private int shieldManPrice;
    [SerializeField] private int bowManPrice;
    [SerializeField] private int wizardManPrice;

    private float timeBetweenCoins=0.4f;
    private float lastCoinTime;
    private int coinCounter=100;
    private int coinMineLvl=1;

    private void Update()
    {
        if (lastCoinTime == 0 | Time.time - lastCoinTime > timeBetweenCoins & coinCounter < 450)
        {
            coinCounter++;
            lastCoinTime = Time.time;
        }
        GameObject.Find("CoinAmount").GetComponent<Text>().text = coinCounter.ToString() + "/450";
    }
    public void spawnKnifeMan()
    {
        if (coinCounter>=knifeManPrice)
        {
            coinCounter = coinCounter - knifeManPrice;
            Instantiate(unitTypes[0], GameObject.Find("TowerFriend").transform.position + new Vector3(1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
        }
    }
        
    public void spawnAxeMan()
    {
        if (coinCounter >= axeManPrice)
        {
            coinCounter = coinCounter - axeManPrice;
            Instantiate(unitTypes[1], GameObject.Find("TowerFriend").transform.position + new Vector3(1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
        }
    }
    public void spawnShieldMan()
    {
        if (coinCounter >= shieldManPrice)
        {
            coinCounter = coinCounter - shieldManPrice;
            Instantiate(unitTypes[2], GameObject.Find("TowerFriend").transform.position + new Vector3(1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
        }
    }
    public void spawnBowMan()
    {
        if (coinCounter >= bowManPrice)
        {
            coinCounter = coinCounter - bowManPrice;
            Instantiate(unitTypes[3], GameObject.Find("TowerFriend").transform.position + new Vector3(1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
        }
    }
    public void spawnWizardMan()
    {
        if (coinCounter >= wizardManPrice)
        {
            coinCounter = coinCounter - wizardManPrice;
            Instantiate(unitTypes[4], GameObject.Find("TowerFriend").transform.position + new Vector3(1, Random.Range(-0.35f, -1.5f)), Quaternion.identity);
        }
    }
    public void upgradeCoinMines()
    {
        if (coinCounter>=coinMineUpgradePrice)
        {
            coinCounter = coinCounter - coinMineUpgradePrice;
            timeBetweenCoins = timeBetweenCoins - 0.1f;
            coinMineLvl++;
        }
        if (coinMineLvl==4)
        {
            GameObject.Find("UpgradeCoinButton").GetComponent<Button>().interactable = false;
        }
    }
}
