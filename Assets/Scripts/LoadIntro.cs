using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadIntro : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadMenu", 4f);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
