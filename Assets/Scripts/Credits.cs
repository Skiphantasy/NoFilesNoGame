﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
