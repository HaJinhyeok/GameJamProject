using UnityEngine;

public static class Define
{
    #region Path
    public const string PinkPongMusic = "Musics/PinkPong";
    public const string GameMusics = "Musics";
    public const string BGM = "BGMVolume";
    public const string VFX = "VFXVolume";
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
    #endregion

    #region Constants
    public static readonly Vector3[] RandomScale = { Vector3.one * 0.8f, Vector3.one, Vector3.one * 1.2f };
    public static readonly string Success = "����";
    public static readonly string Fail = "����";

    public const float PrologueFadeTime = 1f;
    public const float PrologueKeepTime = 2f;
    #endregion

    #region Announcements
    public const string ChapterConfirm = "�����Ͻðڽ��ϱ�?";
    public const string ChapterUndefined = "�ش� é�ʹ� ����־��...";
    #endregion
}
