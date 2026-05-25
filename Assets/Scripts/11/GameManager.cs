using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool is1PTurn = true;   
    public bool isBallsMoving = false;
    public int score1P = 0;
    public int score2P = 0;
    public int winScore = 5;

    public Rigidbody[] allBalls;   
    public Transform ball1P;       
    public Transform ball2P;       
    public CameraOrbit cameraOrbit;

    public bool hitTarget1 = false;
    public bool hitTarget2 = false;
    public bool hitOpponent = false;

    private bool isCheckingStop = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        UpdateCameraTarget();
    }

    private void Update()
    {
        if (isCheckingStop)
        {
            CheckBallsStopped();
        }
    }

    public void OnTurnStarted()
    {
        isBallsMoving = true;

        hitTarget1 = false;
        hitTarget2 = false;
        hitOpponent = false;

        StartCoroutine(WaitAndCheckStop());
    }

    private void CheckBallsStopped()
    {
        bool moving = false;
        foreach (Rigidbody rb in allBalls)
        {
            if (rb.linearVelocity.magnitude > 0.05f)
            {
                moving = true;
                break;
            }
        }

        if (!moving)
        {
            isBallsMoving = false;
            isCheckingStop = false;
            CalculateScore();
            ChangeTurn();
        }
    }

    private IEnumerator WaitAndCheckStop()
    {
        yield return new WaitForSeconds(0.5f);

        isCheckingStop = true;
    }

    private void CalculateScore()
    {
        int currentScore = is1PTurn ? score1P : score2P;

        if (hitTarget1 && hitTarget2 && !hitOpponent)
        {
            currentScore += 1;
            Debug.Log($"{(is1PTurn ? "1P" : "2P")} 득점!");
        }

        if (hitOpponent)
        {
            currentScore = Mathf.Max(0, currentScore - 1);
            Debug.Log($"{(is1PTurn ? "1P" : "2P")} 감점! 상대 공 타격");
        }

        if (is1PTurn) score1P = currentScore;
        else score2P = currentScore;

        Debug.Log($"현재 점수 - 1P: {score1P} | 2P: {score2P}");

        if (score1P >= winScore || score2P >= winScore)
        {
            Debug.Log($"게임 종료! {(score1P >= winScore ? "1P" : "2P")} 승리!");
        }
    }

    private void ChangeTurn()
    {
        is1PTurn = !is1PTurn;
        UpdateCameraTarget();
        Debug.Log($"턴 변경 -> {(is1PTurn ? "1P" : "2P")}의 턴");
    }

    private void UpdateCameraTarget()
    {
        if (cameraOrbit != null)
        {
            cameraOrbit.target = is1PTurn ? ball1P : ball2P;
        }
    }
}
