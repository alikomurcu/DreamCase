using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class LevelsPopupHandler : MonoBehaviour
{
    /*
    Note that this class utilizes a prefab for level rows, which is created in the editor.
    It is important that the structure of the prefab is as follows:
        1. The first child is a text object, which is used to display the level number and the highest score.
        2. The second child is a button object, which is used to play the level.
    */

    // Singleton instance
    public static LevelsPopupHandler Instance { get; private set; }
    [SerializeField] private GameObject scrollViewObject;
    [SerializeField] private GameObject scrollViewport;
    [SerializeField] private GameObject scrollContent;
    [SerializeField] private GameObject levelPrefab;
    private int levelOffset = -45;      // the offset y value between popup rows

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
        scrollViewObject.SetActive(false);  // initially set the scroll view to inactive
    }

    public void SetLevelsPopup()
    {
        StartCoroutine("Wait1Second");
        Debug.Log("Levels popup set.");
        // Sets the level popup according to the size of the levels dictionary,
        // i.e. it updaates the scroll view content if online levels downloaded.
        Dictionary<int, Tuple<bool, int>> levelsDictionary = GameManager.Instance.gameState.LevelsDictionary;
        // iterate in the dictionary
        int i = 0;
        foreach (KeyValuePair<int, Tuple<bool, int>> level in levelsDictionary)
        {
            // create a new level prefab as child of scroll view port
            GameObject levelObject = Instantiate(levelPrefab, scrollContent.transform, false);
            // set the position of the level object
            levelObject.transform.localPosition += new Vector3(0, levelOffset * i++, 0);
            levelObject.name = "Level" + level.Key;
            
            // Select the children and set them accordingly
            // set the text
            Transform c0 = levelObject.transform.GetChild(0);    // this is the first child of the level object, i.e., text
            c0.GetComponent<TMP_Text>().text = levelObject.name + " - Moves" + 
                                               "\nHighest score: " + level.Value.Item2;
            // set the play button event
            Transform c1 = levelObject.transform.GetChild(1);    // this is the second child of the level object, i.e., play button
            c1.GetComponent<Button>().onClick.AddListener(delegate { ButtonHandller.Instance.PlayButton((int) level.Key); });       // when click play, go to PlayButton function
        }
    }
    
    public IEnumerator Wait1Second()
    {
        // wait 1 second
        Debug.Log("Coroutine started.");
        yield return new WaitForSeconds(1);
        

    }
}
