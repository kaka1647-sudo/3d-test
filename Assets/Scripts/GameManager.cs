using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 공 생성, 생존 시간 표시, 게임 오버를 관리하는 스크립트
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 공이 생성되는 간격(초)
    public float spawnInterval = 3f;
    // 플레이어 위 몇 미터 높이에서 생성할지
    public float spawnHeight = 8f;
    // 공이 이 시간(초) 후 자동 삭제 (정리용)
    public float ballLifetime = 15f;

    // 참조 (Inspector에서 연결)
    public PlayerController player;
    public Text timeText;
    // 게임 오버 시 화면 중앙에 표시할 문구
    public Text gameOverText;
    // 게임 오버 시 표시되는 다시 시작 버튼
    public Button restartButton;

    float surviveTime = 0f;
    float spawnTimer = 0f;
    bool isGameOver = false;

    void Awake()
    {
        Instance = this;

        // 다시 시작 버튼: 평소엔 숨기고, 클릭하면 현재 씬을 다시 불러온다
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
            restartButton.onClick.AddListener(Restart);
        }
    }

    // 현재 씬을 처음부터 다시 로드
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if (isGameOver) return; // 게임 오버면 시간·생성 모두 정지

        // 생존 시간 증가 및 표시 (초 단위)
        surviveTime += Time.deltaTime;
        if (timeText != null)
            timeText.text = "Time: " + surviveTime.ToString("F1") + " s"; // 기본 폰트가 한글을 지원하지 않아 영문 표기

        // 3초마다 공 생성
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnBall();
        }
    }

    // 플레이어 위 공중에 공 생성
    void SpawnBall()
    {
        Vector3 pos = player.transform.position + Vector3.up * spawnHeight;
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "FallingBall";
        ball.transform.position = pos;
        ball.GetComponent<Renderer>().material.color = Color.red;
        ball.AddComponent<Rigidbody>(); // 중력으로 낙하
        ball.AddComponent<Ball>();
        Destroy(ball, ballLifetime);
    }

    // 게임 오버 처리
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("Game Over");
        player.Stop();

        // 화면에 게임 오버 문구 표시
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        // 다시 시작 버튼 표시
        if (restartButton != null)
            restartButton.gameObject.SetActive(true);
    }
}
