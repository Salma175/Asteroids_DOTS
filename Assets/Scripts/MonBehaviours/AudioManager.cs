using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public struct AudioObject
    {
        public AudioClipType type;
        public AudioSource source;
    }

    [SerializeField]
    private AudioObject[] audioObjects;

    private void Start()
    {
        EventManager.PlayAudioEvent += PlaySound;
        EventManager.StopAudioEvent += StopSound;
        EventManager.ResetAudioEvent += ResetAudios;
    }

    public void PlaySound(AudioClipType type)
    {
        if (type != AudioClipType.None)
        {
            AudioSource source = GetAudioClip(type);
            source.Play();
        }
    }

    public void StopSound(AudioClipType type)
    {
        if (type != AudioClipType.None)
        {
            AudioSource source = GetAudioClip(type);
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    private AudioSource GetAudioClip(AudioClipType type)
    {
        foreach (var item in audioObjects)
        {
            if (item.type == type) return item.source;
        }
        return null;
    }

    private void ResetAudios() {
        foreach (var item in audioObjects)
        {
            if (item.source.isPlaying) item.source.Stop();
        }
    }


    private void OnDestroy()
    {
        EventManager.PlayAudioEvent -= PlaySound;
        EventManager.StopAudioEvent -= StopSound;
        EventManager.ResetAudioEvent -= ResetAudios;
    }
}
