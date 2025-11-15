using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Letter 
{
    private int correctFilingIndex;
    private int actualFilingIndex;
    private string letterContent;

    private List<string> badWords;

    [SerializeField] private int filingMalus;
    [SerializeField] private int blackingMalus;

    public Letter(LetterData data)
    {
        this.letterContent = data.text;

        this.badWords = new List<string>(data.badWords);

        this.correctFilingIndex = data.filingIndex;
    }

    public int getCorrectFilingIndex()
    {
        return this.correctFilingIndex;
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
        //check if filed correctly
        if (this.actualFilingIndex != this.correctFilingIndex)
        {
            //else return malus
            return filingMalus;
        }

        int wrongAmountBlacked = 0;
        foreach (String badWord in this.badWords)
        {
            wrongAmountBlacked += Regex.Matches(letterContent, badWord).Count; // wie oft das schlechte noch drinnen ist
        }

        wrongAmountBlacked += Regex.Matches(letterContent, "@").Count; // frage: was soll das hier zaehlen?

        return wrongAmountBlacked * blackingMalus;
    }
    public string toString()
    {
        return letterContent + ", Correct Filing Index: " + correctFilingIndex + ", Badwords list: " + badWords;
    }
}

