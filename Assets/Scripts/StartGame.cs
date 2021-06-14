using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine;

public class StartGame : NetworkBehaviour
{
    public static void LoadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void LoadMatch()
    {
        Time.timeScale = 1;
        NetworkClient.Shutdown();
        NetworkServer.Shutdown();
        SceneManager.LoadScene("Game");
    }
}
