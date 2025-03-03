using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public GameObject finalText;
    public TMP_Text highScore;
    public GameObject newRecordText;
    public TMP_Text text;
    public float time;
    public Animator timerAnim;
    public string playerPrefName;

    private bool done;

    public SceneChange sceneChanger;
    // Start is called before the first frame update
    void Start()
    {
        done = false;
        time = 0;
        UpdateText();
        finalText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;
        time += Time.deltaTime;

        UpdateText();
    }

    void UpdateText()
    {
        text.text = "Time: " + time.ToString("0.000");
    }

    public void StopTime()
    {
        done = true;
        text.text = time.ToString("0.000");
        timerAnim.Play("FinalTime");
        if (!PlayerPrefs.HasKey(playerPrefName) || time < PlayerPrefs.GetFloat(playerPrefName))
        {
            PlayerPrefs.SetFloat(playerPrefName, time);
            newRecordText.SetActive(true);
        }
        else
            newRecordText.SetActive(false);

        Invoke("EnableEndScreen", 1f);
    }

    void EnableEndScreen()
    {
        highScore.text = "Best Time: " + PlayerPrefs.GetFloat(playerPrefName).ToString("0.000");
        finalText.SetActive(true);
        Time.timeScale = 0;
    }
}
