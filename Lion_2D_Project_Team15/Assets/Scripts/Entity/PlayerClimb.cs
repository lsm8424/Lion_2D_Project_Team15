using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float climbSpeed; // 사다리 오르내리는 속도

    private void Update()
    {
        // 플레이어가 현재 사다리 상태일 경우에만 실행
        if (Player.Instance.interaction.IsOnLadder())
        {
            // W/S 키 입력 받기
            float v = Input.GetAxisRaw("Vertical"); // W/S 입력
            // 입력 방향으로 위/아래 이동 벡터 계산
            Vector3 climbDir = new Vector3(0, v, 0);
            // 해당 방향으로 이동 (중력 없이 직선 이동)
            transform.Translate(climbDir * climbSpeed * Time.deltaTime);
            // 현재 올라탄 사다리 정보 가져오기
            Ladder ladder = Player.Instance.interaction.GetCurrentLadder();
            // 사다리 객체가 있고, 플레이어가 지정된 높이 이상 올라가면 자동 탈출
            if (ladder != null & transform.position.y >= ladder.topExitY)
            {
                Player.Instance.interaction.ForceExitLadder(); // 사다리 탈출 처리
            }
        }
    }
}
