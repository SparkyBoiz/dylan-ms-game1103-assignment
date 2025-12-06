using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the overall game state, like loading scenes.
/// </summary>
public class Manager_Game : MonoBehaviour
{
    /// <summary>
    /// Loads the "Game Over" scene.
    /// </summary>
    public void GameOver()
    {
        // Make sure "GameOver" is the exact name of your scene file.
        // Also, ensure this scene is added to your Build Settings (File > Build Settings...).
        SceneManager.LoadScene("Menu_GameOver");
    }
}