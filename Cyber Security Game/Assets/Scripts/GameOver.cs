using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //NetworkManager.singleton.StopClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ManinMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
