using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Lay_Button lay_Button;
    GameObject gamecontroller;
    MapManager mapManager;
    CoinSpawn coinSpawn;

    [HideInInspector] public bool isTriggerWall = false;

    bool unMove = false;
    bool Walking = false;
    bool InWater = false;
    float Stoping = 0f;

    Animator anim;
    Rigidbody player_rd;
    CapsuleCollider player_col;
    Vector3 playerLook;


    void Awake()
    {
        try
        {
            gamecontroller = GameObject.FindGameObjectWithTag("GameController");
            anim = GetComponent<Animator>();//플레이어 애니메이터 가져옴
            player_rd = GetComponent<Rigidbody>();//플레이어 리지드바디 가져옴
            player_col = GetComponent<CapsuleCollider>();//플레이어 캡슐 콜라이더 가져옴
            mapManager = gamecontroller.GetComponent<MapManager>();
            coinSpawn = gamecontroller.GetComponent<CoinSpawn>();
        }
        catch { }
    }

    void Start()//처음 플레이어가 정면을 바라보게 함
    {
        lay_Button = GameObject.FindGameObjectWithTag("Lay").GetComponent<Lay_Button>();
        playerLook = transform.position;
        playerLook.y = 0f;
        playerLook.z += 50;
    }

    void FixedUpdate()
    {
        bool lay = lay_Button.Lay;//엎드림 여부 판정
        if (lay)
        {
            unMove = true;//엎드림 참으로 바꿈
            anim.SetBool("Laying", true);//애니메이터의 프로퍼티 "Laying"를 참으로 변환
            if (player_col.center.y >= 0.6f)
            {
                player_col.center = new Vector3(player_col.center.x, player_col.center.y / 5f, player_col.center.z);
                player_col.height = player_col.height / 5f;
            }
        }
        else
        {
            anim.SetFloat("Run", 0f);//애니메이터의 프로퍼티 "Run"에 0 대입
            anim.SetBool("Laying", false);//애니메이터의 프로퍼티 "Laying"를 거짓으로 변환
            unMove = false;
            if (player_col.center.y <= 0.6f)
            {
                player_col.center = new Vector3(player_col.center.x, player_col.center.y * 5f, player_col.center.z);
                player_col.height = player_col.height * 5f;
            }
        }

        //플레이어 움직임 제어
        if (unMove == true) { }//플레이어 엎드리고 있으면 아무것도 안함
        else//일반적인 상태
        {
            float x = 0;
            float y = 0;
            if (FloatingJoystick.Instance.touch == true || VariableJoystick.Instance.touch == true)
            {
                x = FloatingJoystick.Instance.touch == true ? FloatingJoystick.Instance.Direction.x : VariableJoystick.Instance.Direction.x;
                y = FloatingJoystick.Instance.touch == true ? FloatingJoystick.Instance.Direction.y : VariableJoystick.Instance.Direction.y;
            }
            moveAnimating(x, y);
            Turning();
            Stop();
        }
    }


    void moveAnimating(float h, float v)//플레이어 움직임 애니메이션 메소드
    {
        float Run = Mathf.Sqrt(Mathf.Pow(h, 2) + Mathf.Pow(v, 2));

        if(InWater == true)
        {
            Run -= 0.6f;
            if(Run <= 0)
            {
                Run = 0;
            }

            InWater = false;
        }

        anim.SetFloat("Run", Run);
    }

    void Turning()//플레이어 회전 메소드
    {
        if (FloatingJoystick.Instance.touch == true || VariableJoystick.Instance.touch == true)
        {
            playerLook.x = FloatingJoystick.Instance.touch == true ? FloatingJoystick.Instance.Direction.x : VariableJoystick.Instance.Direction.x;
            playerLook.z = FloatingJoystick.Instance.touch == true ? FloatingJoystick.Instance.Direction.y : VariableJoystick.Instance.Direction.y;
            playerLook.y = 0f;
            Quaternion newRotatation = Quaternion.LookRotation(playerLook);
            player_rd.MoveRotation(newRotatation);
        }
        else
        {
            Quaternion newRotatation = Quaternion.LookRotation(playerLook);
            player_rd.MoveRotation(newRotatation);
        }
    }

    void Stop()//자연스러운 멈춤 메소드
    {
        if (FloatingJoystick.Instance.touch == true || VariableJoystick.Instance.touch == true)
        {
            Stoping = 0f;
            anim.SetBool("Walk", false);
            Walking = true;
        }
        else if (FloatingJoystick.Instance.touch == false && VariableJoystick.Instance.touch == false)
        {
            if (Walking == true)
            {
                Stoping = Stoping + Time.deltaTime * 2;
                anim.SetBool("Walk", true);
                anim.SetFloat("Stop", Stoping);
                if (Mathf.Approximately(Stoping, 1f))
                {
                    Walking = false;
                    anim.SetBool("Walk", false);
                    Stoping = 0f;
                }

            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Finish"))
        {
            mapManager.enabled = true;
            coinSpawn.enabled = true;
            isTriggerWall = true;
            coll.enabled = false;
            GameObject backcoll = coll.transform.parent.Find("Cube (1)").gameObject;
            backcoll.SetActive(true);
        }


    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("Water"))
        {
            InWater = true;
        }
    }
}
