using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            PlayerInput playerInput = GetPlayerInput();

            if (playerInput != null)
            {
                //CmdMoveOnServer(playerInput);
            } else
            {

            }
        }
    }

    private PlayerInput GetPlayerInput()
    {
        PlayerInput playerInput = new PlayerInput();
        playerInput.forward += (sbyte)Input.GetAxisRaw("Horizontal");
        if (playerInput.forward == 0)
            return null;
        return playerInput;
    }
}
