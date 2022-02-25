using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Lay_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool Lay;

    public GameObject LayGauge;

    bool cool = false;
    Slider LaySlider;
    Image LayColor;
    Image LayImage;

    void Awake()
    {
        LayImage = GetComponent<Image>();
        LaySlider = LayGauge.GetComponent<Slider>();
        LayColor = LayGauge.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        
        if (Lay && cool == false)
        {
            LaySlider.value -= Time.deltaTime * 0.2f;
            if (LaySlider.value <= 0f)
            {
                LaySlider.value = 0f;
                cool = true;
                Lay = false;
            }
        }
        else
        {
            if(LaySlider.value < 1f)
            {
                LaySlider.value += Time.deltaTime * 0.2f;
            }
            else if(Mathf.Approximately(LaySlider.value, 1f))
            {
                LaySlider.value = 1f;
                cool = false;
            }
        }

        if(cool == true)
        {
            LayImage.color = new Color(1f, 0f, 0f);
            LayColor.color = new Color(1f, 0f, 0f);
        }
        else if(cool == false)
        {
            LayImage.color = new Color(1f, 1f, 1f);
            LayColor.color = new Color(0f, 0f, 1f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(cool == false)
        {
            Lay = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Lay = false;
    }
}
