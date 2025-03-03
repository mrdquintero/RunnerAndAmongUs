using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public Vector3 position;
    public float zRot;
    public float horizSpeed;
    public bool isGrappled;
    public Vector3 grapplePos;
    public float timeScale;

    public EnemyState(Vector3 pos, float zRot, float xSpeed, bool grappled, Vector3 gPos, float time)
    {
        position = pos;
        this.zRot = zRot;
        horizSpeed = xSpeed;
        isGrappled = grappled;
        grapplePos = gPos;
        timeScale = time;
    }
}

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public LineRenderer grapple;
    List<EnemyState> positions = new List<EnemyState>();

    public float delay;
    private PlayerController playerScript;
    private Rigidbody2D playerRb;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizSpeed;
        if (!playerScript.onGround)
            horizSpeed = 0;
        else
            horizSpeed = playerRb.velocity.x;

        positions.Add(new EnemyState(player.position, playerScript.playerSprite.rotation.z, horizSpeed, playerScript.grappled, playerScript.lastGrapplePos, Time.timeScale));
        grapple.SetPosition(0, transform.position);

        if (positions.Count > delay / Time.deltaTime)
        {
            if(positions[0].timeScale == 1)
            {
                SetVars();
                return;
            }
            for (float i = 0; i < 1; i+= positions[0].timeScale)
            {
                SetVars();
            }
        }
    }

    void SetVars()
    {
        if(positions[0].horizSpeed > 0)
        {
            transform.localScale = new Vector3(-1, 1, 0);
            anim.Play("Run");

        }
        else if (positions[0].horizSpeed < 0)
        {
            transform.localScale = new Vector3(1, 1, 0);
            anim.Play("Run");
        }
        else
        {
            anim.Play("Idle");
        }
        transform.position = positions[0].position;
        transform.rotation = Quaternion.Euler(0, 0, positions[0].zRot);
        grapple.gameObject.SetActive(positions[0].isGrappled);
        grapple.SetPosition(1, positions[0].grapplePos);
        positions.RemoveAt(0);
    }
}
