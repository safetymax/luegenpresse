using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Letter : MonoBehaviour
{
    [SerializeField] private GameObject briefPrefab;
    private int correctFilingIndex;
    private int actualFilingIndex;
    private TMP_Text letterContent;

    private Dictionary<int, bool> blackedMap; // index of word, if blacked or not
    //private List<int> goodWordIndexes;
    private List<int> badWordIndexes;

    [SerializeField] private int filingMalus;
    [SerializeField] private int blackingMalus;

    public Letter(LetterData data)
    {
        this.letterContent.text = data.text;
        //this.goodWordIndexes = new List<int>(data.goodWordIndexes);
        this.badWordIndexes = new List<int>(data.badWordIndexes);
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
    public TMP_Text getLetterContent()
    {
        return this.letterContent;
    }
    public Dictionary<int, bool> getBlackedMap()
    {
        return this.blackedMap;
    }
    /*
    public List<int> getGoodWordIndexes()
    {
        return this.goodWordIndexes;
    }
    */
    public List<int> getBadWordIndexes()
    {
        return this.badWordIndexes;
    }

    public int Evaluate(List<String> badWords)
    {
        //check if filed correctly
        if (this.actualFilingIndex != this.correctFilingIndex)
        {
            //else return malus
            return filingMalus;
        }

        int wrongAmountBlacked = 0;
        foreach (String badWord in badWords)
        {
            wrongAmountBlacked += Regex.Matches(letterContent.text, badWord).Count;
        }

        wrongAmountBlacked += Regex.Matches(letterContent.text, "@").Count;

        return wrongAmountBlacked * blackingMalus;
    }
}
