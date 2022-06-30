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

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        foreach(var item in Resources.LoadAll<AudioClip>(BackGround_Path))
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



}