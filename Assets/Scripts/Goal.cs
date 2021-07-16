using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.StopMovement();
            anim.SetBool("IsCollected", true);
        }
    }

    void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
