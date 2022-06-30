using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public readonly string BackGround_Path = "Audio/Background";
    public readonly string Effect_Path = "Audio/Effect";
    public readonly string UI_Path = "Audio/UI";

    Dictionary<string, AudioClip> Background_Clips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> Effect_Clips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> UI_Clips = new Dictionary<string, AudioClip>();

    AudioSource[] BackgroundAudio;
    int curBGIdx = 0;
    int nextBGIdx = 1;
    public float changeDuration = 0.25f;

    private void Awake()
    {
        Init();
        BackgroundAudio = GetComponents<AudioSource>();
    }

    void Init()
    {
        foreach (var item in Resources.LoadAll<AudioClip>(BackGround_Path))
        {
            Background_Clips.Add(item.name, item);
            Debug.Log(item.name);
        }

        foreach (var item in Resources.LoadAll<AudioClip>(Effect_Path))
        {
            Effect_Clips.Add(item.name, item);
            Debug.Log(item.name);
        }

        foreach (var item in Resources.LoadAll<AudioClip>(UI_Path))
        {
            UI_Clips.Add(item.name, item);
            Debug.Log(item.name);
        }
    }

    public void PlayBackground(string key)
    {
        StartCoroutine(ChangeBackground(changeDuration, key));
    }

    IEnumerator ChangeBackground(float duration, string key)
    {
        yield return null;

        float timer = duration;
        BackgroundAudio[nextBGIdx].clip = Background_Clips[key];
        BackgroundAudio[nextBGIdx].Play();

        if (BackgroundAudio[curBGIdx].clip != null)
            do
            {
                BackgroundAudio[curBGIdx].volume = timer / duration;
                BackgroundAudio[nextBGIdx].volume = (1 - timer) / duration;
                timer -= Time.deltaTime;
                yield return null;
            } while (timer >= 0f);


        int temp = curBGIdx;
        curBGIdx = nextBGIdx;
        nextBGIdx = temp;
    }
}