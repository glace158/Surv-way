using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSpawn : MonoBehaviour
{
    public GameObject coin;
    public GameObject Traffic_Light;
    public Slider Traffic_Slider;
    public GameObject StopVignette;

    Movement movement;
    int i = 1;

    public Text coinText;
    float[] Tprobs = { 90, 10 };

    void GetCoin()//코인 증가
    {
        DataController.Instance.gameData.Coin++ ;
        DataController.Instance.gameData.NowGetCoin++;
        coinText.text = string.Format(DataController.Instance.gameData.Coin + "");
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    void TrainStop()
    {
        Traffic_Slider.gameObject.SetActive(true);
        StopVignette.SetActive(true);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Train_Spawn>().enabled = false;
        InvokeRepeating("StopDelay", 0, 0.05f);
    }
    void StopDelay()
    {
        Traffic_Slider.value += 0.5f;
        Tprobs[0] = 100;
        Tprobs[1] = 0;
        
        if (Traffic_Slider.value >= 100)
        { 
            Tprobs[0] = 95;
            Tprobs[1] = 5;
            Traffic_Slider.gameObject.SetActive(false);
            StopVignette.SetActive(false);
            Traffic_Slider.value = 0f;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Train_Spawn>().enabled = true;
            CancelInvoke("StopDelay");
        }
    }

    private void Update()
    {
        CoinManager();
        TrafficManager();
        i++;
        this.enabled = false;
    }

    void CoinManager()//코인 스폰 매니저
    {
        float[] probs = { 60, 40 };
        float CoinSp = Choose(probs);// 0: X, 1: 스폰

        if (CoinSp == 1)
        {
            int frontMap = GameManager.Instance.FrontMap;
            int x, z;
            int minX = 0;
            int maxX = 0;
            switch (frontMap)
            {
                case 0://0: 5line
                    minX = -9;
                    maxX = 9;
                    break;

                case 1:
                case 2:
                case 5: //1: 3line, 2: 3lineE, 5: 3lineS
                    minX = 0;
                    maxX = 9;
                    break;

                case 3:
                case 4:
                case 6://3: 3line_1, 4: 3line_1E, 6: 3line_1S
                    minX = -4;
                    maxX = 4;
                    break;
            }
            x = RandomFN(minX, maxX);
            z = RandomFN(-18, 18);
            Instantiate(coin, new Vector3(x, 1.28f, ( 1 * 40 * i) + z), Quaternion.identity);
        }
    }
    
    void TrafficManager()//신호등 스폰 매니저
    {
        float CoinSp = Choose(Tprobs);// 0: X, 1: 스폰

        if (CoinSp == 1)
        {
            int frontMap = GameManager.Instance.FrontMap;
            int x, z;
            int minX = 0;
            int maxX = 0;
            switch (frontMap)
            {
                case 0://0: 5line
                    minX = -9;
                    maxX = 9;
                    break;

                case 1:
                case 2:
                case 5: //1: 3line, 2: 3lineE, 5: 3lineS
                    minX = 0;
                    maxX = 9;
                    break;

                case 3:
                case 4:
                case 6://3: 3line_1, 4: 3line_1E, 6: 3line_1S
                    minX = -4;
                    maxX = 4;
                    break;
            }
            x = RandomFN(minX, maxX);
            z = RandomFN(-18, 18);
            Instantiate(Traffic_Light, new Vector3(x, 1.7f, (1 * 40 * i) + z), Quaternion.Euler(-120, 0, 40));
        }
    }

    int RandomFN(int min, int max)//일반 램던 뽑기 메소드
    {
        return Random.Range(min, max);
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
