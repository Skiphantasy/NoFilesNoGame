using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScorePlayer : MonoBehaviour
{
    public static List<int> playerCounters;
    public static List<Text> playerScores;
    public static List<AudioSource> playerAudioSources;
    public Text winText;
    public Canvas winScreen;
    public List<AudioClip> audioClips;
    public GameObject restartButton;

    void Start()
    {
        //playerCounters = new List<int>(2);
        //playerCounters[0] = 0;
        //playerCounters[0] = 1;
        //playerAudioSources = new List<AudioSource>();
        //playerAudioSources.Add(GameObject.FindGameObjectWithTag("Player1").GetComponent<AudioSource>());
        //playerAudioSources.Add(GameObject.FindGameObjectWithTag("Player2").GetComponent<AudioSource>());
        //playerScores = new List<Text>();
        //playerScores.Add(GameObject.FindGameObjectWithTag("Player1Score").GetComponent<Text>());
        //playerScores.Add(GameObject.FindGameObjectWithTag("Player2Score").GetComponent<Text>());
        //playerScores[0].text = ("Score Player 1: " + playerCounters[0]);
        //playerScores[1].text = ("Score Player 2: " + playerCounters[1]);
        winScreen.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        int playerIndex = int.Parse(this.gameObject.tag.Substring(this.gameObject.tag.Length - 1));

        if (collision.tag.Contains("ObjectFalling"))
        {
            if (playerIndex.ToString() == collision.tag.Substring(collision.tag.Length - 1))
            {
                playerAudioSources[playerIndex - 1].clip = audioClips[2];
                playerAudioSources[playerIndex - 1].Play();
                playerCounters[playerIndex - 1] += 10;
                Destroy(collision.gameObject);
                playerScores[playerIndex - 1].text = $"Score Player {playerIndex}: " + playerCounters[playerIndex - 1];
            }
            else
            {
                playerAudioSources[playerIndex - 1].clip = audioClips[1];
                playerAudioSources[playerIndex - 1].Play();
                playerCounters[playerIndex - 1] -= 5;

                if (playerCounters[playerIndex - 1] < 0)
                {
                    playerCounters[playerIndex - 1] = 0;
                }

                Destroy(collision.gameObject);
                playerScores[playerIndex - 1].text = "Score Player 1: " + playerCounters[playerIndex - 1];
            }
        }

        if (playerCounters[playerIndex - 1] >= 100)
        {
            winText.text = $"Player {playerIndex} Wins";
            winScreen.enabled = true;
            EventSystem.current.SetSelectedGameObject(restartButton);
            Time.timeScale = 0;
        }
    }
}
