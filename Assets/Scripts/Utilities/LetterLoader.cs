using UnityEngine;

public class LetterLoader : MonoBehaviour
{
    public static LetterDatabase Database { get; private set; }

    void Awake()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "letters.json");
        string json = System.IO.File.ReadAllText(path);
        Database = JsonUtility.FromJson<LetterDatabase>(json);
    }
}
