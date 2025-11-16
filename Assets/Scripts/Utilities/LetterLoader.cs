using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class LetterLoader : MonoBehaviour
{
    public static LetterLoader Instance { get; private set; }
    public static LetterDatabase Database { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL → must use async web request
        StartCoroutine(LoadJsonWebGL());
#else
        // Editor / Standalone → can use normal file IO
        LoadJsonLocal();
#endif
    }

    private void LoadJsonLocal()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "letters.json");

        if (!File.Exists(path))
        {
            Debug.LogError("letters.json not found at: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        Database = JsonUtility.FromJson<LetterDatabase>(json);

        Debug.Log("Loaded letters.json (Local/Desktop)");
    }

    private IEnumerator LoadJsonWebGL()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "letters.json");

        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load letters.json (WebGL): " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        Database = JsonUtility.FromJson<LetterDatabase>(json);

        Debug.Log("Loaded letters.json (WebGL)");
    }
}
