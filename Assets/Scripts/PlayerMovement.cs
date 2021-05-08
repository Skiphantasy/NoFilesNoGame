using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   
    public float speed;                
    public float jumpHeight;
    private Rigidbody2D rb2d;        //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private bool isGrounded;
    private const float MAX_SPEED = 20f;
    private Animator anim;
    public AudioSource jumpSound;
    public List<AudioClip> audioClips;

    void Start()
    {
        jumpSound = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGrounded = false;
    }

    void FixedUpdate()
    {
        var hsp = 0f;
        var vsp = 0f;
        float moveHorizontal = this.gameObject.CompareTag("Player1")? Input.GetAxisRaw("Horizontal2") : Input.GetAxisRaw("Horizontal");
        var jumpKey = this.gameObject.CompareTag("Player1") ? Input.GetAxisRaw("Jump2") : Input.GetAxisRaw("Jump");

        if(isGrounded && jumpKey != 0)
        {
            jumpSound.clip = audioClips[0];
            jumpSound.Play();
            vsp = 100 * jumpHeight;
        }
        // By Doing this we control the sliding. Using Input.GetAxis value keeps sliding
        if (rb2d.velocity.x <= MAX_SPEED && rb2d.velocity.x >= -MAX_SPEED)
        {
            hsp = moveHorizontal * speed * 10;
            if (hsp > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            } else if (hsp < 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        } 
        rb2d.AddForce(new Vector2(hsp, vsp));

        if (Mathf.Abs(rb2d.velocity.x) > 0)
        { 
            anim.SetBool("isWalking", true);
        } else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.LogError("Grounded");
            anim.SetBool("isGrounded", true);
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.LogError("Not Grounded");
            anim.SetBool("isGrounded", false);
            isGrounded = false;
        }
    }
}
