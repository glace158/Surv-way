using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackKey : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    int sc = DataController.Instance.gameData.CurrentScene;
                    SceneManager.LoadSceneAsync(sc);
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    }
}
