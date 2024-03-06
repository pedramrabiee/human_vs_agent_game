using UnityEngine;
using System.Collections;
using System.Collections.Generic;  //lets us use lists
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Game");
    }


}
