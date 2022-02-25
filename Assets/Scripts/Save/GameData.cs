using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable] //<== MonoBehaviour가 아닌 클래스에 대해 Inspector에 나타납니다.  
public class UnLockedplayArray
{
    public bool[] data = new bool[6];
}

[Serializable]
public class GameData
{
    public bool TutoralEnd = false;
    public int TutoralPage = 0;

    public int Coin = 100;
    public int HighScore = 0;
    public List<int> CurrentCharacter = new List<int>();

    public int CurrentScene = 2;
    public int NowGetCoin = 0;
    public bool RePlayGame = false;

    public bool VariableJoystick = false;
    public Vector3 JoystickPosition = new Vector3(400, 600, 0);
    public Vector3 LayPosition = new Vector3(400, 300, 0);

    public bool Audio = true;

    public List<int> UnLockedCharacter = new List<int>();

    public UnLockedplayArray[] UnLocked = new UnLockedplayArray[4];

    public bool chswarp = false;
}