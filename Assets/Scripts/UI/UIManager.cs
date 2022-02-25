using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable] //<== MonoBehaviour가 아닌 클래스에 대해 Inspector에 나타납니다.  
public class playArray
{
    public GameObject[] data;
}

public class UIManager : MonoBehaviour
{
    public GameObject Ad;

    public playArray[] player;

    public GameObject main;
    public GameObject CoinRewardButton;
    public GameObject LifeRewardButton;
    public GameObject CharacterRewardButton;
    bool isPaused = false;
    public GameObject SoundButton;
    public RectTransform Floating_Joystick;
    public RectTransform Variable_Joystick;
    public RectTransform Lay_Button;

    GameObject Cplayer;
    GameObject train;
    int StartTopScore;

    public Text TopText;
    public Text GetCoin;
    public Text Pause;
    public GameObject PausePannel;
    public int PauseTime = 3;
    Animator animUI;

    private void Start()
    {
        try
        {
            int num = DataController.Instance.gameData.CurrentCharacter[0];
            int num2 = DataController.Instance.gameData.CurrentCharacter[1];
            Cplayer = Instantiate(player[num].data[num2], new Vector3(0, 0.08f, -15f), Quaternion.identity);
            Cplayer.GetComponent<AudioListener>().enabled = true;

            if (DataController.Instance.gameData.Audio == true)
            {
                SoundButton.GetComponent<Toggle>().isOn = true;
            }
            else
            {
                SoundButton.GetComponent<Toggle>().isOn = false;
            }

        }
        catch{}
    }

    private void Awake()
    {
        Application.targetFrameRate = 30;
        animUI = GetComponent<Animator>();
        DataController.Instance.gameData.NowGetCoin = 0;
    }

    public void UIsetting()//UI 셋팅
    {
        if (DataController.Instance.gameData.VariableJoystick == true)
        {
            Floating_Joystick.gameObject.SetActive(false);
            Variable_Joystick.gameObject.SetActive(true);
            Variable_Joystick.anchoredPosition = DataController.Instance.gameData.JoystickPosition;
        }
        else
        {
            Floating_Joystick.gameObject.SetActive(true);
            Variable_Joystick.gameObject.SetActive(false);
        }

        Lay_Button.anchoredPosition = DataController.Instance.gameData.LayPosition;
    }

    public void Sound()//소리 옵션
    {
        if(SoundButton.GetComponent<Toggle>().isOn == true)
        {
            DataController.Instance.gameData.Audio = true;
        }
        else
        {
            DataController.Instance.gameData.Audio = false;
        }
    }

    public void GameOver()//게임 오버
    {
        animUI.SetTrigger("GameOver");
        TopText.text = string.Format("Top " + DataController.Instance.gameData.HighScore);
        GetCoin.text = string.Format(DataController.Instance.gameData.NowGetCoin + " Coin");

        if (DataController.Instance.gameData.NowGetCoin != 0)
        {
            CoinRewardButton.SetActive(true);
        }

        if (DataController.Instance.gameData.RePlayGame == false)
        {
            LifeRewardButton.SetActive(true);
        }
        else
        {
            DataController.Instance.gameData.RePlayGame = false;
        }

        if(DataController.Instance.gameData.Coin >= 50)
        {
            CharacterRewardButton.SetActive(true);
        }
        
        DataController.Instance.SaveGameData();
        
    }

    public void replay()//생명 추가
    {
        animUI.SetTrigger("Replay");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().SendMessage("Life");
    }

    public void ChangeSceneLoad(bool playScene)//플레이어 선택창
    {
        if(playScene == true)
        {
            DataController.Instance.SaveGameData();
            SceneManager.LoadScene(1);
        }
        else if(playScene == false)
        {
            DataController.Instance.SaveGameData();
            int sc = DataController.Instance.gameData.CurrentScene;
            SceneManager.LoadSceneAsync(sc);
        }
        
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = true;
            /* 앱이 비활성화 되었을 때 처리 */
            DataController.Instance.SaveGameData();
        }
        else
        {
            if (isPaused)
            {
                isPaused = false;
                /* 앱이 활성화 되었을 때 처리 */
            }
        }
    }

    public void GameStart()//게임 시작
    {
        Movement movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        ScoreCheck scoreCheck = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreCheck>();
        Train_Spawn train_Spawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<Train_Spawn>();

        StartTopScore = DataController.Instance.gameData.HighScore;
        animUI = GetComponent<Animator>();
        movement.enabled = true;
        gameManager.enabled = true;
        scoreCheck.enabled = true;
        train_Spawn.enabled = true;

        animUI.SetBool("Play", true);

        animUI.SetBool("Play", true);

        Invoke("Destroymain", 1.2f);
    }
    void Destroymain()
    {
        main.SetActive(false);
    }

    public void GamePause(bool button)//게임 일시정지
    {
        if(button == true)
        {
            Time.timeScale = 0;
        }
        else if(button == false)
        {
            Time.timeScale = 1;
        }
        DataController.Instance.SaveGameData();
    }

    public void GameReturnMain()//메인화면 돌아가기
    {
        float[] Sprobs = { 25, 75 };
        float SCall = Choose(Sprobs);
        if (SCall == 0f)
        {
            Ad.GetComponent<AdmobScreenAd>().ShowInterstitial();
        }

        if ((int)GameManager.Instance.score > DataController.Instance.gameData.HighScore)
        {
            DataController.Instance.gameData.HighScore = (int)GameManager.Instance.score;

        }
        DataController.Instance.SaveGameData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    float Choose(float[] probs)//확률 랜덤 뽑기 메소드
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;// 확률을 모두 더함
        }

        float randomPoint = Random.value * total;//0.0에서 1.0 사이의 임의의 수를 반환하고 total를 곱함

        for (int i = 0; i < probs.Length; i++)//계산
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;//Random.value가 1를 반환할시때를 대비해서
    }


}
