using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance = null;

    public static GameManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject("@Managers");
                    DontDestroyOnLoad(go);
                }

                s_instance = FindAnyObjectByType<GameManager>();
                if (s_instance == null)
                {
                    GameObject gameManager = new GameObject("GameManager");
                    GameManager comp = gameManager.AddComponent<GameManager>();
                    gameManager.transform.SetParent(go.transform);
                    s_instance = comp;
                }
                else
                {
                    s_instance.gameObject.transform.SetParent(go.transform);
                }
            }
            return s_instance;
        }
    }

    public Action OnScoreUpdate;
    public Action OnResultPanelActive;

    AudioClip[] _gameMusics;
    AudioClip _currentMusic;
    // ���ӿ��� �� ����� �� �� ���� �ʿ�
    int _currentMusicIndex;
    // ���� ����
    int _score;
    // ���� ���
    bool _isSuccess;
    // ���� ���� ����
    bool _isPlaying = true;

    #region Properties
    public AudioClip CurrentMusic
    {
        get { return _currentMusic; }
    }

    public AudioClip[] GameMusics
    {
        get { return _gameMusics; }
    }

    public int CurrentMusicIndex
    {
        get { return _currentMusicIndex; }
    }

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    public bool IsSuccess
    {
        get
        {
            return _isSuccess;
        }
        set
        {
            _isSuccess = value;
        }
    }

    public bool IsPlaying
    {
        get
        {
            return _isPlaying;
        }
        set
        {
            _isPlaying = value;
            if (!_isPlaying)
            {
                OnScoreUpdate?.Invoke();
            }
        }
    }
    #endregion


    void Start()
    {
        LoadMusics();
    }

    void LoadMusics()
    {
        if (_gameMusics == null)
        {
            _gameMusics = Resources.LoadAll<AudioClip>(Define.GameMusics);
        }
    }

    // chapter choice ȭ�鿡�� ���õ� ��ư�� �ε����� ���� ����
    // ����� ��, ������ ���õ� �ε����� ���� ����
    public void GameStart(int idx = -1)
    {
        if (idx >= 0)
        {
            _currentMusicIndex = idx;
            _currentMusic = _gameMusics[idx];
        }
        IsPlaying = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(Define.GameScene);
    }
}
