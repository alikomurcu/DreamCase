using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // Singleton instance
    public static GameUI Instance { get; private set; }
    [SerializeField] public GameObject moveCount;
    [SerializeField] public GameObject score;
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
        // Set the move count
        var t = moveCount.GetComponent<Text>();
        t.text = GameManager.Instance.currentMoveCount.ToString();
    }
}
