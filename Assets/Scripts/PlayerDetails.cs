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

    private void Update()
    {
        if (isServer)
        {
            playersConnected = NetworkServer.connections.Count;
        }

        if (isLocalPlayer)
        {
            if (playersConnected >= 1 && !playerState.hasDetails)
            {
                CmdStartGameOnServer();
            }
        }

        SyncState();
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
        int index = 0;

        if (GameObject.FindGameObjectWithTag("Player2") == null)
        {
            index = 2;
        } else
        {
            index = 1;
        }      
        if (index != 0)
        {
            newSprite = $"Sprites/jump/character{index}_idle";
            newTag = $"Player{index}";
            newInteractionZoneTag = $"Player{index}InteractionZone";
            newRuntimeAnimatorController = $"Animation/Player{index}";
            newPlayerMaterial = $"Materials/Player{index}Outline";
            newHasDetails = true;
        }

        return new PlayerState()
        {
            spritePath = newSprite,
            tag = newTag,
            interactionZoneTag = newInteractionZoneTag,
            controllerPath = newRuntimeAnimatorController,
            hasDetails = newHasDetails,
            materialPath = newPlayerMaterial
        };
    }

    public void SyncState()
    {
        this.gameObject.tag = playerState.tag;
        this.gameObject.GetComponentInChildren<Transform>().GetChild(0).tag = playerState.interactionZoneTag;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(playerState.spritePath);
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(playerState.controllerPath);
        this.gameObject.GetComponent<SpriteRenderer>().material = Resources.Load<Material>(playerState.materialPath);
    }

    void Awake()
    {
        playersConnected = 0;

        playerState = new PlayerState()
        {
            spritePath = "Sprites/jump/character1_idle",
            tag = "Player1",
            interactionZoneTag = "Player1InteractionZone",
            controllerPath = "Animation/Player1",
            materialPath = "Materials/Player1Outline",
            hasDetails = false
        };
    }
}
