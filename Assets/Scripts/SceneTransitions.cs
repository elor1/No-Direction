using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{
    void EndTransition()
    {
        GameManager.currentState = GameManager.GameState.Playing;
        Animator anim = GetComponent<Animator>();
        anim.SetBool("IsTransitioning", false);
    }
}
