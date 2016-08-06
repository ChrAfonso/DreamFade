using UnityEngine;
using UnityEngine.SceneManagement;

public class FPCredits : MonoBehaviour
{
    /// <summary>
    /// Load the main menu after game over or manual exiting the game scene.
    /// Reset the seed.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene((int)FPConstants.Scenes.MainMenu);
    }
}
