using System;

public static class EventManager {

    #region AUDIO
    public static Action<AudioClipType> PlayAudioEvent;

    public static Action<AudioClipType> StopAudioEvent;

    public static Action ResetAudioEvent;
    #endregion

    #region UI
    public static Action<bool> HandleGameUIEvent;

    public static Action<int> ScoreUpdateEvent;
    #endregion
}
