using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAnime : MonoBehaviour
{
    Animator T_Rottle;
    AudioSource TAudio;

    private void Start()
    {
        T_Rottle = GetComponent<Animator>();
        TAudio = GetComponent<AudioSource>();
    }

    void TrainPlay()
    {
        T_Rottle.SetTrigger("Open");
        if (DataController.Instance.gameData.Audio == true)
        {
            TAudio.Play();
        }
    }
}
