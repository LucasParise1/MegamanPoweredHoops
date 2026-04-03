using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameplayData _gameplayData;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Player _player;

    private void Start()
    {
        _gameplayData.ResetData();
        StartCoroutine(CountDown());
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
    }
}
