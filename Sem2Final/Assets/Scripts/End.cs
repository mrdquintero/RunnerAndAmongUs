using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public Timer timeTracker;
    private PlayerController player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        timeTracker.StopTime();
        CutsceneManager.instance.isPlaying = true;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.speed, 0);
    }
}