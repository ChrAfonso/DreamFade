using UnityEngine;

public class FPConstants : MonoBehaviour
{
    /// <summary>
    /// Used to load scenes.
    /// The Scene Order must match the one in File > Build Settings > Scenes in Build.
    /// </summary>
    public enum Scenes
    {
        MainMenu = 0,
        Game = 1,
        Credits = 2
    }
}
