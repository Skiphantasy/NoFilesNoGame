using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

public class PlayerScore : NetworkBehaviour
{
    public List<AudioClip> audioClips;
    AudioSource playerAudioSource;
    Text winText;
    Canvas winScreen;
    GameObject selectedButton;
    List<Text> playerScoreTexts = new List<Text>(new Text[2]);
    private float timeSinceLastScoreUpdate = 0f;
    private float timeBetweenScoreUpdates = 1f;
    
    [SyncVar]
    public string playerName;
    [SyncVar]
    public int playersConnected;
    [SyncVar]
    public int score;
    [SyncVar]
    public int index;
    [SyncVar]
    bool isFileProcessed;

    bool isGoodFile = false;

    private void Awake()
    {
        score = 0;
        index = 0;
        playerName = "";
    }

    void Start()
    {    
        GameObject winScreenGameObject = GameObject.FindGameObjectWithTag("WinScreen");
        playerAudioSource = this.gameObject.GetComponent<AudioSource>();
        GameObject p1Scoretext = GameObject.FindGameObjectWithTag("Player1Score");
        GameObject p2Scoretext = GameObject.FindGameObjectWithTag("Player2Score");
        playerScoreTexts[0] = p1Scoretext.GetComponent<Text>();
        playerScoreTexts[1] = p2Scoretext.GetComponent<Text>();

        if (winScreenGameObject != null)
        {
            winScreen = winScreenGameObject.GetComponent<Canvas>();
            winText = winScreen.gameObject.GetComponentInChildren<Text>();
            selectedButton = winScreen.gameObject.GetComponentInChildren<Button>().gameObject;
        }        

        winScreen.enabled = false;
    }

    private void FixedUpdate()
    {
        if (GameSparks.Core.GS.Authenticated)
        { 
            if (isServer)
            {
                playersConnected = NetworkServer.connections.Count;
            }

            if (isLocalPlayer)
            {
                CmdSetPlayerIndex();

                timeSinceLastScoreUpdate += Time.deltaTime;

                if (timeSinceLastScoreUpdate >= timeBetweenScoreUpdates)
                {
                    //CmdSetPlayerName();

                    if (!isFileProcessed && index != 0)
                    {
                        CmdCalculateScore();                   
                    }

                    timeSinceLastScoreUpdate -= timeBetweenScoreUpdates;
                }
            }

            SyncState();
        }
    }

    public void SyncState()
    {
        if (index != 0)
        {
            playerScoreTexts[index - 1].text = $"Score Player {index}: " + score;

            if (score >= 100)
            {
                winText.text = $"Player {index} Wins";
                winScreen.enabled = true;
                EventSystem.current.SetSelectedGameObject(selectedButton);
                Time.timeScale = 0;
            }
        }
    }

    [Command]
    void CmdCalculateScore()
    {
        if (GameSparks.Core.GS.Authenticated)
        {
            if (score >= 0 && score < 100)
            {
                if (isGoodFile)
                {
                    score += 10;
                } else
                {
                    score -= 5;
                }

                if (score < 0)
                {
                    score = 0;
                }

                RpcUpdateFileBoolInClient(true);
            }
        }
    }

    [ClientRpc]
    void RpcUpdateFileBoolInClient(bool newValue)
    {
        isFileProcessed = newValue;
    }

    [Command]
    private void CmdSetPlayerIndex()
    {
        try
        {
            if (index == 0)
                index = int.Parse(this.gameObject.tag.Substring(this.gameObject.tag.Length - 1));        
        } catch
        {
            Debug.Log("Error setting index");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {
            if (collision.tag.Contains("ObjectFalling"))
            {
                if (index == int.Parse(collision.tag.Substring(collision.tag.Length - 1)))
                {
                    isGoodFile = true;                   
                }
                else
                {
                    isGoodFile = false;          
                }

                int clip = isGoodFile ? 2 : 1;
                playerAudioSource.clip = audioClips[clip];
                playerAudioSource.Play();
                isFileProcessed = false;
                Destroy(collision.gameObject);

                RpcUpdateData(isGoodFile, isFileProcessed, collision.gameObject);
            }       
        }
    }

    [ClientRpc]
    public void RpcUpdateData(bool goodFile, bool processed, GameObject objectToDestroy)
    {
        isFileProcessed = processed;
        isGoodFile = goodFile;
        int clip = isGoodFile ? 2 : 1;
        playerAudioSource.clip = audioClips[clip];
        playerAudioSource.Play();
        Destroy(objectToDestroy);
    }
}
