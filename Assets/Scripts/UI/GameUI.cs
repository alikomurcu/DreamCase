using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // Singleton instance
    public static GameUI Instance { get; private set; }
    [SerializeField] public GameObject moveCount;
    [SerializeField] public GameObject score;
    [SerializeField] private GameObject congratsPopup;
    [SerializeField] private GameObject noPossibleMoves;
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

    public void OnEnable()
    {
        // initially set the congrats popup to inactive
        congratsPopup.SetActive(false);
        noPossibleMoves.SetActive(false);
        // Set the move count
        var t = moveCount.GetComponent<Text>();
        t.text = GameManager.Instance.currentMoveCount.ToString();
    }
    
    public void RenderCount()
    {
        var t = moveCount.GetComponent<Text>();
        t.text = GameManager.Instance.currentMoveCount.ToString();
    }    
    
    public void RenderScore()
    {
        var t = score.GetComponent<Text>();
        t.text = GameManager.Instance.currentScore.ToString();
    }
    
    public void RenderNoPossibleMoves()
    {
        noPossibleMoves.SetActive(true);
    }
    
    public void CongratsPopup()
    {
        StartCoroutine("Congrats");
    }

    IEnumerator Congrats()
    {
        congratsPopup.SetActive(true);
        yield return new WaitForSeconds(3);
        congratsPopup.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }

}
