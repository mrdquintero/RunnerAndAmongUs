using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStates : MonoBehaviour
{
    public int animState;
    public Animator animator;
    public void ChangeState(int state)
    {
        animState = state;
    }
}
