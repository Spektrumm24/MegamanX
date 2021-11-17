using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text enemyCountText;
    int enemyCount;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        enemyCount = 5;
        enemyCountText.text = enemyCount.ToString();
    }
    private void Update()
    {
        if(enemyCount == 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void killEnemy()
    {
        enemyCount--;
        enemyCountText.text = enemyCount.ToString();
    }
}
