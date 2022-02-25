using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Transform WaterTrans;
    float T = 0;
    bool des = false;
    float Xscale;
    float Yscale;
    float Zscale = 2.5f;

    private void Awake()
    {
        WaterTrans = GetComponent<Transform>();
    }

    private void Start()
    {
        WaterTrans.transform.localScale = new Vector3(0 ,0 ,0);
        Xscale = 435 * Zscale / 100;
        Yscale = 509 * Zscale / 100;
    }

    void FixedUpdate()
    {
        if(des == false)
        {
            if (WaterTrans.transform.localScale.z <= 100f)
            {
                WaterTrans.transform.localScale = new Vector3(WaterTrans.transform.localScale.x + Xscale, WaterTrans.transform.localScale.y + Yscale, WaterTrans.transform.localScale.z + Zscale);
            }
            else
            {
                T += Time.deltaTime * 1;
                if (T > 4)
                {
                    des = true;
                    T = 0;
                    Zscale = 0.5f;
                    Xscale = 435 * Zscale / 100;
                    Yscale = 509 * Zscale / 100;

                }
            }
        }
        else if (des == true)
        {
            if (WaterTrans.transform.localScale.z >= 10f)
            {
                WaterTrans.transform.localScale = new Vector3(WaterTrans.transform.localScale.x - Xscale, WaterTrans.transform.localScale.y - Yscale, WaterTrans.transform.localScale.z - Zscale);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
