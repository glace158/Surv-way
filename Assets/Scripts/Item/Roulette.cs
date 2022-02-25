using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    public playArray[] player01;

    public Text OKText;
    public Text CoinText;
    public Text NameText;

    int Playnum;
    int Playnum01;
    GameObject character;
    GameObject cd;
    Vector3 offset;
    Animator animUI;
    bool again = false;
    public GameObject[] scene;

    private void Awake()
    {
        animUI = GetComponent<Animator>();

        for (int i = 0; i < scene.Length; i++)
        {
            scene[i].SetActive(false);
        }
        switch (DataController.Instance.gameData.CurrentScene)
        {
            case 2:
                scene[0].SetActive(true);
                break;
            case 4:
                scene[1].SetActive(true);
                break;
            case 3:
                scene[2].SetActive(true);
                break;
            case 5:
                scene[3].SetActive(true);
                break;
        }
    }

    void RouletteStart()
    {
        if (DataController.Instance.gameData.Coin >= 50)
        {
            DataController.Instance.gameData.Coin -= 50;
            CoinText.text = DataController.Instance.gameData.Coin + "";

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CollectionUI>().enabled = false;

            Playnum = RandomFN(0, player01.Length);
            Playnum01 = RandomFN(0, player01[Playnum].data.Length);
            character = Instantiate(player01[Playnum].data[Playnum01], new Vector3(0.067f, 29f, -4f), Quaternion.Euler(0, 180, 0));
            character.transform.localScale = new Vector3(0, 0, 0);

            if(DataController.Instance.gameData.UnLocked[Playnum].data[Playnum01] == true)
            {
                OKText.text = "\n+ 25C";
                again = true;
                DataController.Instance.SaveGameData();
            }
            else
            {
                DataController.Instance.gameData.UnLocked[Playnum].data[Playnum01] = true;
                DataController.Instance.SaveGameData();
            }


            InvokeRepeating("CamMove", 2f, 0.05f);
            Invoke("TrainPlay", 3f);
            InvokeRepeating("CharacterSpawn", 5f, 0.02f);
        }
        else
        {
            animUI.SetTrigger("Need");
        }
    }

    void TrainPlay()
    {
        GameObject.FindGameObjectWithTag("Train").GetComponent<TrainAnime>().SendMessage("TrainPlay");
    }

    void CamMove()
    {
        try
        {
            cd = GameObject.FindGameObjectWithTag("UI").transform.Find("ScrollRect").Find("Contents").gameObject;
            offset = cd.transform.position;
        }
        catch { }

        Vector3 targetCamPos = new Vector3(-9.4f, offset.y, offset.z);

        cd.transform.position = Vector3.Lerp(cd.transform.position, targetCamPos, 5f * Time.deltaTime);
    }

    void CharacterSpawn()
    {
        if (character.transform.localScale.z <= 0.5f)
        {
            character.transform.localScale = new Vector3(character.transform.localScale.x + 0.05f, character.transform.localScale.y + 0.05f, character.transform.localScale.z + 0.05f);
        }

        character.transform.Rotate(0, 90 * Time.deltaTime * 1, 0, Space.World);

        if (again == true)
        {
            DataController.Instance.gameData.Coin += 25;

            CoinText.text = DataController.Instance.gameData.Coin + "";
            again = false;
        }

        NameText.text = player01[Playnum].data[Playnum01].name;
        OKText.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeSceneLoad(bool playScene)
    {
        if (playScene == true)
        {
            SceneManager.LoadScene(1);
        }
        else if (playScene == false)
        {
            int sc = DataController.Instance.gameData.CurrentScene;
            SceneManager.LoadSceneAsync(sc);
        }
    }

    public void ChangePlayer(string code)
    {
        string[] split = code.Trim().Split('-');
        int num = int.Parse(split[0]);
        int num2 = int.Parse(split[1]);
        DataController.Instance.gameData.CurrentCharacter[0] = num;
        DataController.Instance.gameData.CurrentCharacter[1] = num2;

        switch (num)
        {
            case 0:
                SceneManager.LoadScene(2);
                DataController.Instance.gameData.CurrentScene = 2;
                break;
            case 1:
                SceneManager.LoadScene(4);
                DataController.Instance.gameData.CurrentScene = 4;
                break;
            case 2:
                SceneManager.LoadScene(3);
                DataController.Instance.gameData.CurrentScene = 3;
                break;
            case 3:
                SceneManager.LoadScene(5);
                DataController.Instance.gameData.CurrentScene = 5;
                break;
        }
    }

    int RandomFN(int min, int max)//일반 램던 뽑기 메소드
    {
        return Random.Range(min, max);
    }
}
