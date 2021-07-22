using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput; // We using this nameSpace so that if we want to make this game for mobile or any other device, without having to chnag the controls.

public class Player : MonoBehaviour
{
    // Configs
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKickForce = new Vector2(25f, 25f);
    [SerializeField] AudioClip duckQuack;
    [SerializeField] [Range(0, 1)] float duckQuackSFXVolume;

    // States
    bool isAlive = true;
    

    // Cached component references
    /* These are an example of "caches". You store something in these variables, which may or maynot be necessary */
    Rigidbody2D playerRb;
    SpriteRenderer playerSpriteRenderer;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    AudioSource audioSource;
    float playerGravityAtStart;


    // Messages then Methods
    private void Start()
    {
        // These are an example of "caches"
        // The RISK of referencing is that if we remove the specified component in the inspector, countless errors will appear. One fo risks of "caching"
        // ******* "FindObjectofType<...>()" looks for a GameObject in the whole game, whereas "GetComponent<...>();" looks for a GameObject in just the gameObject that this script/class is atatched to. ***
        playerRb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        playerGravityAtStart = playerRb.gravityScale;
    }
    

    void Update()
    {
        if (!isAlive) { return;  } //if isAlive is equal to false, then return or dont continue with the rest of the code below.

        Move();
        Jump();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void Move()
    {
            var horizontalMovement = CrossPlatformInputManager.GetAxis("Horizontal");
            Vector2 playerMovement = new Vector2(horizontalMovement * speed, playerRb.velocity.y);
            playerRb.velocity = playerMovement;
            //print(playerRb.velocity);

            // Controls the running animation
            bool playerMoving = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
            playerAnimator.SetBool("Running", playerMoving);

            // Another way of playing the running animation
            /* if (playerRb.velocity.x > 0 || playerRb.velocity.x < 0)
            {
                playerAnimator.SetBool("Running", true);
            }
            else if (playerRb.velocity.x == 0)
            {
                playerAnimator.SetBool("Running", false);
            } */

            // Another way of flip the sprite, doesnt change the rotation of the player tho.
            /* if (horizontalMovement > 0)
            {
                playerSpriteRenderer.flipX = true;
            }
            else if (horizontalMovement < 0 )
            {
                playerSpriteRenderer.flipX = false;
            } */

    }

    private void ClimbLadder()
    {
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
        {
            playerRb.gravityScale = playerGravityAtStart;
            return; // if the player is not touching the ground, then dont continue with the rest of the code.
        }
       
        var verticalMovement = CrossPlatformInputManager.GetAxis("Vertical");
        playerRb.velocity = new Vector2(playerRb.velocity.x, verticalMovement * climbSpeed);
        playerRb.gravityScale = 0; // Sets the gravity to 0 when on ladder so that we dont float on ladder.

        bool isClimbingLadder = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("Jumping", isClimbingLadder); // by using this we don't have to setBool to true, and then set it the false somewhwere else.

    }

    private void Jump()
    {
        // "Layer" and "Tag" are similar. Its is just an extra step of organization. We can have a GameObject having both a "Tag" and a "Layer".
        // Checks if player is touching/colliding "Ground" layer, and then prevents him from jumping again.
        //bool isOnGround = playerCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        /* // if player is not colliding with the ground, then allow the player to jump (THIS IS RICK'S WAY)
         * if player is not on ground then RETURN, don't continue with the rest of the code, but if he is on the ground => then let him jump *****
        // its better to say if the condition is NOT met then dont let the player jump. MORE EFFECTIVE */
        
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump") /* && isOnGround */)
        {
            playerRb.velocity += new Vector2(0f, jumpForce); // we ADD not multiply when jumping
            audioSource.PlayOneShot(duckQuack, duckQuackSFXVolume);
        }

        playerAnimator.SetBool("Jumping", CrossPlatformInputManager.GetButtonDown("Jump"));
    }


    // Performance wise, it is better to calculate the direction rather than just flip the sprite (which caches variables)
    /* Calculating uses the computer processor, while caching uses the computer memory (today, processors are way faster than memories which 
     * is why it is better to calculate things than caching them) */
    // DON'T CACHE IT IF YOU CAN CALCULATE IT => keeps the code more functional 
    private void FlipSprite()
    {
        // if player is moving horizontally
        bool playerMoving = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon; //  = playerRb.velocity.x > 0
        if (playerMoving)
        {
            // reverse the current scaling of x
            transform.localScale = new Vector2(Mathf.Sign(-playerRb.velocity.x), 1f);
        }
        
    }

    private void Die()
    {
        if(playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Slime", "Hazards")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Die");
            var deathKickDirection = new Vector2(Mathf.Sign(-playerRb.velocity.x), Mathf.Sign(- playerRb.velocity.y));
            playerRb.velocity = deathKickDirection * deathKickForce; 
            transform.Rotate(0, 0, Random.Range(45, 90));
            FindObjectOfType<GameSesssion>().ProcessPlayerDeath();
        }
    }
}
