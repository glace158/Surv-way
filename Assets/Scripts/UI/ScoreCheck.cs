using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreCheck : MonoBehaviour
{
    float cuposition = 0;//현재 플레이어 위치
    float lastPosition = 0;// 제일 멀리간 플레이어 위치
    int TopScore = 0;
    [HideInInspector] public float score = 0;

    public Text ScoreText;
    public Text HighText;
    GameObject playerObject;

    public Text coinText;

    private void Awake()
    {
        HighText.text = string.Format("Top\n" + DataController.Instance.gameData.HighScore);
        coinText.text = string.Format("" + DataController.Instance.gameData.Coin); 
    }

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        cuposition = playerObject.transform.position.z;//현재 플레이어 위치 저장
        TopScore = DataController.Instance.gameData.HighScore;
    }

    void FixedUpdate()
    {
        DisScore();
        TimeScore();

        if (TopScore < score)
        {
            DataController.Instance.gameData.HighScore = (int)score;
            HighText.text = string.Format("Top\n" + DataController.Instance.gameData.HighScore);
        }
        ScoreText.text = "Score\n" + (int)score;
    }

    void TimeScore()//시간 점수 계산
    {
        score += Time.deltaTime * 1;
    }

    void DisScore()// 거리 점수 계산
    {
        cuposition = playerObject.transform.position.z;//현재 플레이어 위치 저장

        if (lastPosition <= cuposition)//화면이 터치가 되고 조이스틱이 움직이고 제일 멀리간 플레이어 위치보다 클때 실행
        {
            score += (cuposition - lastPosition) * 1.2f;//플레이어가 움직인 만큼 점수 올림
            lastPosition = cuposition;
        }
    }
}
