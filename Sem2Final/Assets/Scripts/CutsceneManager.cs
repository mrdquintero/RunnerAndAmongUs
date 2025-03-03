using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public bool isPlaying;
    public static CutsceneManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        isPlaying = false;
    }
}
