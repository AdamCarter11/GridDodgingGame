using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] musicSections;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().loop = true;
        StartCoroutine(playSong());
    }

    IEnumerator playSong()
    {
        musicSource.clip = musicSections[0];
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.clip = musicSections[1];
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.clip = musicSections[2];
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.clip = musicSections[3];
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.clip = musicSections[2];
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.clip = musicSections[3];
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
    }
}
