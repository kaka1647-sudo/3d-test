using UnityEngine;

// 오브젝트를 Y축 기준으로 계속 회전시키는 스크립트
public class Rotator : MonoBehaviour
{
    // 초당 회전 속도(도). Inspector에서 조절 가능
    public float rotateSpeed = 90f;

    void Update()
    {
        // 프레임 시간에 비례해 회전 (초당 rotateSpeed 도)
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
