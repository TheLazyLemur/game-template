﻿using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton

    private static AudioManager _instance;

    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    _instance =
                        new GameObject("Spawned audio manager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }

            return _instance;
        }
        private set { _instance = value; }
    }

    #endregion

    private AudioSource _musicSource;
    private AudioSource _musicSource2;
    private AudioSource _sfxSource;

    private bool _firstMusicSourceIsPlaying;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource2 = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();

        _musicSource.loop = true;
        _musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip clipToPlay)
    {
        var activeSource = (_firstMusicSourceIsPlaying) ? _musicSource : _musicSource2;

        activeSource.clip = clipToPlay;
        activeSource.volume = 1;
        activeSource.Play();
    }

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1)
    {
        var activeSource = (_firstMusicSourceIsPlaying) ? _musicSource : _musicSource2;
        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }

    public void PlayMusicWithCrossFade(AudioClip clip, float transitionTime)
    {
        var activeSource = (_firstMusicSourceIsPlaying) ? _musicSource : _musicSource2;
        var newSource = (_firstMusicSourceIsPlaying) ? _musicSource2 : _musicSource;

        _firstMusicSourceIsPlaying = !_firstMusicSourceIsPlaying;

        newSource.clip = clip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithFade(activeSource, newSource, transitionTime));
    }

    private IEnumerator UpdateMusicWithFade(AudioSource originalSource, AudioSource newSource, float transitionTime)
    {
        var t = 0f;

        for (t = 0; t < transitionTime; t+= Time.deltaTime)
        {
            originalSource.volume = (1 - (t / transitionTime));
            newSource.volume = (t / transitionTime);
            yield return null;
        }
        
        originalSource.Stop();
    }

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        if (!activeSource.isPlaying)
            activeSource.Play();

        var t = 0f;

        //Fade out
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (1 - (t / transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        //Fade in
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = t / transitionTime;
            yield return null;
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
    public void PlaySfx(AudioClip clip, float vol)
    {
        _sfxSource.PlayOneShot(clip, vol);
    }
}