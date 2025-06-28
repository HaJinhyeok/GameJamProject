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
    // 게임오버 후 재시작 시 곡 정보 필요
    int _currentMusicIndex;
    // 최종 점수
    int _score;
    // 현재 해금된 최대 챕터(0-index)
    int _maxChapter = 0;
    // 게임 결과
    bool _isSuccess;
    // 게임 진행 여부
    bool _isPlaying = true;
    // 튜토리얼 클리어 여부
    bool _isTutorialClear = false;

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

    public int MaxChapter
    {
        get
        {
            return _maxChapter;
        }
        set
        {
            _maxChapter = value;
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
            if (_isSuccess)
            {
                // 현재 클리어한 음악 번호가 최대 챕터와 같으면
                if (_currentMusicIndex == _maxChapter && _currentMusicIndex < _gameMusics.Length)
                {
                    _maxChapter++;
                }
            }
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

    public float Percentage { get; set; }

    public bool IsTutorialClear
    {
        get
        {
            return _isTutorialClear;
        }
        set
        {
            _isTutorialClear = value;
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

    // chapter choice 화면에서 선택된 버튼의 인덱스로 게임 시작
    // 재시작 시, 기존에 선택된 인덱스로 게임 시작
    public void GameStart(int idx = -1)
    {
        if (idx >= 0)
        {
            _currentMusicIndex = idx;
            _currentMusic = _gameMusics[idx];
        }
        IsPlaying = true;
        Time.timeScale = 1f;
        if (_currentMusicIndex == 0)
        {
            SceneManager.LoadScene(Define.TutorialScene);
        }
        else
        {
            SceneManager.LoadScene(Define.GameScene);
        }
    }
}
