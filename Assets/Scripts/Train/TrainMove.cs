using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrainMove : MonoBehaviour
{
    public Image TrainFill;//스폰되는 선로 UI의 이미지 가져오기 
    public GameObject StopColl;
    public float Speed = 6f;
    public AudioSource AudioSource;
    GameObject playerObject;
    Rigidbody train_rd;
    int UpDown = 1;
    bool OnOff = false;
    float a = 0.25f;

    void Awake()
    {
        train_rd = GetComponent<Rigidbody>();
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            a = 0.5f;
        }
    }

    private void Start()
    {
        if (DataController.Instance.gameData.Audio == false)
        {
            AudioSource.Stop();
        }
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (train_rd.transform.position.z - playerObject.transform.position.z > 0)
        {
            UpDown = -1;
        }
        else
        {
            UpDown = 1;
        }

        Speed *= UpDown;

        InvokeRepeating("blink", 0f, 0.2f);
        Invoke("CancelRepeating", 0.8f);
        InvokeRepeating("move", 0.8f, 0.02f);

    }

    void CancelRepeating()
    {
        CancelInvoke("blink");
    }

    void blink()
    {
        if(OnOff == false)
        {
            TrainFill.color = new Color(TrainFill.color.r, TrainFill.color.g, TrainFill.color.b, a);
            OnOff = true;
        }
        else
        {
            TrainFill.color = new Color(TrainFill.color.r, TrainFill.color.g, TrainFill.color.b, 0f);
            OnOff = false;
        }
    }


    void move()
    {
        train_rd.velocity = new Vector3(0, 0, Speed);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.CompareTag("TrainStop"))
        {
            train_rd.velocity = new Vector3(0, 0, 0);
            StopColl.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.transform.CompareTag("TrainStop"))
        {
            StopColl.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
