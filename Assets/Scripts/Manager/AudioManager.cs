using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingleTon<AudioManager>
{
    //public static AudioManager instance;
    
    [Header("#BGM")]
    public List<AudioClip> bgmClips = new List<AudioClip>();
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public List<AudioClip> sfxClip = new List<AudioClip>();
    public float sfxVolume;
    public int channels = 16;
    AudioSource[] sfxPlayers;
    int channelIndex;// 현재 

    public enum Type { BGM, SFX}
    public enum Sfx {Click, Dead, Hit, LevelUp, Lose, Select, Win}

    private void OnEnable()
    {
        Debug.Log("enable");
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume = 0;//UserData.bgmVolume = 0;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume = 0;//UserData.soundVolume = 0;
        }
    }

    public void LoadSound(Type type, string soundName)
    {
        AudioClip soundClip =  Resources.Load<AudioClip>("Sound/" + soundName);
        if(soundClip != null)
        {
            if(type == Type.BGM)
            {
                if (bgmClips.Count == 0)
                    bgmClips.Add(soundClip);
                else
                {
                    for (int i = 0; i < bgmClips.Count; i++)
                    {
                        if (soundClip == bgmClips[i])
                            break;
                        //if (i == bgmClips.Count)
                        bgmClips.Add(soundClip);
                    }
                }
            }
            else if(type == Type.SFX)
            {
                if (sfxClip.Count == 0)
                {
                    sfxClip.Add(soundClip);
                }
                else
                {
                    for (int i = 0; i < sfxClip.Count; i++)
                    {
                        if (soundClip == sfxClip[i])
                            break;
                        //if (i == sfxClip.Count)
                        sfxClip.Add(soundClip);
                    }
                }
            }
            
        }
    }

    public void PlayBgm(bool isPlay, string clipName)
    {
        if (isPlay)
        {
            for(int i = 0; i < bgmClips.Count; i++)
            {
                if(bgmClips[i].name == clipName)
                {
                    bgmPlayer.clip = bgmClips[i];
                    bgmPlayer.Play();
                }
            }
        }
        else
            bgmPlayer.Stop();
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx, string clipName)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            for (int j = 0; j < sfxClip.Count; j++)
            {
                if (clipName == sfxClip[j].name)
                {
                    if (sfxPlayers[channelIndex].clip != sfxClip[j])
                        sfxPlayers[channelIndex].clip = sfxClip[j];

                    sfxPlayers[channelIndex].Play();
                    break;
                }
            }
            //sfxPlayers[channelIndex].clip = sfxClip[(int)sfx];
            //sfxPlayers[channelIndex].Play();
            break;
        }
    }
    public void BGMVolume(float value)
    {
        UserData.bgmVolume = value;
        bgmPlayer.volume = value;
        Debug.Log("bgmset");
    }
    public void SoundVolume(float value)
    {
        UserData.soundVolume = value;
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = value;
        }
    }
}
