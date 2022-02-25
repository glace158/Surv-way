using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteSceneLoad : MonoBehaviour
{
    
    public GameObject Contents;
    public Text CoinText;
    public int SceneNum;

   

    void Start()
    {

        CoinText.text = DataController.Instance.gameData.Coin + "";

        for (int i = 0; i < 6; i++)
        {
            bool unLock = DataController.Instance.gameData.UnLocked[SceneNum].data[i];

            if (unLock == true)
            {
                Contents.transform.GetChild(i + 1).GetChild(1).gameObject.SetActive(true);
                Contents.transform.GetChild(i+ 1).GetComponent<Button>().enabled = true;
                Destroy(Contents.transform.GetChild(i + 1).GetChild(0).gameObject);
            }
        }
    }

    private void Update()
    {
        Destroy(this);
    }
}
