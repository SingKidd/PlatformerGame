using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]float runSpeed = 10f;
    [SerializeField]float jumpSpeed = 5f;
    [SerializeField]float climbSpeed = 2f;
    //[SerializeField]Vector2 deathKick = new Vector2(Random.Range(-15f,15f), 10f);
    [SerializeField]GameObject bullet;
    [SerializeField]Transform gun;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;

    CapsuleCollider2D myBodyCollider;

    BoxCollider2D myFeetCollider;
    CircleCollider2D enemyCollider;

    
    
    float gravityScaleAtStart;

    bool isAlive = true;

    Animator myAnimator;
    void Start()
    {
        
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        //enemyCollider = EnemyMovement.FindObjectOfType<CircleCollider2D>();
        
        gravityScaleAtStart = myRigidbody.gravityScale;
        
    }

    void Update()
    {
        if(!isAlive)
        {
            return;
        }
        
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive)
        {
            return;
        }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);

    }

    void OnJump(InputValue value)
    {
        //int groundLayerMask = LayerMask.GetMask("Ground");
        //bool isTouchingGround = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if(!isAlive)
        {
            return;
        }
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) )
        {
            return;
        }

        if(value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }

    }

    void OnFire(InputValue value)
    {
        if(!isAlive)
        {
            return;
        }
        Instantiate(bullet, gun.position, transform.rotation);
        Debug.Log("Fire!");

    }



    void ClimbLadder()
    {
        //bool isPlayerClimbing;
        
         if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {            
            myRigidbody.gravityScale = gravityScaleAtStart;
            //isPlayerClimbing = false;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        
        Vector2 playerVelocity = new Vector2(myRigidbody.velocity.x , moveInput.y * climbSpeed);
        myRigidbody.velocity = playerVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;

        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);

        
        //isPlayerClimbing = true;
        //myAnimator.SetBool("isClimbing", isPlayerClimbing);
        
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed , myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        
        
        myAnimator.SetBool("isRunning" , playerHasHorizontalSpeed);       
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);

        }
    }




    
    void Die()
    {
        
        //Disable movements
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Traps")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Traps", "Enemies")))
        {            
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity =new Vector2(Random.Range(-15f,15f), 15f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
            
        }
        
    }
}
