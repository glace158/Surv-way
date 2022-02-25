using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 40;

        if (DataController.Instance.gameData.chswarp == false)
        {
            for (int i = 0; i < DataController.Instance.gameData.UnLockedCharacter.Count; i++)
            {
                int unLock = DataController.Instance.gameData.UnLockedCharacter[i];

                switch (unLock)
                {
                    case 0:
                        DataController.Instance.gameData.UnLocked[0].data[0] = true;
                        break;
                    case 1:
                        DataController.Instance.gameData.UnLocked[0].data[1] = true;
                        break;
                    case 2:
                        DataController.Instance.gameData.UnLocked[0].data[2] = true;
                        break;
                    case 3:
                        DataController.Instance.gameData.UnLocked[0].data[3] = true;
                        break;
                    case 4:
                        DataController.Instance.gameData.UnLocked[0].data[4] = true;
                        break;
                    case 5:
                        DataController.Instance.gameData.UnLocked[0].data[5] = true;
                        break;
                    case 6:
                        DataController.Instance.gameData.UnLocked[1].data[0] = true;
                        break;
                    case 7:
                        DataController.Instance.gameData.UnLocked[1].data[1] = true;
                        break;
                    case 8:
                        DataController.Instance.gameData.UnLocked[1].data[2] = true;
                        break;
                    case 9:
                        DataController.Instance.gameData.UnLocked[1].data[3] = true;
                        break;
                    case 10:
                        DataController.Instance.gameData.UnLocked[2].data[0] = true;
                        break;
                    case 11:
                        DataController.Instance.gameData.UnLocked[2].data[1] = true;
                        break;
                    case 12:
                        DataController.Instance.gameData.UnLocked[2].data[2] = true;
                        break;
                    case 13:
                        DataController.Instance.gameData.UnLocked[2].data[3] = true;
                        break;
                }
            }
            DataController.Instance.gameData.UnLocked[0].data[0] = true;
            DataController.Instance.gameData.CurrentCharacter.Add(0);
            DataController.Instance.gameData.CurrentCharacter.Add(0);
            DataController.Instance.gameData.CurrentScene = 2;
            DataController.Instance.gameData.chswarp = true;
        }

        if(DataController.Instance.gameData.TutoralEnd == true)
        {
            int sc = DataController.Instance.gameData.CurrentScene;
            SceneManager.LoadSceneAsync(sc);
        }
        else
        {
            DataController.Instance.gameData.CurrentScene = 2;
            DataController.Instance.gameData.CurrentCharacter[0] = 0;
            DataController.Instance.gameData.CurrentCharacter[1] = 0;
            DataController.Instance.SaveGameData();
            SceneManager.LoadSceneAsync(2);
        }
    }
}
