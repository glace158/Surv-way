using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestDta : MonoBehaviour
{
    public Text Coin;
    public Text Top;

    void Start()
    {
        GoogleTest.GetInstance.Init();
    }

    public void Login()
    {
        GoogleTest.GetInstance.Login();
    }

    public void Leaderboard()
    {
        if (GoogleTest.GetInstance.CheckLogin() == true)
        {
            GoogleTest.GetInstance.SendScore();
        }
        else
        {
            GoogleTest.GetInstance.Login();
        }
    }

    public void Save()//클라우드 저장
    {
        if (GoogleTest.GetInstance.CheckLogin() == true)
        {
            string data = string.Format("{0},{1}", DataController.Instance.gameData.Coin, DataController.Instance.gameData.HighScore);

            GoogleTest.GetInstance.SaveToCloud(data);
        }
        else
        {
            GoogleTest.GetInstance.Login();
        }
    }

    public void Load()//클라우드 로드
    {
        if (GoogleTest.GetInstance.CheckLogin() == true)
        {
            GoogleTest.GetInstance.LoadFromCloud();
        }
        else
        {
            GoogleTest.GetInstance.Login();
        }
    }

    public void SetData(string data)
    {
        string[] split = data.Trim().Split(',');

        DataController.Instance.gameData.Coin = int.Parse(split[0]);
        DataController.Instance.gameData.HighScore = int.Parse(split[1]);

        Coin.text = DataController.Instance.gameData.Coin.ToString();
        Top.text = DataController.Instance.gameData.HighScore.ToString();
    }
}
