using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameplayData _gameplayData;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private TextMeshProUGUI _player1Score;

    private void Awake() => _gameplayData.SetUIManager(this);
    public void UpdatePlayer1Score(int score) => _player1Score.SetText(score.ToString("D2"));

    public void UpdateCountdown(string text) => _countdownText.SetText(text);
    public void HideCountdown() => _countdownPanel.SetActive(false);
}
