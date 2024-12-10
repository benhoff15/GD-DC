using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip mainSceneMusic;
    public AudioClip combatMusicEnemy1;
    public AudioClip combatMusicEnemy2;
    public AudioClip combatMusicWizard1;
    public AudioClip combatMusicWizard2;
    void Start()
    {
        PlayMainSceneMusic();
    }
    public void PlayMainSceneMusic()
    {
        PlayMusic(mainSceneMusic);
    }
    public void PlayCombatMusicEnemy1()
    {
        PlayMusic(combatMusicEnemy1);
    }

    public void PlayCombatMusicEnemy2()
    {
        PlayMusic(combatMusicEnemy2);
    }
    public void PlayCombatMusicWizard1()
    {
        PlayMusic(combatMusicWizard1);
    }
    public void PlayCombatMusicWizard2()
    {
        PlayMusic(combatMusicWizard2);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}