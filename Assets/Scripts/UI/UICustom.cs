using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICustom : MonoBehaviour, IDragHandler
{
    RectTransform UIposition;
    public GameObject ChangeButton;
    public GameObject VariableJoystick;
    public Vector3 Defaultpos;
    public bool Lay = false;

    void Start()
    {
        UIposition = GetComponent<RectTransform>();
        if (DataController.Instance.gameData.VariableJoystick == true)
        {
            ChangeButton.SetActive(false);
            VariableJoystick.SetActive(true);
        }

        if (Lay == true)
        {
            UIposition.anchoredPosition = DataController.Instance.gameData.LayPosition;
        }
        else
        {
            UIposition.anchoredPosition = DataController.Instance.gameData.JoystickPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)//드래그 중일 때 
    {
        UIposition.position = Input.mousePosition;
    }

    public void ReturnDefault()
    {
        UIposition.anchoredPosition = Lay == true ? new Vector3(400, 300, 0) : new Vector3(400, 600, 0);
        DataController.Instance.gameData.VariableJoystick = false;
    }

    public void VariableJoystickAble(bool TT)
    {
        if (TT == true)
        {
            DataController.Instance.gameData.VariableJoystick = true;
        }
        else
        {
            DataController.Instance.gameData.VariableJoystick = false;
        }
        
    }

    public void SaveData()
    {
        if (Lay == true)
        {
            DataController.Instance.gameData.LayPosition = UIposition.anchoredPosition;
            DataController.Instance.SaveGameData();
        }
        else
        {
            DataController.Instance.gameData.JoystickPosition = UIposition.anchoredPosition;
            DataController.Instance.SaveGameData();
        }
    }
}
