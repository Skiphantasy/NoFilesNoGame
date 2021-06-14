using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerDetails : NetworkBehaviour
{
    public List<Sprite> playerSprites;
    public List<RuntimeAnimatorController> playerControllers;

    [SyncVar]
    public PlayerState playerState;

    [SyncVar]
    public int playersConnected;

    [SyncVar]
    bool charactersUpdated = false;

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
                if (playersConnected == 2 && playerState.index == 0)
                {
                    CmdStartGameOnServer();
                }
            }

            SyncState();
        }
    }

    [Command]
    void CmdStartGameOnServer()
    {
        playerState = ProcessPlayerCreation(playerState);
    }

    PlayerState ProcessPlayerCreation(PlayerState state)
    {
        string newSprite = state.spritePath;
        string newTag = state.tag;
        string newInteractionZoneTag = state.interactionZoneTag;
        string newRuntimeAnimatorController = state.controllerPath;
        string newPlayerMaterial = state.materialPath;
        bool newHasDetails = state.hasDetails;
        int newIndex = state.index;

        if (GameObject.FindGameObjectWithTag("Player2") == null)
        {
            newIndex = 2;
        } else
        {
            newIndex = 1;
        }      
        if (newIndex != 0)
        {
            newSprite = $"Sprites/jump/character{newIndex}_idle";
            newTag = $"Player{newIndex}";
            newInteractionZoneTag = $"Player{newIndex}InteractionZone";
            newRuntimeAnimatorController = $"Animation/Player{newIndex}";
            newPlayerMaterial = $"Materials/Player{newIndex}Outline";
            newHasDetails = true;
        }

        return new PlayerState()
        {
            spritePath = newSprite,
            tag = newTag,
            interactionZoneTag = newInteractionZoneTag,
            controllerPath = newRuntimeAnimatorController,
            materialPath = newPlayerMaterial,
            hasDetails = newHasDetails,
            index = newIndex
        };
    }

    public void SyncState()
    {
        if (playerState.index != 0)
        {
            this.gameObject.tag = playerState.tag;
            this.gameObject.GetComponentInChildren<Transform>().GetChild(0).tag = playerState.interactionZoneTag;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(playerState.spritePath);
            this.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(playerState.controllerPath);
            this.gameObject.GetComponent<SpriteRenderer>().material = Resources.Load<Material>(playerState.materialPath);
        }        
    }

    [Command]
    void CmdSetUpdatedStatus()
    {
        if (charactersUpdated == false)
            charactersUpdated = true;
    }

    void Awake()
    {
        if (GameSparks.Core.GS.Authenticated)
        {
            playersConnected = 0;

            playerState = new PlayerState()
            {
                spritePath = "Sprites/jump/character1_idle",
                tag = "Player1",
                interactionZoneTag = "Player1InteractionZone",
                controllerPath = "Animation/Player1",
                materialPath = "Materials/Player1Outline",
                index = 0
            };
        }
    }

    private void OnApplicationQuit()
    {
        GameSparks.Core.GS.Instance.Reset();
    }
}
