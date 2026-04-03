using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameplayData _gameplayData;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private TextMeshProUGUI _player1Score;
    [SerializeField] private Slider _player1Gauge;

    private void Awake() => _gameplayData.SetUIManager(this);
    public void UpdatePlayer1Score(int score) => _player1Score.SetText(score.ToString("D2"));
    public void UpdateCharge1UI(float value) => _player1Gauge.value = value;
    public void UpdateCountdown(string text) => _countdownText.SetText(text);
    public void HideCountdown() => _countdownPanel.SetActive(false);
    public void SetGauge1MaxValue(float maxValue) => _player1Gauge.maxValue = maxValue;
}
