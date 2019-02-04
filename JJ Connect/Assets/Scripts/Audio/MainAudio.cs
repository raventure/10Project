using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainAudio : MonoBehaviour
{
    static MainAudio main;
    public InputAudio[] listinputAudio;

    AudioSource[] sound;
    Dictionary<TypeAudio, AudioSource> audioDict = new Dictionary<TypeAudio, AudioSource>();
    public bool isMute;
    int bgIndex;

    private void Awake()
    {
        AddComponienAudioSources();
        main = this;
    }

    private void Start()
    {
        PlayBGSound();
    }

    public static MainAudio Main
    {
        get { return main; }
    }
    void AddComponienAudioSources()
    {
        sound = new AudioSource[listinputAudio.Length];
        for(int i = 0; i < listinputAudio.Length; i++)
        {
            AudioSource thisAudio = gameObject.AddComponent<AudioSource>();
            sound[i] = thisAudio;
            thisAudio.playOnAwake = false;
            thisAudio.clip = listinputAudio[i].audioClip;
            audioDict.Add(listinputAudio[i].type, thisAudio);
        }
    }
    public void StopBGSound()
    {
        audioDict[TypeAudio.SoundBG].Stop();
    }
    public void PlayBGSound()
    {
        PlaySound(TypeAudio.SoundBG);
        audioDict[TypeAudio.SoundBG].volume = 0.1f;
        audioDict[TypeAudio.SoundBG].loop = true; // 반복 재생

    }
    public void PlaySound(TypeAudio type)
    {
        if(!isMute)
        {
            audioDict[type].Play();
        }
    }
    public void MuteSound(bool bol)
    {
        isMute = !bol;
        if(isMute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

}
