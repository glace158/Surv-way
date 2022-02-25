using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject TutorialCanvas;
    public GameObject[] MainPage;
    public GameObject[] ChangePage;

    private void Awake()
    {
        if (DataController.Instance.gameData.TutoralEnd == false && SceneManager.GetActiveScene().buildIndex != 1)
        {
            TutorialCanvas.SetActive(true);
            MainPage[DataController.Instance.gameData.TutoralPage].SetActive(true);
        }
        else if (DataController.Instance.gameData.TutoralEnd == false)
        {
            ChangePage[0].SetActive(true);
        }
        else
        {
            TutorialCanvas.SetActive(false);
        }
    }

    public void ChangePageLoad()
    {
        Invoke("ChangeLoad", 5f);
    }

    void ChangeLoad()
    {
        ChangePage[0].SetActive(false);
        ChangePage[1].SetActive(true);
    }

    public void Stop()
    {
        Invoke("TimeStop", 0.7f);
    }

    void TimeStop()
    {
        Time.timeScale = 0;
    }

    public void PageUp()
    {
        DataController.Instance.gameData.TutoralPage++;
    }

    public void TutorialOver(bool a)
    {
        DataController.Instance.gameData.TutoralEnd = a;
        DataController.Instance.SaveGameData();
    }
}
