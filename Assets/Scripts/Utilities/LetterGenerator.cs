using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// this class generates letters and gives a list of them
// TODO: Add functionality for the days, topics, etc.
public class LetterGenerator : MonoBehaviour
{
    public static LetterGenerator Instance { get; private set;}

    public List<Letter> generateLetters(int letterCount, List<string> dailyTags)
    {
        System.Random rnd = new System.Random();

        var filtered = LetterLoader.Database.letters
            .Where(ld => ld.tags.Any(tag => dailyTags.Contains(tag)))
            .ToList();

        letterCount = Mathf.Min(letterCount, filtered.Count);

        var selected = filtered
            .OrderBy(_ => rnd.Next())
            .Take(letterCount)
            .ToList();

        List<Letter> letterList = new List<Letter>();
        foreach (var data in selected)
            letterList.Add(new Letter(data));

        return letterList;
    }

    private void Awake() {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
