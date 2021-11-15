using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaman : MonoBehaviour
{
    Animator myAnimator;
    Rigidbody2D myBody;
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
    [SerializeField] float velPower;
    [SerializeField] float frictionAmount;
    
    [SerializeField] float fallGravityMultiplier;
    private float lastGroundedTime;
    private float gravityScale;

    [SerializeField] BoxCollider2D pies;
    [SerializeField] float jumpForce;
    [SerializeField] float dashTime;
    [SerializeField] int extraJumpsValue;
    [SerializeField] GameObject vfxDeath;
    [SerializeField] AudioClip sfxDeath;

    private int extraJumps;
    public GameObject[] bullet;
    public bool shooting;
    private float shootTime;
    public float time;
    public GameObject point;
    float inTime = 0f;
    float dir = 1f;
    bool pause = false;
    


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpsValue;
        StartCoroutine(ShowTime());
        gravityScale = myBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            Movement();
            Jump();
            Falling();
            Fire();
            Dash();
        } 
    }

    IEnumerator ShowTime()
    {
        int count = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            count++;
        }
    }
    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            shootTime = 0.01f;

            GameObject obj = Instantiate(bullet[0], point.transform.position, transform.rotation) as GameObject;
            Bullet myBullet = obj.GetComponent<Bullet>();
            myBullet.Shoot(dir, moveSpeed * 2);
            if (!shooting) { shooting = true; }
            
        }
        if (shooting)
        {
            shootTime += 1 * Time.deltaTime;
            myAnimator.SetLayerWeight(0, 0);
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(0, 1);
            myAnimator.SetLayerWeight(1, 0);
        }
        if(shootTime >= time)
        {
            shooting = false;
            shootTime = 0;
        }
    }
    void Movement()
    {
        //movement input
        float moveInput = Input.GetAxis("Horizontal");
        //new movement
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - myBody.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        myBody.AddForce(movement * Vector2.right);
        if (moveInput != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(moveInput), 1);
            myAnimator.SetBool("isRunning", true);
            dir = Mathf.Sign(moveInput);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
        //friction
        if(lastGroundedTime > 0 && isGrounded() && Mathf.Abs(moveInput) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(myBody.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(myBody.velocity.x);
            myBody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    void Jump()
    {
        //bool isGrounded = pies.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGrounded())
        {
            myAnimator.SetBool("isFalling", false);
            myAnimator.SetBool("isJumping", false);
            extraJumps = extraJumpsValue;
        }
            
        if (Input.GetKeyDown(KeyCode.Z) && extraJumps > 0)
        {
            myAnimator.SetTrigger("TakeOff");
            myAnimator.SetBool("isJumping", true);
            
            myBody.velocity = new Vector2(myBody.velocity.x, jumpForce);

            lastGroundedTime = 0;
            extraJumps--;
        }
        
    }
    void Falling()
    {
        if (myBody.velocity.y < 0 && !myAnimator.GetBool("isJumping") && !isGrounded())
        {
            myAnimator.SetBool("isFalling", true);
            myBody.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            myBody.gravityScale = gravityScale;
        }
    }
    void Dash()
    {
        if(Input.GetKeyDown(KeyCode.C) && isGrounded())
        {
            inTime = Time.time;

        }
        if (Input.GetKey(KeyCode.C) && isGrounded())
        {
            if (inTime + dashTime >= Time.time)
            {
                myAnimator.SetBool("isDashing", true);
                myBody.AddForce(Vector2.right * moveSpeed * 6f * dir);
            }
            else
                myAnimator.SetBool("isDashing", false);
        }
        else
        {
            myAnimator.SetBool("isDashing", false);
        }
            
        
    }
    bool isGrounded()
    {
        return pies.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
    void AfterTakeOffEvent()
    {
        myAnimator.SetBool("isJumping", false);
        myAnimator.SetBool("isFalling", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine("Die");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine("Die");
        }
    }
    IEnumerator Die()
    {
        pause = true;
        myBody.isKinematic = true;
        myAnimator.SetBool("isDead", true);
        yield return new WaitForSeconds(1);
        AudioSource.PlayClipAtPoint(sfxDeath, Camera.main.transform.position);
        Instantiate(vfxDeath, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
