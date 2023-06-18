using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandller : MonoBehaviour
{
    
    public void OnClick(string sceneName)
    {
        // load scene
        SceneManager.LoadScene(sceneName);
        Debug.Log("Scene loaded: " + sceneName);
    }
}
