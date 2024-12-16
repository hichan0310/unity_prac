using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 주인공의 트랜스폼
    public float followSpeed = 0.01f; // 카메라 따라가기 속도

    private Vector3 originalPos; // 카메라 원래 위치

    void Start()
    {
        originalPos = transform.localPosition; // 원래 위치 저장
    }
    
    void Update()
    {
        // 주인공을 따라 부드럽게 이동
        //FollowPlayer();
        transform.position = new Vector3(player.position.x, player.position.y, originalPos.z);
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, originalPos.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, followSpeed);
    }

    public void Vibration(int strength)
    {
        Debug.Log("vib");
        StartCoroutine(Shake(strength));
    }

    // 진동 효과를 처리하는 코루틴
    IEnumerator Shake(int strength)
    {
        Vector3 originalPos = transform.localPosition; // 원래 위치 저장
        float duration = 0.5f; // 진동 지속 시간
        float magnitude = strength * 0.1f; // 진동 세기 조절
        float elapsedTime = 0f;

        // 진동의 패턴을 다양하게 조정
        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-1f, 1f) * magnitude; // X축 랜덤 변동
            float yOffset = Random.Range(-1f, 1f) * magnitude; // Y축 랜덤 변동

            // 카메라의 위치를 변동된 위치로 설정
            transform.localPosition = originalPos + new Vector3(xOffset, yOffset, 0);

            // 경과 시간을 업데이트
            elapsedTime += Time.deltaTime;

            // 한 프레임 대기
            yield return null;
        }

        // 진동이 끝나면 카메라의 위치를 원래대로 되돌림
        transform.localPosition = originalPos; // 원래 위치로 복원
    }
}