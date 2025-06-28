using UnityEngine;

public static class Define
{
    #region Path & Tag
    public const string PinkPongMusic = "Musics/PinkPong";
    public const string GameMusics = "Musics";
    public const string MasterMixerPath = "Sounds/MasterMixer";
    public const string ChoiceSoundPath = "Sounds/Choice";
    public const string BGM = "BGMVolume";
    public const string VFX = "VFXVolume";
    public const string GameControllerTag = "GameController";
    #endregion

    #region Scene
    public const string MainScene = "MainScene";
    public const string PrologueScene = "PrologueScene";
    public const string TutorialScene = "TutorialScene";
    public const string GameScene = "GameScene";
    public const string ChapterChoiceScene = "ChapterChoiceScene";
    public const string ResultScene = "ResultScene";
    #endregion

    #region Enum
    public enum JudgementType
    {
        Perfect300,
        Good100,
        Poor50,
        Miss
    }

    public enum TutorialType : int
    {
        Bubble,
        Disturbance,
        Life,
        Arrow,
        Percentage,
    }
    #endregion

    #region Constants
    public static readonly Vector3[] TutorialPosition =
    {
        new Vector3(300, 0, 0),     //Bubble
        new Vector3(0, 150, 0),     //Disturbance
        new Vector3(70, 70, 0),     //Life
        new Vector3(400, 400, 0),   //Arrow
        new Vector3(75, 415, 0),   //Percentage
    };
    public static readonly Vector3[] RandomScale =
    {
        Vector3.one * 0.8f,
        Vector3.one,
        Vector3.one * 1.2f,
    };
    public static readonly string Success = "성공";
    public static readonly string Fail = "실패";

    public const float MainPressAnyKeyImageBlinkTime = 1.5f;
    public const float PrologueFadeTime = 1f;
    public const float PrologueKeepTime = 2f;
    public const float TutorialPauseTime = 2.5f;
    #endregion

    #region Announcements
    public const string ChapterConfirm = "도전하시겠습니까?";
    public const string ChapterUndefined = "해당 챕터는 잠겨있어요...";
    #endregion
}
