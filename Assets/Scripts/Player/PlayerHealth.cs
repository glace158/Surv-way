using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject Train;    
    Animator anim;
    Transform tran;
    Movement movement;
    Train_Spawn train_Spawn;
    CapsuleCollider player_col;
    public bool over = false;

    void Awake()
    {
        try
        {
            anim = GetComponent<Animator>();
            tran = GetComponent<Transform>();
            movement = GetComponent<Movement>();
            train_Spawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<Train_Spawn>();
            player_col = GetComponent<CapsuleCollider>();//플레이어 캡슐 콜라이더 가져옴
        }
        catch { }

    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("MapOut"))
        {
            player_col.enabled = false;
            movement.enabled = false;
            GameObject.Find("GameObject").GetComponent<ScoreCheck>().enabled = false;
            train_Spawn.enabled = false;
            anim.SetFloat("Run", 0f);
            over = true;
            anim.SetTrigger("Hit");
            GameObject.FindGameObjectWithTag("UI").transform.Find("UI").GetComponent<UIManager>().SendMessage("GameOver");
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Back"))
        {
            movement.enabled = false;
            GameObject.Find("GameObject").GetComponent<ScoreCheck>().enabled = false;
            train_Spawn.enabled = false;

            anim.SetFloat("Run", 0f);
            anim.SetTrigger("BackDie");
            over = true;

        }

        if (coll.gameObject.CompareTag("Train") && over == true)
        {
            player_col.enabled = false;
            anim.SetTrigger("Die");
            GameObject.FindGameObjectWithTag("UI").transform.Find("UI").GetComponent<UIManager>().SendMessage("GameOver");
        }
        else if (coll.gameObject.CompareTag("Train"))
        {
            player_col.enabled = false;
            movement.enabled = false;
            GameObject.Find("GameObject").GetComponent<ScoreCheck>().enabled = false;
            train_Spawn.enabled = false;
            anim.SetFloat("Run", 0f);
            over = true;
            anim.SetTrigger("Hit");
            GameObject.FindGameObjectWithTag("UI").transform.Find("UI").GetComponent<UIManager>().SendMessage("GameOver");
        }
    }

    public void Life()
    {
        player_col.enabled = true;
        movement.enabled = true;
        GameObject.Find("GameObject").GetComponent<ScoreCheck>().enabled = true;
        train_Spawn.enabled = true;
        anim.SetFloat("Run", 0f);
        over = false;
        anim.SetTrigger("Replay");
    }

    public void BackTrain()//플레이어 뒷걸음 사망시
    {
        bool[] TrainBanLine = { true, true, true, true, true };//열차 스폰 금지선로 확인

        int backmap = GameManager.Instance.BackMap;

        switch (backmap)
        {
            case 0:
            case 5://0: 5line, 5: 3lineS, 6: 3line_1S
            case 6: 
                break;
            case 1:
            case 2://1: 3line, 2: 3lineE
                for (int i = 0; i < 2; i++)
                {
                    TrainBanLine[i] = false;
                }
                break;
            case 3:
            case 4://3: 3line_1, 4: 3line_1E
                for (int i = 0; i < 6; i += 4)
                {
                    TrainBanLine[i] = false;
                }
                break;
        }

        int j = 0;
        for (int i = -8; i <= 8; i += 4)
        {
            if (TrainBanLine[j] == true)
            {
                Instantiate(Train, new Vector3(i, 1.9f, tran.position.z - 40), Quaternion.identity);
            }
            j++;
        }
    }
}
