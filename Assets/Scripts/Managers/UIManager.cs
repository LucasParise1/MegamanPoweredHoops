using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameplayData _gameplayData;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private TextMeshProUGUI _player1Score;
    [SerializeField] private Slider _player1Gauge;
    [SerializeField] private Image _playerIcon;
    [SerializeField] private GameObject _finishScreen;
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private TextMeshProUGUI _victoryText;

    private void Awake() => _gameplayData.SetUIManager(this);
    public void UpdatePlayer1Score(int score) => _player1Score.SetText(score.ToString("D2"));
    public void UpdateCharge1UI(float value) => _player1Gauge.value = value;
    public void UpdateCountdown(string text) => _countdownText.SetText(text);
    public void HideCountdown() => _countdownPanel.SetActive(false);
    public void SetGauge1MaxValue(float maxValue) => _player1Gauge.maxValue = maxValue;
    public void UpdatePlayer1Icon(Sprite sprite) => _playerIcon.sprite = sprite;
    public void ShowFinishScreen(bool active) => _finishScreen.SetActive(active);

    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        _timerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    public void ShowVictoryScreen(string text)
    {
        ShowFinishScreen(false);
        _timerText.transform.parent.gameObject.SetActive(false);
        _victoryScreen.SetActive(true);
        _victoryText.SetText(text);
    }
}
