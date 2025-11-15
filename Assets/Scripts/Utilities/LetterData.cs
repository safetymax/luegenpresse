using System.Collections.Generic;


[System.Serializable]
public class LetterData
{
    public string text;
    public List<string> badWords;
    public int filingIndex;
}

[System.Serializable]
public class LetterDatabase
{
    public List<LetterData> letters;
}
