using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Choose_Options : MonoBehaviour
{
    private GameController gameController;
    public int mode = 0;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SinglePlayer()
    {
        mode = 1;
        SceneManager.LoadScene("MainGame");
    }
    public void TwoPlayer()
    {
        mode = 2;
        SceneManager.LoadScene("MainGame");
    }
}
