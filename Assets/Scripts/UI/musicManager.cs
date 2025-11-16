using UnityEngine;

public class musicManager : MonoBehaviour
{
    public AudioClip menu;
    private AudioSource audioSource;

void Start()
{
    audioSource = GetComponent<AudioSource>();
    audioSource.clip = menu;
    audioSource.loop = true;
    audioSource.Play();
}
}
