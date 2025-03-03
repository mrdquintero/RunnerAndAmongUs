using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRunning : MonoBehaviour
{
    private PlayerController playerScript;
    private Rigidbody2D playerRb;
    public GameObject barrier;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        Animator playerAnim = playerScript.anim;

        playerAnim.Play("Run");
        playerRb.velocity = new Vector2(playerScript.speed, 0);
        CutsceneManager.instance.isPlaying = true;
        Invoke("EndCutscene", 1f);
    }

    void EndCutscene()
    {
        playerRb.velocity = Vector2.zero;
        CutsceneManager.instance.isPlaying = false;
        barrier.SetActive(true);
    }
}
