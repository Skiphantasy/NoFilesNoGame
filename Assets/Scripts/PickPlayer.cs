using UnityEngine;

public class PickPlayer : MonoBehaviour
{
    public bool pickedByPlayer1 = false;
    public bool pickedByPlayer2 = false;

    public bool pickingPlayer1 = false;
    public bool pickingPlayer2 = false;

    public bool player1IsPickable = false;
    public bool player2IsPickable = false;


    public Transform interactionZonePlayer2;
    public Transform interactionZonePlayer1;

    GameObject player1;
    GameObject player2;

    private void Start()
    {
        player2 = GameObject.FindGameObjectWithTag("Player2");
        player1 = GameObject.FindGameObjectWithTag("Player1");
    }

    private void Update()
    {
        if (this.gameObject.tag == "Player1")
        {
            if (player2IsPickable && !pickingPlayer2 && !pickingPlayer1)
            {

                if (Input.GetAxisRaw("Grab") != 0)
                {
                    pickingPlayer2 = true;
                    player2.transform.SetParent(interactionZonePlayer1);
                    player2.GetComponent<Rigidbody2D>().isKinematic = true;
                }
            }

            if (pickingPlayer2 && Input.GetAxisRaw("Grab") == 0)
            {
                pickingPlayer2 = false;
                player2.transform.SetParent(null);
                player2.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
        else if (this.gameObject.tag == "Player2")
        {
            if (player1IsPickable && !pickingPlayer1 && !pickingPlayer2)
            {

                if (Input.GetAxisRaw("Grab2") != 0)
                {
                    pickingPlayer1 = true;
                    player1.transform.SetParent(interactionZonePlayer2);
                    player1.GetComponent<Rigidbody2D>().isKinematic = true;
                }
            }

            if (pickingPlayer1 && Input.GetAxisRaw("Grab2") == 0)
            {
                pickingPlayer1 = false;
                player1.transform.SetParent(null);
                player1.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.tag == "Player1" && collision.tag == "Player2InteractionZone")
        {
            player2IsPickable = true;
        }
        else if (this.gameObject.tag == "Player2" && collision.tag == "Player1InteractionZone")
        {
            player1IsPickable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.tag == "Player1" && collision.tag == "Player2InteractionZone")
        {
            player2IsPickable = false;
        }
        else if (this.gameObject.tag == "Player2" && collision.tag == "Player1InteractionZone")
        {
            player1IsPickable = false;
        }
    }
}
