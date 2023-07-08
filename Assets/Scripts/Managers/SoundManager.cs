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
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/Title2"), "Title2"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/Title3"), "Title3"),
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
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/ItemGain"), "ItemGain"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/Potion 01"), "Potion 01"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/Potion 02"), "Potion 02"),

            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/UI/OpenUI"), "OpenUI"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/UI/ButtonClick"), "Click"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/PortalSound"), "Portal"),

            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/LevelUp"), "LevelUp"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword7"), "NormalSword1"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword5"), "NormalSword2"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword4"), "NormalSword3"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword5"), "NormalSword4"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword8"), "NormalSword5"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Player/Sword/Sword9"), "NormalSword6"),

            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/ScareCrowHit"), "ScareCrowHit"),

            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/PhantomDie"), "PhantomDie"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/PhantomSpell"), "PhantomSpell"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/PhantomSpellAttack"), "PhantomSpellAttack"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/PhantomLeftAttack"), "PhantomLeft"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/PhantomRightAttack"), "PhantomRight"),

            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/SpiderDie"), "SpiderDie"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/SpiderAttack"), "SpiderAttack"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/SpiderProjectileAttack"), "SpiderProjectile"),

            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/BeeDie"), "BeeDie"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/BeeAttack"), "BeeAttack"),

            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Monster/MonsterHit"), "MonsterHit"),

            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Earthquake 1"), "Earthquake 1"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Laugh"), "Laugh"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Claw1"), "Claw1"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Claw2"), "Claw2"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Dead"), "BossDead"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/BossHit"), "BossHit"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Rage"), "Rage"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Heal"), "Heal"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Jump"), "BossJump"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Dead"), "BossDead"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/BossHit"), "BossHit"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Smash"), "Smash"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Summon"), "Summon"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Throw"), "Throw"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Grab"), "Grab"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Ground Crush"), "Ground Crush"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Breath"), "Breath"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/Groggy"), "Groggy"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Boss/SummonExplosion"), "Explosion"),
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

        PlayMusic("Title2");
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