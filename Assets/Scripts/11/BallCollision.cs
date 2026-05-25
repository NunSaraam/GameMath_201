using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance == null || !GameManager.Instance.isBallsMoving) return;

        bool isMyTurnBall = (GameManager.Instance.is1PTurn && gameObject.CompareTag("Player1")) ||
                            (!GameManager.Instance.is1PTurn && gameObject.CompareTag("Player2"));

        if (isMyTurnBall)
        {
            if (collision.gameObject.CompareTag("Target1"))
                GameManager.Instance.hitTarget1 = true;

            if (collision.gameObject.CompareTag("Target2"))
                GameManager.Instance.hitTarget2 = true;

            if (GameManager.Instance.is1PTurn && collision.gameObject.CompareTag("Player2"))
                GameManager.Instance.hitOpponent = true;

            if (!GameManager.Instance.is1PTurn && collision.gameObject.CompareTag("Player1"))
                GameManager.Instance.hitOpponent = true;
        }
    }
}
