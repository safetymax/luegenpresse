using System.Collections.Generic;


[System.Serializable]
public class LetterData
{
    public string text;
    public List<int> goodWordIndexes;
    public List<int> badWordIndexes;
    public int filingIndex;
}

[System.Serializable]
public class LetterDatabase
{
    public List<LetterData> letters;
}
