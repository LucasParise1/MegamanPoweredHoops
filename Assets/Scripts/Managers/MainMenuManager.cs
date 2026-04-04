using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator _fadeScreen;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _charSelectMenu;

    public void PlayButton()
    {
        _fadeScreen.SetInteger(AnimationsParameters.FADE_VALUE, 1);
        StartCoroutine(CharacterSelectionRoutine());
    }

    private IEnumerator CharacterSelectionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _mainMenu.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        _charSelectMenu.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        _fadeScreen.SetInteger(AnimationsParameters.FADE_VALUE, 0);
    }

    public void QuitGame() => Application.Quit();
}
