using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Letter 
{
    private int actualFilingIndex;
    protected string letterContent;
    protected List<string> tags;
    [SerializeField] private int filingMalus;

    public Letter(LetterData data)
    {
        if (data != null){
            this.letterContent = data.text;
            this.tags = data.tags;
        }
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

    public List<string> getTags()
    {
        return this.tags;
    }
    public int Evaluate()
    {
        int wrongAmountBlacked = 0;
        foreach (String badWord in GameManager.Instance.badWordsGlobal)
        {
            wrongAmountBlacked += Regex.Matches(letterContent, badWord).Count; // wie oft das schlechte noch drinnen ist
        }

        wrongAmountBlacked += Regex.Matches(letterContent, "€").Count; //words that were not bad but blacked by user anyways

        //evaluate filing in combination with blacking score!
        return GameManager.Instance.evaluateFilingOfLetter(this, wrongAmountBlacked);
    }
    public string toString()
    {
        return letterContent;
    }
}

public class ResultLetter : Letter
{
    private int wordsWronglyBlackedOrMissed;
    private int wrongFilingCount;

    public ResultLetter(int wordsWronglyBlackedOrMissed, int wrongFilingCount)
        : base(new LetterData { text = "", tags = new List<string>() })
    {
        this.wordsWronglyBlackedOrMissed = wordsWronglyBlackedOrMissed;
        this.wrongFilingCount = wrongFilingCount;
        this.letterContent =
            $"<b>Work Day Summary</b>\n\n" +
            $"Wrongly blacked / missed words: {wordsWronglyBlackedOrMissed}\n" +
            $"Incorrectly filed letters: {wrongFilingCount}\n" +
            $"Current Total score: {GameManager.Instance.getTotalScore()}\n\n";

        //ResultLetter has no tags
        this.tags = new List<string>();
    }
}


