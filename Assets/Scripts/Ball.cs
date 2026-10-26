using UnityEngine;

// 떨어지는 공: 플레이어와 닿으면 게임 오버 처리
public class Ball : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // 닿은 대상이 플레이어인지 확인
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}
