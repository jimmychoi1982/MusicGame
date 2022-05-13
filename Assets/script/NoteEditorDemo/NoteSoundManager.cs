using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSoundManager : SingletonMonoBehaviour<NoteSoundManager>
{
    public AudioSource BGM;
    public AudioSource SE;
    public AudioClip SEClip;

    public void PlaySE()
    {
        SE.PlayOneShot(SEClip);
    }

    public void PlayBGM()
    {
        BGM.Play();
    }
}
