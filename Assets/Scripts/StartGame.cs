using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine;

public class StartGame : NetworkBehaviour
{
    public void LoadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
}
