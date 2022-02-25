using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train_Spawn : MonoBehaviour
{
    public GameObject[] Train;//기차 종류 저장

    [HideInInspector] public int Up_Down;
    [HideInInspector] public int SpawnLine = 0;
    [HideInInspector] public int TrainType = 0;

    GameObject playerObject;

    bool[] TrainBanLine = { true, true, true, true, true };//열차 스폰 금지선로 확인
    bool[] SpTrainLine = { false, false, false, false, false };//현재 열차 위치

    float RoutineTime = 0;
    float T = 5;
    int TrainLevel = 1;
    bool tt = true;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()//업데이트
    {
        RoutineTime += Time.deltaTime * 1;//열차 스폰시간 계산
    }

    void Update()
    {
        if (RoutineTime > T)
        {
            int count = RandomFN(0, 4);

            for (int i = 0; i < count; i++)
            {
                bool AllBan = false;
                Up_Down = RandomFN(0, 2);//열차 상단스폰(1), 하단스폰(0)
                TrainSpLineBan();// 현재 열차가 스폰된 선로 밴
                SpawnBan(Up_Down, out AllBan);//열차 선로 밴

                int[] LineXpos = { -8, -4, 0, 4, 8 };

                SpawnTrain(out TrainType, AllBan, out SpawnLine);// 열차 스폰

                int SpAnlge = Up_Down == 1 ? 180 : 0;
                int Sppos = Up_Down == 1 ? 40 : -40;

                if (AllBan == true)
                {
                    break;
                }
                Instantiate(Train[TrainType], new Vector3(LineXpos[SpawnLine], 1.9f, playerObject.transform.position.z + Sppos), Quaternion.Euler(0, SpAnlge, 0));//열차 스폰

                RoutineTime = 0f;//스폰시간 초기화
            }
        }
        phase();
    }

    void TrainSpLineBan()
    {
        //현재 열차가 있는 위치 밴
        try
        {
            GameObject[] SaveTrain = (GameObject.FindGameObjectsWithTag("Train"));

            for (int i = 0; i <= SaveTrain.Length - 1; i++)
            {
                float Tpos = SaveTrain[i].transform.position.x;
                switch (Tpos)
                {
                    case -8f:
                        SpTrainLine[0] = true;
                        break;
                    case -4f:
                        SpTrainLine[1] = true;
                        break;
                    case 0f:
                        SpTrainLine[2] = true;
                        break;
                    case 4f:
                        SpTrainLine[3] = true;
                        break;
                    case 8f:
                        SpTrainLine[4] = true;
                        break;
                }
            }
        }
        catch
        {
            Debug.Log("No Train");
        }
    }

    void SpawnBan(int UpandDown, out bool AllBan)//선로 밴 설정 매소드
    {
        int frontMap = GameManager.Instance.FrontMap;//플레이어 앞에 있는 맵 가져오기

        int backmap = GameManager.Instance.BackMap;

        int WhatBan = UpandDown == 1 ? frontMap : backmap;

        for (int i = 0; i < TrainBanLine.Length; i++)
        {
            TrainBanLine[i] = true;
        }

        switch (WhatBan)
        {
            case 0://0: 5line
                break;
            case 1:
            case 2:
            case 5: //1: 3line, 2: 3lineE, 5: 3lineS
                if(WhatBan == 5 && UpandDown == 0)
                {
                    for (int i = 0; i < TrainBanLine.Length; i++)
                    {
                        TrainBanLine[i] = true;
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    TrainBanLine[i] = false;
                }
                break;
            case 3:
            case 4:
            case 6://3: 3line_1, 4: 3line_1E, 6: 3line_1S
                if (WhatBan == 6 && UpandDown == 0)
                {
                    for (int i = 0; i < TrainBanLine.Length; i++)
                    {
                        TrainBanLine[i] = true;
                    }
                }
                for (int i = 0; i < 6; i += 4)
                {
                    TrainBanLine[i] = false;
                }
                break;
        }

        for (int i = 0; i < 5; i++)
        {
            if (SpTrainLine[i] == true)//만약 열차가 선로에 있다면
            {
                TrainBanLine[i] = false;//열차가 있는 선로를 밴
                SpTrainLine[i] = false;//다시 false로 바꿈
            }
        }


        int BanCount = 0;
        //밴되어 있는 선로 개수 카운트
        foreach (bool elem in TrainBanLine)
        {
            if (elem == false)
            {
                BanCount += 1;
            }
        }

        if (BanCount >= 5)
        {
            AllBan = true;
            BanCount = 0;
        }
        else
        {
            AllBan = false;
        }
    }



    void SpawnTrain(out int TrainType, bool AllBan, out int SpawnLine)//기차 스폰 매소드
    {
        TrainType = RandomFN(0, TrainLevel);//열차종류 선택
        while (true)
        {
            SpawnLine = RandomFN(0, 5);//열차 스폰 선로 지정

            if (TrainBanLine[SpawnLine] == true)//열차 스폰 위치가 밴되어 있으면 다시 뽑기
            {
                break;
            }
            if (AllBan == true)
            {
                AllBan = false;
                break;
            }
        }
    }

    void phase()//페이즈 메소드
    {
        if ((int)GameManager.Instance.score % 200 != 0)
        {
            tt = true;
        }

        if ((int)GameManager.Instance.score % 200 == 0 && TrainLevel < Train.Length && tt == true && (int)GameManager.Instance.score != 0)
        {
            TrainLevel += 1;
            tt = false;
            Debug.Log("phase UP!");
        }

        if ((int)GameManager.Instance.score % 200 == 0 && T != 0 && tt == true && (int)GameManager.Instance.score != 0)
        {
            T -= 0.5f;
            if(T < 0)
            {
                T = 0;
            }
        }
    }

    int RandomFN(int min, int max)//일반 램던 뽑기 메소드
    {
        return Random.Range(min, max);
    }
}
