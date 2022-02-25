using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] Map;//맵( 0: 5line, 1: 3line, 2: 3lineE, 3: 3line_1, 4: 3line_1E, 5: 3lineS, 6: 3line_1S )

    Movement movement;

    [HideInInspector]public int WhatMap;//무슨맵을 생성할지 받는 변수
    [HideInInspector] public int backmap;//무슨맵을 생성할지 받는 변수

    int i = 1, count = 2;
    float length;
    bool tt = true;

    int[] backmaps = { 0, 0, 0 };//플레이어 뒤에 있는 맵 번호 저장
    GameObject[] DesMap = new GameObject[3];//맵 임시저장
    bool map_s = false;//맵 스타트 생성 여부

    float R5or3 = 0, R3or3_1 = 0, R3 = 0;//램덤 값 저장

    public static float line_3d_probs = 80f, line_3_Eprobs = 20f;
    float[] probs3_3E = { line_3d_probs, line_3_Eprobs };
    public static float line_5_probs = 90f, line_3_probs = 10f;// 확률 지정
    float[] probs5_3 = { line_5_probs, line_3_probs };

    private void Awake()
    {
        DesMap[0] = Instantiate(Map[0], new Vector3(0, 0, -40), Quaternion.Euler(-90f, 0f, 0f));
        DesMap[1] = Instantiate(Map[0], new Vector3(0, 0, 0), Quaternion.Euler(-90f, 0f, 0f));

        R3or3_1 = Random.Range(0, 2);//3line 또는 3line_1 선택

    }

    private void Update()
    {
        
        MapDestory();

        MapSelect(out WhatMap);

        DesMap[count] = Instantiate(Map[WhatMap], new Vector3(0, 0, 40 * i), Quaternion.Euler(-90f, 0f, 0f));

        backmaps[2] = WhatMap;

        backmaps[0] = backmaps[1];
        backmaps[1] = backmaps[2];

        backmap = backmaps[0];

        i++;

        phase();
        this.enabled = false;
    }

    void MapDestory()//맵 삭제 메소드
    {
        switch (i % 3)
        {
            case 1:
                Destroy(DesMap[2]);
                count = 2;
                break;
            case 2:
                Destroy(DesMap[0]);
                count = 0;
                break;
            case 0:
                Destroy(DesMap[1]);
                count = 1;
                break;
        }
    }

    void MapSelect(out int WhatMap)
    {
        if (R5or3 == 0)//5line 선택시
        {
            WhatMap = 0;//5line
            R5or3 = Choose(probs5_3);//5line 또는 3line 선택
        }
        else if (R5or3 == 1)//3line 선택시
        {
            if (R3or3_1 == 0)//3line 선택시
            {
                if (map_s == false)//3line 스타트 생성
                {
                    WhatMap = 5;//map3S
                    map_s = true;
                }
                else if (R3 == 0)//3line 생성
                {
                    WhatMap = 1;//map3
                }
                else if (R3 == 1)//3line 끝 생성
                {
                    WhatMap = 2;//map3E
                    map_s = false;
                    R5or3 = 0;//5line으로 초기화
                    R3or3_1 = Random.Range(0, 2);//3line 또는 3line_1 선택
                }
                else
                {
                    WhatMap = 0;
                }
                R3 = Choose(probs3_3E);
            }
            else if (R3or3_1 == 1)//3line_1 선택시
            {
                if (map_s == false)//3line_1 스타트 생성
                {
                    WhatMap = 6;//map3_1S
                    map_s = true;
                }
                else if (R3 == 0)//3line_1 생성
                {
                    WhatMap = 3;//map3_1
                }
                else if (R3 == 1)//3line_1 끝 생성
                {
                    WhatMap = 4;//map3_1E
                    map_s = false;
                    R5or3 = 0;
                    R3or3_1 = Random.Range(0, 2);//3line 또는 3line_1 선택
                }
                else
                {
                    WhatMap = 0;
                }
                R3 = Choose(probs3_3E);
            }
            else
            {
                WhatMap = 0;
            }
        }
        else
        {
            WhatMap = 0;
        }
 
    }

    void phase()//페이즈 메소드
    {
        if ((int)GameManager.Instance.score % 300 != 0)
        {
            tt = true;
        }

        if ((int)GameManager.Instance.score % 300 == 0 && line_5_probs != 50 && tt == true && (int)GameManager.Instance.score != 0)
        {
            line_5_probs -= 10;
            line_3_probs += 10;
            tt = false;
        }
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
