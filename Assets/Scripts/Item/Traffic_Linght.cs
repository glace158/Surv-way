using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic_Linght : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime * 1, 0, Space.World);
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position);//월드 좌표를 스크린 좌표로 변형한다.
        if (view.y < -800)
        {
            Destroy(gameObject);//스크린 좌표가 -50 이하일시 삭제  
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<CoinSpawn>().SendMessage("TrainStop");
            Destroy(gameObject);
        }
    }
}
