using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MidDayDialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogue;
    
    StringBuilder sb;
    private void Awake()
    {
        sb = new StringBuilder();
        switch (GameManager.Instance.ending)
        {
            case -1:
                sb.Append("Keep going like that. You will not be excepted if you make a single mistake!");
                break;
            case 0:
                sb.Append("YOU WORK LIKE A PEASANT! GET THE HELL OUT OF MY OFFICE!");
                break;
            case 1:
                sb.Append("Well done! You impressed me. I got a joboffer for you. follow me. NOW!");
                break;
        }
        dialogue.text = sb.ToString();
    }

    public void Acknowledge()
    {
        switch (GameManager.Instance.ending)
        {
            case -1:
                SceneManager.LoadScene("InstructionScene");
                break;
            default:
                Application.Quit();
                break;
                
        }
    }

    public void ChangeSceneToEditor()
    {
        // TODO: go back to Main scene and change day in editor somehow
    }
}
