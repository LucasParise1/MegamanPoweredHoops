using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameplayData _gameplayData;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Player _player;
    [SerializeField] private Animator _fadeScreen;
    [SerializeField] private SceneAsset _nextScene;

    private float _currentTime;
    private bool _isPlaying;

    private void Start()
    {
        _gameplayData.ResetData();
        _currentTime = _gameplayData.MatchTime;
        _uiManager.UpdateTimer(_currentTime);
        StartCoroutine(CountDown());
    }

    private void Update()
    {
        if (_isPlaying)
        {
            HandleTimer();
            _uiManager.UpdateTimer(_currentTime);
        }
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.5f);
        _uiManager.UpdateCountdown("3");

        yield return new WaitForSeconds(1);
        _uiManager.UpdateCountdown("2");

        yield return new WaitForSeconds(1);
        _uiManager.UpdateCountdown("1");

        yield return new WaitForSeconds(1);
        _uiManager.UpdateCountdown("Go!");

        yield return new WaitForSeconds(1);
        _uiManager.HideCountdown();
        _player.SetCanPlay(true);
        _isPlaying = true;
    }

    private void HandleTimer()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
        {
            _currentTime = 0;
            _isPlaying = false;
            EndGame();
        }
    }

    private void EndGame()
    {
        _player.SetCanPlay(false);
        _uiManager.ShowFinishScreen(true);
        Invoke(nameof(ShowVictorious), 3);
    }

    private void ShowVictorious()
    {
        if (_gameplayData.Player1Points > _gameplayData.Player2Points)
        {
            _player.SetVictory();
            //bot set defeat
        }

        if (_gameplayData.Player1Points < _gameplayData.Player2Points)
        {
            _player.SetDefeat();
            //bot set victory
        }

        if (_gameplayData.Player1Points == _gameplayData.Player2Points) _player.SetTie();
    }

    public void ContinueButton()
    {
        _fadeScreen.SetInteger(AnimationsParameters.FADE_VALUE, 1);
        Invoke(nameof(LoadNextScene), 1);
    }

    private void LoadNextScene() => SceneManager.LoadScene(_nextScene.name);
}
