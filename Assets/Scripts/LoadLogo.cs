using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLogo : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadIntro", 4f);      
    }

    private void LoadIntro()
    {
        SceneManager.LoadScene("Intro");
    }
}
