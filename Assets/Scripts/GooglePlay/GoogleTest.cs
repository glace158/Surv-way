using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

public class GoogleTest : MonoBehaviour
{
    private static GoogleTest instance;
    public static GoogleTest GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GoogleTest>();

                if (instance == null)
                {
                    instance = new GameObject("PlayGameCloud").AddComponent<GoogleTest>();
                }
            }

            return instance;
        }
    }

    public bool onSaving;
    public bool onLoading;
    public string saveData;

    TestDta testData;
    
    public void Init()
    {
        testData = FindObjectOfType<TestDta>();

        DontDestroyOnLoad(gameObject);

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
    }

    public void Login()
    {
        Social.localUser.Authenticate((bool _login) =>
        {
            if (_login == true)
            {
                Debug.Log("Login Comlete");
            }
            else
            {
                Debug.Log("Login Fail");
            }
        });
    }

    public bool CheckLogin()
    {
        return Social.localUser.authenticated;
    }

    public void SendScore()//스코어 업로드
    {
        int highScore = DataController.Instance.gameData.HighScore;
        Social.ReportScore((long)highScore, GPGSIds.leaderboard_top_score, (bool success) =>
        {
            if (success)
            {
                Social.ShowLeaderboardUI();
            }
            else
            {
            }
        });
    }

    #region Save

    public void SaveToCloud(string _data)
    {
        StartCoroutine(Save(_data));
    }
    IEnumerator Save(string _data)
    {
        while(CheckLogin() == false)
        {
            Login();
            yield return new WaitForSeconds(2f);
        }
        onSaving = true;

        string id = Social.localUser.id;
        string fileName = string.Format("{0}_Data", id);
        saveData = _data;

        OpenSavedGame(fileName, true);
    }

    void OpenSavedGame(string _fileName, bool _saved)
    {
        ISavedGameClient savedClient = PlayGamesPlatform.Instance.SavedGame;

        if(_saved == true)
        {
            //save
            savedClient.OpenWithAutomaticConflictResolution(_fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedToSave);

        }
        else
        {
            //loading
            savedClient.OpenWithAutomaticConflictResolution(_fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedToRead);
        }
    }

    void OnSavedGameOpenedToSave(SavedGameRequestStatus _status, ISavedGameMetadata _data)
    {
        if(_status == SavedGameRequestStatus.Success)
        {
            byte[] b = Encoding.UTF8.GetBytes(string.Format(saveData));
            SaveGame(_data, b, DateTime.Now.TimeOfDay);
        }
        else
        {
            Debug.Log("Faill");
        }
    }

    void SaveGame(ISavedGameMetadata _data, byte[] _byte, TimeSpan _playTime)
    {
        ISavedGameClient savedClient = PlayGamesPlatform.Instance.SavedGame;
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

        builder = builder.WithUpdatedPlayedTime(_playTime).WithUpdatedDescription("Saved at " + DateTime.Now);

        SavedGameMetadataUpdate updatedData = builder.Build();
        savedClient.CommitUpdate(_data, updatedData, _byte, OnSavedGameWritten);
    }

    void OnSavedGameWritten(SavedGameRequestStatus _status, ISavedGameMetadata _data)
    {
        onSaving = false;
        if(_status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Save Complete");
        }
        else
        {
            Debug.Log("Save Fail");
        }
    }

    #endregion

    #region Load

    public void LoadFromCloud()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        onLoading = true;
        while (CheckLogin() == false)
        {
            Login();
            yield return new WaitForSeconds(2f);
        }
        string id = Social.localUser.id;
        string fileName = string.Format("{0}_DATA", id);

        OpenSavedGame(fileName, false);
    }

    void OnSavedGameOpenedToRead(SavedGameRequestStatus _status, ISavedGameMetadata _data)
    {
        if(_status == SavedGameRequestStatus.Success)
        {
            LoadGameData(_data);
        }
        else
        {
            Debug.Log("Fail");
        }
    }

    void LoadGameData(ISavedGameMetadata _data)
    {
        ISavedGameClient savedClient = PlayGamesPlatform.Instance.SavedGame;
        savedClient.ReadBinaryData(_data, OnSavedGameDataRead);
    }

    void OnSavedGameDataRead(SavedGameRequestStatus _status, byte[] _byte)
    {
        if(_status == SavedGameRequestStatus.Success)
        {
            string data = Encoding.Default.GetString(_byte);

            testData.SetData(data);
        }
        else
        {
            Debug.Log("Load Faill");
        }
    }


    #endregion
}
