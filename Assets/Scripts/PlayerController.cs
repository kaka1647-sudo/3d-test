using UnityEngine;
using UnityEngine.InputSystem;

// WASD로 플레이어(캡슐)를 이동시키는 스크립트
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // 이동 속도. Inspector에서 조절 가능
    public float moveSpeed = 5f;

    // 바닥 중앙 기준 이동 가능 범위 (Plane 반크기 5 - 여유)
    public float moveLimit = 4.5f;

    // 게임 오버 상태 (true면 이동 불가)
    public bool isStopped = false;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // 넘어지지 않도록 회전 고정
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // 게임 오버 시 수평 이동을 멈춘다
        if (isStopped)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        var kb = Keyboard.current;
        if (kb == null) return;

        // WASD 입력 읽기
        float h = 0f, v = 0f;
        if (kb.aKey.isPressed) h -= 1f;
        if (kb.dKey.isPressed) h += 1f;
        if (kb.sKey.isPressed) v -= 1f;
        if (kb.wKey.isPressed) v += 1f;

        // 대각선 이동 시 속도가 빨라지지 않도록 정규화
        Vector3 dir = new Vector3(h, 0f, v).normalized;
        rb.linearVelocity = new Vector3(dir.x * moveSpeed, rb.linearVelocity.y, dir.z * moveSpeed);

        // 바닥(10x10) 밖으로 떨어지지 않도록 이동 범위를 제한
        Vector3 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, -moveLimit, moveLimit);
        pos.z = Mathf.Clamp(pos.z, -moveLimit, moveLimit);
        rb.position = pos;
    }

    // 플레이어를 멈춘다 (게임 오버 시 호출)
    public void Stop()
    {
        isStopped = true;
    }
}
