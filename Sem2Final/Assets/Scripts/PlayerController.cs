
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;

    [Header("Health")]
    public float maxHealth; // Slime capacity
    public float health;
    public AnimStates deathScreen;

    [Header("Player Info (Non-Mutable)")]
    public int enterGroundCollision;
    public Vector2 hitDirection;
    public bool stunned;
    public int moveDirection;
    public bool isDead;
    public bool isHoldingMouse;
    public Animator anim;

    private Camera cam;
    public GameObject vcam;


    [Header("Level")]
    // public LevelManager currLevel;
    public GameObject enemy;

    int groundDetect = 0;
    private bool isJumping;
    public float gravityScaler;
    private float gravity;
    public float speed;
    public float jumpForce;
    public bool onGround;

    string currCollision;
    public GroundCheck collisionCheckScript;


    public LineRenderer grapple;
    public Transform grappleEnd;
    private bool grappleHooked;
    private float length;
    public LayerMask grappleLayer;
    public LayerMask groundLayer;

    float angle = 0;
    float angularVelocity;

    private float maxMomentum;
    private float horizMomentum;
    Vector2 mousePos;
    private Transform lastSpawnpoint;
    public Transform firstSpawn;

    private float coyoteTimer;
    private float size = 2;
    private Transform sprite;
    private Transform grappleSprite;
    public Transform playerSprite { get { return sprite; } }
    //public GameObject particleEmitterObject;


    void Start()
    {
        sprite = transform.GetChild(0);
        grappleSprite = transform.GetChild(1);
        moveDirection = 1;
        //jumpParticle = particleEmitterObject.GetComponent<ParticleSystem>();
        playerRb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        grappleHooked = false;
        length = 0;
        isDead = false;
        StartCoroutine(ResetEnemy());
        lastSpawnpoint = firstSpawn;
        coyoteTimer = -10f;
    }

    void Update()
    {
        if (CutsceneManager.instance.isPlaying)
        {
            return;
        }
            
        if(isDead)
        {
            playerRb.velocity = Vector2.zero;
            if(deathScreen.animState == 2)
            {
                vcam.SetActive(false);
                transform.position = lastSpawnpoint.position;
                cam.transform.position = transform.position;
                vcam.SetActive(true);
                UpdateDeathScreenPos();
                StartCoroutine(ResetEnemy());
                isDead = false;
            }
            return;
        }

        grapple.SetPosition(0, grappleSprite.position);
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //GrappleSprite movement
        float grappleAngle;
        if (grappleHooked)
        {
            grappleAngle = Mathf.Atan2(transform.position.y - grappleEnd.position.y, transform.position.x - grappleEnd.position.x);
        }
        else
        {
            grappleAngle = Mathf.Atan2(transform.position.y - mousePos.y, transform.position.x - mousePos.x);
        }
        grappleSprite.rotation = Quaternion.Euler(0, 0, grappleAngle * Mathf.Rad2Deg + 180);




        if (grappleHooked && !onGround)
        {
            MovePlayerRope();
        }
        else
        {
            MovePlayerStandard();
        }
        

        if(!grappleHooked && Input.GetKeyDown(KeyCode.Mouse0))
        {
            grapple.gameObject.SetActive(true);
        }

        if(grapple.gameObject.activeSelf && !grappleHooked)
        {
            ShootGrapple();
            length += Time.deltaTime * 200;
            if (length > 20)
            {
                grapple.gameObject.SetActive(false);
                length = 0;
                grappleHooked = false;
                angularVelocity = 0;
            }
        }

        if((Input.GetKeyDown(KeyCode.Space) && grappleHooked))
        {
            horizMomentum = -angularVelocity * Mathf.Sin(angle) * length;
            maxMomentum = Mathf.Abs(horizMomentum);
            playerRb.velocity = angularVelocity * length * new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));
            grapple.gameObject.SetActive(false);
            playerRb.gravityScale = gravity;
            length = 0;
            grappleHooked = false;
            angularVelocity = 0;
            Debug.Log(horizMomentum);
        }

        if (onGround)
            coyoteTimer = 0.05f;

        if (coyoteTimer > 0)
            coyoteTimer -= Time.deltaTime;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spawnpoint"))
        {
            lastSpawnpoint = other.gameObject.transform;
        }
        if(other.gameObject.CompareTag("Death"))
        {
            Death();
        }
    } 

    public IEnumerator CameraTransfer(float time, Tilemap tilemap, TileBase doorTile, Vector3 topPos, Vector3 botPos)
    {
        yield return new WaitForSeconds(time);
    }

    public IEnumerator Knockback(Vector2 force, float stunTimer)
    {
        stunned = true;
        playerRb.velocity = force;
        yield return new WaitForSeconds(stunTimer);
        stunned = false;
    }

    public IEnumerator SlowTime()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.01f);
        Time.timeScale = 1;
    }

    public void SetDirection(int newDir)
    {
        moveDirection = newDir;
        transform.localScale = new Vector3(moveDirection, transform.localScale.y, transform.localScale.z);
    }

    public void UnParent()
    {
        Debug.Log("lol");
        transform.parent = null;
    }

    public void MovePlayerStandard()
    {
        if (CutsceneManager.instance.isPlaying)
            return;
        if (onGround)
            currCollision = CheckCollisions();

        //Player Movement
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (!onGround)
        {
            playerRb.AddForce(Vector2.down * gravity, ForceMode2D.Force);
            if (playerRb.velocity.x == horizontal * speed)
            {
                horizontal = Input.GetAxisRaw("Horizontal");

            }
            else
            {
                horizontal = Input.GetAxis("Horizontal") * 2;

            }


        }

        if(Mathf.Abs(horizMomentum) < speed)
        {
            sprite.eulerAngles = Vector3.zero;
            horizMomentum = 0;
            playerRb.velocity = new Vector2(Mathf.Clamp(horizontal * speed, -speed, speed), Mathf.Clamp(playerRb.velocity.y, -30, Mathf.Infinity));
        }
        else
        {
            horizMomentum = Mathf.Clamp(horizMomentum + horizontal * Time.deltaTime * 10f, -maxMomentum, maxMomentum);
            playerRb.velocity = new Vector2(horizMomentum, Mathf.Clamp(playerRb.velocity.y, -30, Mathf.Infinity));
            sprite.Rotate(0, 0, Time.deltaTime * 50 * horizMomentum);
        }

        //Jumpingd
        if (Input.GetKey(KeyCode.Space) && playerRb.velocity.y > 0)
        {
            playerRb.gravityScale = gravityScaler / 2;
        }
        else if(playerRb.velocity.y > 0)
        {
            playerRb.gravityScale = gravityScaler * 1.25f;
        }
        else
        {
            playerRb.gravityScale = gravityScaler * .75f;
        }


        transform.localScale = new Vector3(size * -Mathf.Sign(playerRb.velocity.x), transform.localScale.y, 0);
        //Movement
        if (!Mathf.Approximately(playerRb.velocity.x, 0.0f) && onGround)
        {
            anim.Play("Run");
        }
        else
        {
            anim.Play("Idle");
        }

        if (Input.GetKey(KeyCode.Space) && (onGround || coyoteTimer > 0) && !isJumping)
        {
            onGround = false;
            isJumping = true;
            playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            coyoteTimer = -10f;
        }

        if(grappleHooked)
        {
            angle = Mathf.Atan2(transform.position.y - grappleEnd.position.y, transform.position.x - grappleEnd.position.x);
            length = (transform.position - grappleEnd.position).magnitude;
        }
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (!onGround)
        {
            currCollision = CheckCollisions();
        }
    }

    protected string CheckCollisions()
    {
        string ceilingCheck = collisionCheckScript.CeilingCollision();
        string groundCheck = collisionCheckScript.GroundCollision();

        onGround = !groundCheck.Equals(""); 
        if (!groundCheck.Equals(""))
        {
            ResetToground();
        }

        if (!ceilingCheck.Equals(""))
        {
            isJumping = false;
            playerRb.gravityScale = gravity;
        }

 

        return collisionCheckScript.GroundCollision();
    }

    

    public int normalizeFloat(float value)
    {
        if (value < 0)
            return -1;
        if (value > 0)
            return 1;
        return 0;
    }

    private float bounceNum;
    public void ResetToground()
    {
        sprite.eulerAngles = Vector3.zero;
        coyoteTimer = -1f;
        onGround = true;
        isJumping = false;
        playerRb.gravityScale = gravity;
        if (Mathf.Abs(horizMomentum) > 0 && Input.GetKey(KeyCode.Space))
        {
            if (bounceNum > 0)
                maxMomentum = Mathf.Clamp(maxMomentum / 1.5f, 0, maxMomentum);
            bounceNum++;
            return;
        }
        bounceNum = 0;
        horizMomentum = 0;
    }

    public void ShootGrapple()
    {
        Vector2 mouseDir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
        grapple.SetPosition(1, (Vector2) transform.position + mouseDir * length);

        RaycastHit2D ray = Physics2D.Linecast(transform.position, (Vector2)transform.position + mouseDir * length, grappleLayer);
        RaycastHit2D groundRay = Physics2D.Linecast(transform.position, (Vector2)transform.position + mouseDir * length, groundLayer);
        Debug.DrawLine(transform.position, (Vector2) transform.position + mouseDir * length);

        if (groundRay.collider != null)
        {
            grapple.gameObject.SetActive(false);
            length = 0;
            grappleHooked = false;
            angularVelocity = 0;
            return;
        }
        if(ray.collider != null)
        {
            bounceNum = 0;
            length = ray.distance;
            grappleEnd.position = ray.point;
            grapple.SetPosition(1, grappleEnd.position);
            angle = Mathf.Atan2(transform.position.y - grappleEnd.position.y, transform.position.x - grappleEnd.position.x);
            grappleHooked = true;
            angularVelocity = (playerRb.velocity.y * Mathf.Cos(angle) - playerRb.velocity.x * Mathf.Sin(angle)) / length;
        }
    }

    public void MovePlayerRope()
    {
        playerRb.velocity = Vector2.zero;
        if (!onGround)
        {
            angularVelocity -= Mathf.Cos(angle) * Time.deltaTime * 30f / length + Input.GetAxisRaw("Horizontal") * Mathf.Sin(angle) * Time.deltaTime;
        }
        else
        {
            angularVelocity = 0;
        }
        angle += angularVelocity * Time.deltaTime;
        transform.position = grappleEnd.position + length * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        sprite.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + 90);

        if (CheckRopeCollisions(angularVelocity * new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle)))) angularVelocity = 0;

    }

    public bool CheckRopeCollisions(Vector2 direction)
    {
        if (angularVelocity == 0) return false;

        Vector2 normDir = direction.normalized;
        string collisions = "";
        if (normDir.x > 0)
            collisions += collisionCheckScript.RightWallCollision();
        else
            collisions += collisionCheckScript.LeftWallCollision();
        if (normDir.y > 0)
            collisions += collisionCheckScript.CeilingCollision();
        else
            collisions += collisionCheckScript.GroundCollision();

        if (!collisions.Equals(""))
            return true;
        return false;
    }

    public void Death()
    {
        isDead = true;
        grappleHooked = false;
        isJumping = false;
        grapple.gameObject.SetActive(false);
        UpdateDeathScreenPos();
        deathScreen.animator.Play("In");
    }

    public IEnumerator ResetEnemy()
    {
        enemy.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => Vector2.Distance(transform.position,  lastSpawnpoint.position) > 2);
        yield return new WaitForSeconds(1.0f);
        enemy.SetActive(true);
    }

    void UpdateDeathScreenPos()
    {
        deathScreen.gameObject.transform.position = cam.WorldToScreenPoint(transform.position);
    }

    public Vector3 lastGrapplePos { get { return grappleEnd.position; } }
    public bool grappled { get { return grappleHooked; } }
}
