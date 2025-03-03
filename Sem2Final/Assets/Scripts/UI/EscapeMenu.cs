using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    public GameObject escapeMenu;
    public static bool escapeOpen;
    // Start is called before the first frame update
    void Start()
    {
        escapeMenu.SetActive(false);
        escapeOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && escapeOpen)
        {
            EscapeClose();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !escapeOpen)
        {
            escapeMenu.SetActive(true);
            escapeOpen = true;
            Time.timeScale = 0;
        }
    }

    public void EscapeClose()
    {
        escapeMenu.SetActive(false);
        escapeOpen = false;
        Time.timeScale = 1;
    }
}
