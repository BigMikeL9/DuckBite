using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlimeMovement : MonoBehaviour
{

    // Configs
    [SerializeField] float slimeSpeed = 1f;

    //States
    // bool goingLeft = false;
    // bool goingRight = true;

    // Cached References
    Rigidbody2D slimeRb;
    Animator slimeAnimator;
    BoxCollider2D slimeBoxCollider;


    // Start is called before the first frame update
    void Start()
    {
        slimeRb = GetComponent<Rigidbody2D>();
        //slimeAnimator = GetComponent<Animator>();
        slimeBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SlimeMove();
    }

    private void SlimeMove()
    {
        //slimeAnimator.Play("Slime Bounce");
        slimeRb.velocity = new Vector2(slimeSpeed, 0f);
      
        
        /* slimeAnimator.Play("Slime Bounce");
        if (goingLeft)
        {
            slimeRb.velocity = new Vector2(-slimeSpeed, 0f);
            transform.localScale = new Vector2(Mathf.Sign(slimeRb.velocity.x), 1f);
        }
        else if (goingRight)
        {
            slimeRb.velocity = new Vector2(slimeSpeed, 0f);
            transform.localScale = new Vector2(Mathf.Sign(slimeRb.velocity.x), 1f);
        } */
    }

    /* private void OnTriggerEnter2D(Collider2D other)
     {
         if (other.gameObject.tag == "Slime Left Boundary")
         {
             Debug.Log("Slime hit left boundary!!!");
             goingLeft = false;
             goingRight = true;
         }
         else if (other.gameObject.tag == "Slime Right Boundary")
         {
             Debug.Log("Slime hit right boundary!!!!!!");
             goingLeft = true;
             goingRight = false;
         }
     } */



    // Other method of moving enemies
    private void OnTriggerExit2D(Collider2D other)
    {
        if (slimeBoxCollider) // when slimeBoxCollider is no longer touching the "background" collider.
        {
            transform.localScale = new Vector2(Mathf.Sign(-slimeRb.velocity.x), 1f); // sets the sprite direction/scale opposite to what it currently is.
            slimeSpeed = -slimeSpeed; // sets the velocity opposite to what it currently is.
        }
    }
}
