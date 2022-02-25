using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollectionUI : MonoBehaviour
{
    RaycastHit hit;
    float MaxDis = 15f;
    public Text NameText;


    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDis))
        {
            NameText.text = hit.transform.name;
        }
    }


}

