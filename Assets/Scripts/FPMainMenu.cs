using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FPMainMenu : MonoBehaviour
{
    public GameObject _loadingImage;
    public GameObject _quitConfirmationDialog;
    public GameObject _backgroundMusic;

    public GameObject _startButton;
    public GameObject _creditsButton;
    public GameObject _quitButton;

    private static GameObject _audioClip;

    void Start()
    {
        if (_audioClip == null)
            _audioClip = _backgroundMusic;
        else
            DestroyObject(_backgroundMusic);

        _loadingImage.SetActive(false);
        _quitConfirmationDialog.SetActive(false);
        _startButton.GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// Toggle menu buttons active / inactive.
    /// </summary>
    /// <param name="p_state">bool: true for active buttons, false for inactive buttons.</param>
    private void ToggleMainMenuButtons(bool p_state)
    {
        _startButton.SetActive(p_state);
        _quitButton.SetActive(p_state);
    }

    /// <summary>
    /// Activate the loading screen and load the game.
    /// </summary>
    public void LoadNewGame()
    {
        //DontDestroyOnLoad(_audioClip);
        _loadingImage.SetActive(true);
        SceneManager.LoadScene((int)FPConstants.Scenes.Game);
    }

    /// <summary>
    /// Load the credits scene while keeping the music playing.
    /// </summary>
    public void LoadCredits()
    {
        DontDestroyOnLoad(_audioClip);
        SceneManager.LoadScene((int)FPConstants.Scenes.Credits);
    }

    /// <summary>
    /// Activate the dialog that lets the user choose to quit or not,
    /// and deactivate the main menu buttons.
    /// </summary>
    public void LoadQuitConfirmationDialog()
    {
        ToggleMainMenuButtons(false);
        _quitConfirmationDialog.SetActive(true);
    }

    /// <summary>
    /// Deactivate the quit dialog.
    /// Reactivate menu buttons.
    /// </summary>
    public void CancelQuit()
    {
        _quitConfirmationDialog.SetActive(false);
        ToggleMainMenuButtons(true);
    }

    /// <summary>
    /// Quit the application without a warning. No turning back.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
