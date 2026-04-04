using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator _fadeScreen;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _charSelectMenu;
    [SerializeField] private GameplayData _gameplayData;
    [SerializeField] private string _nextScene;

    public void PlayButton()
    {
        _fadeScreen.SetInteger(AnimationsParameters.FADE_VALUE, 1);
        StartCoroutine(CharacterSelectionRoutine());
    }

    public void SelectedPlayer(CharacterData data)
    {
        _gameplayData.SetSelectedPlayer(data);
        _fadeScreen.SetInteger(AnimationsParameters.FADE_VALUE, 1);
        Invoke(nameof(LoadNextScene), 1);
    }

    public void QuitGame() => Application.Quit();

    private IEnumerator CharacterSelectionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _mainMenu.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        _charSelectMenu.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        _fadeScreen.SetInteger(AnimationsParameters.FADE_VALUE, 0);
    }

    private void LoadNextScene() => SceneManager.LoadScene(_nextScene);
}
