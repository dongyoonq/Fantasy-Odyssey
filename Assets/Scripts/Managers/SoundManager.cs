using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<Sound> musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Start()
    {
        musicSounds = new List<Sound>()
        {
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/TitleSceneBgm"), "Title"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/TownBgm"), "Town"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/CastleBgm"), "Castle"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/DungeonBgm"), "Dungeon"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/BossBgm"), "Boss"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/BossBattleBgm"), "BossBattle"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/DieBgm"), "Die"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/VictoryBgm"), "Victory"),
        };

        sfxSounds = new List<Sound>
        {
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/ItemGain"), "ItemGain"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword7"), "NormalSword1"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword5"), "NormalSword2"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword4"), "NormalSword3"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword5"), "NormalSword4"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword8"), "NormalSword5"),
        };

        GameObject _musicSource = new GameObject();
        _musicSource.AddComponent<AudioSource>();
        _musicSource.name = "Music Source";
        _musicSource.transform.parent = transform;
        musicSource = _musicSource.GetComponent<AudioSource>();

        GameObject _sfxSource = new GameObject();
        _sfxSource.AddComponent<AudioSource>();
        _sfxSource.name = "SFX Source";
        _sfxSource.transform.parent = transform;
        sfxSource = _sfxSource.GetComponent<AudioSource>();

        PlayMusic("Title");
        musicSource.loop = true;
    }

    public void PlayMusic(string name)
    {
        Sound sound = musicSounds.Find(x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = sfxSounds.Find(x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    Sound CreateSound(AudioClip clip, string name)
    {
        Sound sound = new Sound();
        sound.clip = clip;
        sound.name = name;
        return sound;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}