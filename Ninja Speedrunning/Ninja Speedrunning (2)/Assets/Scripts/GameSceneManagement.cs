using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManagement : MonoBehaviour
{
    
    /*
     Scene 0 = Start menu
    Scene 1 = Game Scene*/
  
    public void LoadMenu()
    {
        SceneManager.LoadScene(0); 
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
