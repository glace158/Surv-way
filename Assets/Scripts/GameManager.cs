using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager Instance;

    MapManager mapManager;

    Train_Spawn train_Spawn;

    Movement movement;

    ScoreCheck scoreCheck;

    PlayerHealth playerHealth;

    GameObject player;

    [HideInInspector]
    public float score;

    [HideInInspector]
    public int FrontMap;

    [HideInInspector]
    public int BackMap;

    [HideInInspector]
    public int Up_Down;

    [HideInInspector]
    public bool isTriggerWall;

    [HideInInspector]
    public float line_5_prob, line_3_prob;

    [HideInInspector]
    public int backmap;

    [HideInInspector]
    public bool over;


    void Awake()
    {
        GameManager.Instance = this;
        Time.timeScale = 1;
        mapManager = GetComponent<MapManager>();
        train_Spawn = GetComponent<Train_Spawn>();
        scoreCheck = GetComponent<ScoreCheck>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = player.GetComponent<Movement>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        score = scoreCheck.score;
        FrontMap = mapManager.WhatMap;
        BackMap = mapManager.backmap;
        Up_Down = train_Spawn.Up_Down;
        isTriggerWall = movement.isTriggerWall;
        line_5_prob = MapManager.line_5_probs;
        line_3_prob = MapManager.line_3_probs;
        over = playerHealth.over;
    }
}