using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Letter 
{
    private int actualFilingIndex;
    private string letterContent;
    private List<string> tags;
    [SerializeField] private int filingMalus;
    [SerializeField] private int blackingMalus;

    public Letter(LetterData data)
    {
        this.letterContent = data.text;
        this.tags = data.tags;
    }

    public int getActualFilingIndex()
    {
        return this.actualFilingIndex;
    }
    public void setActualFilingIndex(int filingIndex)
    {
        this.actualFilingIndex = filingIndex;
    }
    public string getLetterContent()
    {
        return this.letterContent;
    }


    public int Evaluate()
    {
        //TODO: check if files correctly
        if (false)
        {
            //else return malus
            return filingMalus;
        }

        int wrongAmountBlacked = 0;
        foreach (String badWord in GameManager.Instance.badWordsGlobal)
        {
            wrongAmountBlacked += Regex.Matches(letterContent, badWord).Count; // wie oft das schlechte noch drinnen ist
        }

        wrongAmountBlacked += Regex.Matches(letterContent, "€").Count; 

        return wrongAmountBlacked * blackingMalus;
    }
    public string toString()
    {
        return letterContent;
    }
}

