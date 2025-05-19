using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private SoundObject BGMAudioObject;
    private float current, percent;

    private AudioClip BGM;
    private SoundObject playObject;

    private bool soundOnOFF = true;

    public Pool soundPool;

    public void Init()
    {
        TryGetComponent<Pool>(out soundPool);

        if (BGM)
        {
            if (!BGMAudioObject)
                ChangeBGM(BGM);
            else
                if(BGM != BGMAudioObject.AudioSource.clip)
                    ChangeBGM(BGM);
        } 
    }

    public void SoundOnOFF(bool tf)
    {
        soundOnOFF = tf;
        PlayerPrefs.SetInt("Sound", tf ? 1 : 0);
        PlayerPrefs.Save();

        var pool = soundPool;
        SoundObject audio;
        for (int i = 0; i < pool.transform.childCount; i++)
        {
            if(pool.transform.GetChild(i).TryGetComponent<SoundObject>(out audio))
            {
                if(soundOnOFF)
                    audio.SetVolume(1f);
                else
                    audio.SetVolume(0f);
            }
        }
    }

    public void ChangeBGM(AudioClip audioClip)
    {
        if (!BGMAudioObject)
        {
            BGMAudioObject = PlaySound(audioClip, true);
            BGMAudioObject.AudioSource.loop = true;
            BGMAudioObject.name = "BGM Object";
        }
        else
            StartCoroutine("ChangeBGMClip", audioClip);
    }

    public void StopSound()
    {
        if (playObject == null) return;

        if (playObject.gameObject.activeSelf)
            playObject.Stop();
    }

    public SoundObject PlaySound(AudioClip audioClip, bool imortal = false)
    {
        if (soundPool.GetPoolObject().TryGetComponent<SoundObject>(out SoundObject soundObject))
        {
            if(!imortal)
                playObject = soundObject;
            soundObject.Init(audioClip, imortal);
            if (soundOnOFF)
                soundObject.SetVolume(1f);
            else
                soundObject.SetVolume(0f);
            return soundObject;
        }

        return null;
    }

    IEnumerator ChangeBGMClip(AudioClip newClip)
    {
        current = percent = 0f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            BGMAudioObject.AudioSource.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null;
        }

        BGMAudioObject.AudioSource.clip = newClip;
        BGMAudioObject.AudioSource.Play();
        current = percent = 0f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            BGMAudioObject.AudioSource.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }

    }
}
