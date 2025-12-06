using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_Game : MonoBehaviour
{
    public void GameOver()
    {
        SceneManager.LoadScene("Menu_GameOver");
    }
}