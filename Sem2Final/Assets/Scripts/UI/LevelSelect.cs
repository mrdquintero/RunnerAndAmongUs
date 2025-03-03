using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LevelSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject gui;
    public Camera cam;
    public TMP_Text timeText;
    public string key;
    // Start is called before the first frame update
    void Start()
    {
        gui.SetActive(false);
        if (PlayerPrefs.HasKey(key))
            timeText.text = PlayerPrefs.GetFloat(key).ToString("0.000");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gui.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gui.SetActive(false);
    }
}
