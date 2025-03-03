using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGrapple : MonoBehaviour
{
    private bool isPlaying = false;
    private PlayerController player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Time.timeScale = 0.1f;
            isPlaying = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 1;
            isPlaying = false;
        }
    }

    private void Update()
    {
        if(isPlaying && player.grappled)
        {
            isPlaying = false;
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
