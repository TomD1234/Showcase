using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    string activeSceneName;
    int nextLevelNum;
    string nextLevelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            activeSceneName = SceneManager.GetActiveScene().name;
            nextLevelNum =  int.Parse(activeSceneName[5].ToString()) + 1;
            nextLevelName = "Level" + nextLevelNum.ToString();
            SceneManager.LoadScene(nextLevelName);
            collision.gameObject.transform.position = new Vector3(0f, 3f, 0f);
        }
    }
}
