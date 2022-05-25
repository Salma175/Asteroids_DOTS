using System;

public static class EventManager {

    public static Action<AudioClipType> PlayAudioEvent;

    public static Action<AudioClipType> StopAudioEvent;

    public static Action ResetAudioEvent;


    public static Action<bool> HandleGameUIEvent;
    public static Action<int> ScoreUpdateEvent;
}
