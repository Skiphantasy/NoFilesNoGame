using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetAxisRaw("Cancel") != 0)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
