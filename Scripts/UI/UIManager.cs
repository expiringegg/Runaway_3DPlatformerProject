using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    public GameObject pause;
    public Button pauseButton;
    public bool pauseButtonSelected;

    public GameObject dead;
    public Button deadButton;    
    public bool deadButtonSelected;

    public GameObject journal;
    public Button journalButton;
    public bool journalButtonSelected;

    public GameObject levelComplete;
    public Button levelCompleteButton;
    public bool levelCompleteButtonSelected;

    public GameObject dialogueBox;
    public Text dialogueText;

    void Update()
    {
        if (journal.activeInHierarchy == true && !journalButtonSelected)
        {
            EventSystem.current.SetSelectedGameObject(journalButton.gameObject);
            Time.timeScale = 0f;
            journalButtonSelected = true;
        }

        if (dead.activeInHierarchy == true && !deadButtonSelected)
        {
            EventSystem.current.SetSelectedGameObject(deadButton.gameObject);
            Time.timeScale = 0f;
            deadButtonSelected = true;
        }

        if (pause.activeInHierarchy == true && !pauseButtonSelected)
        {
            EventSystem.current.SetSelectedGameObject(pauseButton.gameObject);
            Time.timeScale = 0f;
            pauseButtonSelected = true;
        }

        if (levelComplete.activeInHierarchy == true && !levelCompleteButtonSelected)
        {
            EventSystem.current.SetSelectedGameObject(levelCompleteButton.gameObject);
            Time.timeScale = 0f;
            levelCompleteButtonSelected = true;
        }

        if (levelComplete.activeInHierarchy == false)
        {
            levelCompleteButtonSelected = false;
        }

        if (dead.activeInHierarchy == false)
        {
            deadButtonSelected = false;
        }

        if (pause.activeInHierarchy == false)
        {
            pauseButtonSelected = false;
        }

        if(journal.activeInHierarchy == false)
        {
            journalButtonSelected = false;
        }
    }

    public void Dialogue(string newspeech)
    {
        dialogueText.text = newspeech;
    }
}
