using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScreen : MonoBehaviour
{
    public GameObject currScreen;

    private bool swipeRight;

    public Transform rightStart;
    public Transform leftStart;
    public Transform midStart;

    public void SetSwipe(bool swipeRight)
    {
        this.swipeRight = swipeRight;
    }
    public void ChangeScene(GameObject nextScreen)
    {
        if(swipeRight)
        {
            StartCoroutine(Tween(currScreen.transform, midStart.position, leftStart.position));
            StartCoroutine(Tween(nextScreen.transform, rightStart.position,midStart.position));
        }
        else
        {
            StartCoroutine(Tween(currScreen.transform, midStart.position, rightStart.position));
            StartCoroutine(Tween(nextScreen.transform, leftStart.position, midStart.position));
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Tween(Transform lerpObject, Vector3 start, Vector3 end)
    {
        float t = 0;
        while(t < 1)
        {
            lerpObject.position = new Vector3(sineInterp(start.x, end.x, t), midStart.position.y, 0);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
        currScreen = lerpObject.gameObject;
    }

    float sineInterp(float start, float end, float t)
    {
        return -(Mathf.Cos(Mathf.PI* t) - 1) * (end - start) / 2 + start;
    }
}
