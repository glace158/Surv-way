using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashTrain : MonoBehaviour
{
    public GameObject Water;
    Transform WTtrans;

    float zpos = -5;

    private void Awake()
    {
        WTtrans = GetComponent<Transform>();
    }

    private void Start()
    {
        zpos = GameManager.Instance.Up_Down == 1 ? 5 : -5;
        InvokeRepeating("Discheck", 0f, 0.5f);
    }

    void Discheck()
    {
        Instantiate(Water, new Vector3(WTtrans.transform.position.x + 4f, 0, WTtrans.transform.position.z + zpos), Quaternion.Euler(180, 0, 0));
        Instantiate(Water, new Vector3(WTtrans.transform.position.x - 4f, 0, WTtrans.transform.position.z + zpos), Quaternion.Euler(180, 0, 0));
    }
}
