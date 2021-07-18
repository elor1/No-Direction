using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Intro
    }

    public static GameState currentState;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Intro;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings)
        {
            if (currentState == GameState.Intro)
            {
                GameObject intro = GameObject.FindGameObjectWithTag("Intro");
                if (intro)
                {
                    anim = intro.GetComponent<Animator>();
                    anim.SetBool("IsTransitioning", true);
                }
            }
        }
    }
}
