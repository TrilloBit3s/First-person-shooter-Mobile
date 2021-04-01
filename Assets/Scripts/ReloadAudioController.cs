using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAudioController : MonoBehaviour
{
    public AudioSource GunAtualAudio;
    public AudioClip sound1;
    public AudioClip sound2; 
    public AudioClip sound3;
  
    void SoundPrimeiro()
    {
        GunAtualAudio.clip = sound1;
        GunAtualAudio.Play();
    }

    void SoundSegundo()
    {
        GunAtualAudio.clip = sound2;
        GunAtualAudio.Play();
    }

    void SoundTerceiro()
    {
        GunAtualAudio.clip = sound3;
        GunAtualAudio.Play();
    }
}