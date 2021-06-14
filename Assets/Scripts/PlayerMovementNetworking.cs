using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovementNetworking : NetworkBehaviour
{
    public float speed = 5.0f;
    public const float PlayerLerpEasing = 0.05f;
    public const float PlayerLerpSpacing = 1.0f;
    public const float PlayerFixedUpdateInterval = 0.01f;

    Vector2 screenBounds;

    private Animator animator;
    public List<AudioClip> audioClips;

    [SyncVar(hook = "OnServerStateChanged")]
    private PlayerState state;
    private PlayerState predictedState;
    private List<PlayerInput> pendingMoves;

    void Awake()
    {
        InitState();
    }

    private void InitState()
    {
        state = new PlayerState
        {
            timestamp = 0,
            position = transform.position,
            flipped = GetComponent<SpriteRenderer>().flipX,
            isWalking = false
        };
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            pendingMoves = new List<PlayerInput>();
        }

        screenBounds = new Vector2(10, 0);
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (GameSparks.Core.GS.Authenticated)
        {
            if (isLocalPlayer)
            {
                PlayerInput playerInput = GetPlayerInput();

                if (playerInput != null)
                {
                    pendingMoves.Add(playerInput);
                    UpdatePredictedState();
                    CmdMoveOnServer(playerInput);
                } else
                {
                    CmdAnimateOnServer();
                }
            }

            SyncState();
        }
    }

    [Command]
    void CmdMoveOnServer(PlayerInput playerInput)
    {
        state = ProcessPlayerInput(state, playerInput);
    }

    [Command]
    void CmdAnimateOnServer()
    {
        state.isWalking = false;
    }

    private PlayerInput GetPlayerInput()
    {
        PlayerInput playerInput = new PlayerInput();
        playerInput.forward += (sbyte)Input.GetAxisRaw("Horizontal");
        if (playerInput.forward == 0)
            return null;
        return playerInput;
    }

    public void SyncState()
    {
        if (isServer)
        {
            transform.position = state.position;
            GetComponent<SpriteRenderer>().flipX = state.flipped;
            animator.SetBool("isWalking", state.isWalking);
            return;
        }

        // if If we are on a client and we are the owner of the player GO,
        // we should render the predicted state. Otherwise we should render the server state
        PlayerState stateToRender = isLocalPlayer ? predictedState : state;

        GetComponent<SpriteRenderer>().flipX = stateToRender.flipped;
        transform.position = Vector2.Lerp(transform.position, stateToRender.position * PlayerLerpSpacing, PlayerLerpEasing);
        animator.SetBool("isWalking", stateToRender.isWalking);
    }

    public PlayerState ProcessPlayerInput(PlayerState previous, PlayerInput playerInput)
    {
        Vector2 newPosition = previous.position;
        float newHorizontalSpeed = playerInput.forward;
        bool newFlipped = previous.flipped;
        bool newIsWalking = previous.isWalking;

        // valid input?
        if (newHorizontalSpeed >= -1 && newHorizontalSpeed <= 1)
        {
            newPosition += new Vector2(newHorizontalSpeed, 0)
                * PlayerFixedUpdateInterval
                * speed;
            newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x, screenBounds.x);

            if (newHorizontalSpeed != 0)
            {
                newFlipped = newHorizontalSpeed > 0 ? true : false;
                newIsWalking = true;
            }
            else
            {
                newIsWalking = false;
            }
        }

        return new PlayerState
        {
            position = newPosition,
            timestamp = previous.timestamp + 1,
            flipped = newFlipped,
            isWalking = newIsWalking
        };
    }

    public void OnServerStateChanged(PlayerState oldState, PlayerState newState)
    {
        state = newState;

        if (pendingMoves != null)
        {
            // If the number of pending moves is greater than the timestamp difference between the server and last predictedState,
            // then we remove that number of pending moves.
            while (pendingMoves.Count >
                  (predictedState.timestamp - state.timestamp))
            {
                pendingMoves.RemoveAt(0);
            }

            UpdatePredictedState();
        }
    }

    public void UpdatePredictedState()
    {
        predictedState = state;

        foreach (PlayerInput playerInput in pendingMoves)
        {
            predictedState = ProcessPlayerInput(predictedState, playerInput);
        }
    }
}

