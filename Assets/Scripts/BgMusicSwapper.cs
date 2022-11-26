using System.Collections;
using UnityEngine;

public class BgMusicSwapper : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip[] audioClips;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayAudioSequentially());
    }

    private IEnumerator PlayAudioSequentially()
    {
        yield return null;

        foreach (AudioClip t in audioClips)
        {
            _audioSource.clip = t;

            _audioSource.Play();

            while (_audioSource.isPlaying)
            {
                yield return null;
            }
        }

        StartCoroutine(PlayAudioSequentially());
    }
}