using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
public class Letter : MonoBehaviour
{
    [SerializeField] private GameObject briefPrefab;
    private int correctFilingIndex;
    private int actualFilingIndex;
    private TMP_Text letterContent;

    private Dictionary<int, bool> blackedMap; // index of word, if blacked or not
    private List<int> goodWordIndexes;
    private List<int> badWordIndexes;


    public int getCorrectFilingIndex()
    {
        return this.correctFilingIndex;
    }
    public int getActualFilingIndex()
    {
        return this.actualFilingIndex;
    }
    public TMP_Text getLetterContent()
    {
        return this.letterContent;
    }
    public Dictionary<int, bool> getBlackedMap()
    {
        return this.blackedMap;
    }
    public List<int> getGoodWordIndexes()
    {
        return this.goodWordIndexes;
    }
    public List<int> getBadWordIndexes()
    {
        return this.badWordIndexes;
    }
}
