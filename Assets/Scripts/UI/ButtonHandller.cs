using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandller : MonoBehaviour
{
    /*
     * This class is responsible from handling the button clicks, from ui.
     */
    // Singleton instance
    public static ButtonHandller Instance { get; private set; }
    [SerializeField] private GameObject levelsPopup;
    [SerializeField]private GameObject levelsButton;
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            // set the instance
            Instance = this;
        }
    }

    public void PlayButton(int level)
    {
        // load scene
        GameManager.Instance.currentLevel = level;
        SceneManager.LoadScene("SampleScene");
        GameManager.Instance.SetGrid();
    }
    public void LevelsButton()
    {
        // levels popup TODO: add animations here
        levelsButton.SetActive(false);
        levelsPopup.SetActive(true);
        LevelsPopupHandler.Instance.SetLevelsPopup();
    }
    
    public void MainMenuButton()
    {
        // main menu TODO: add animations here
        GameManager.Instance.DestroyGrid();
        SceneManager.LoadScene("MainScene");
        
    }
    
}
