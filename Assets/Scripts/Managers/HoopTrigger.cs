using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    [SerializeField] private GameplayData _gameplayData;
    [SerializeField] private bool _scorePlayer1;

    private bool _scored;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.BALL) && !_scored)
        {
            _scored = true;

            if (_scorePlayer1) _gameplayData.AddPlayer1Points(1);
            else _gameplayData.AddPlayer2Points(1);

            Invoke(nameof(CanScoreAgain), 1);
        }
    }

    private void CanScoreAgain() => _scored = false;
}
