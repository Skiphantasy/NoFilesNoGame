using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private void Awake()
    {
        ScorePlayer.playerCounters = new List<int>(2) { 0, 0 };
        ScorePlayer.playerAudioSources = new List<AudioSource>();
        ScorePlayer.playerAudioSources.Add(GameObject.FindGameObjectWithTag("Player1").GetComponent<AudioSource>());
        ScorePlayer.playerAudioSources.Add(GameObject.FindGameObjectWithTag("Player2").GetComponent<AudioSource>());
        ScorePlayer.playerScores = new List<Text>();
        ScorePlayer.playerScores.Add(GameObject.FindGameObjectWithTag("Player1Score").GetComponent<Text>());
        ScorePlayer.playerScores.Add(GameObject.FindGameObjectWithTag("Player2Score").GetComponent<Text>());
        ScorePlayer.playerScores[0].text = ("Score Player 1: " + ScorePlayer.playerCounters[0]);
        ScorePlayer.playerScores[1].text = ("Score Player 2: " + ScorePlayer.playerCounters[1]);
        Time.timeScale = 1;
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
}
