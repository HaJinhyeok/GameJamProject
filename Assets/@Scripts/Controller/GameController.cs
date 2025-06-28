using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MusicController musicController;

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject _disturbanceButtonPrefab;
    [SerializeField] AudioSource _bubblePopSound;
    [SerializeField] AudioSource _brokenSound;
    // UI가 위치한 캔버스의 RectTransform
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] float gameSpeed = 2f;

    const float disturbanceSpawnInterval = 5f;
    const float disturbanceButtonRadius = 100f;

    private string gameDataFileName;
    private int gameScore = 0;
    private float _timer;
    private bool _isFirstBubble = true;
    private bool _isFirstDisturbance = true;

    private SortedList<float, ButtonItem> gameButtons = new SortedList<float, ButtonItem>();

    public static Action<bool> OnPreferencePanelSet;

    public static Action OnCollision;

    void Start()
    {
        if (!LoadGameData())
        {
            UnityEngine.Debug.Log("Failed to Load Game Data!!!");
            return;
        }

        StartCoroutine(PlayMusicOnDelay(0));
        _timer = 0f;

        StartCoroutine(SpawnDisturbanceButtons());

        _isFirstBubble = true;
        _isFirstDisturbance = true;

        ButtonController.OnClicked += OnGameButtonClick;
        ButtonController.OnPlayerMissClicked += OnBrokenSoundPlay;
        GameManager.Instance.OnScoreUpdate += OnGameOver;
        OnPreferencePanelSet += MusicPause;
    }

    private IEnumerator SpawnDisturbanceButtons()
    {
        while (GameManager.Instance.IsPlaying)
        {
            yield return new WaitForSeconds(disturbanceSpawnInterval);
            Vector2 spawnPos;
            bool foundPos = false;
            int maxAttempts = 30;

            for (int i = 0; i < maxAttempts; i++)
            {
                spawnPos = GetRandomCanvasPosition();

                if (!IsOverlappingWithRealButtons(spawnPos))
                {
                    GameObject disturbance = Instantiate(_disturbanceButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    disturbance.transform.SetParent(transform, false);
                    ButtonController buttonController = disturbance.GetComponent<ButtonController>();

                    buttonController.duration = gameSpeed;
                    buttonController.InitializeButton(_timer * 1000, spawnPos[0], spawnPos[1]);

                    if (TutorialController.IsTutorial && _isFirstDisturbance)
                    {
                        TutorialController.OnTutorial?.Invoke(Define.TutorialType.Disturbance, buttonController.transform.localPosition);
                        _isFirstDisturbance = false;
                    }
                    foundPos = true;
                    break;
                }
            }

            if (!foundPos)
            {
                Debug.LogWarning("Disturbance 위치 찾지 못함");
            }
        }
    }

    private bool IsOverlappingWithRealButtons(Vector2 newPos)
    {
        var buttons = FindObjectsByType<ButtonController>(FindObjectsSortMode.None);

        foreach (var button in buttons)
        {
            if (button is DisturbanceButtonController) continue;

            Vector2 existingPos = ((RectTransform)button.transform).anchoredPosition;
            if (Vector2.Distance(existingPos, newPos) < disturbanceButtonRadius * 2f)
            {
                return true; // 너무 가까움
            }
        }
        return false;
    }

    private Vector2 GetRandomCanvasPosition()
    {
        float width = canvasRectTransform.rect.width;
        float height = canvasRectTransform.rect.height;

        float x = UnityEngine.Random.Range(disturbanceButtonRadius, width - disturbanceButtonRadius);
        float y = UnityEngine.Random.Range(disturbanceButtonRadius, height - disturbanceButtonRadius);

        return new Vector2(x, y);
    }

    private IEnumerator PlayMusicOnDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        musicController.PlayAudio();
    }

    private IEnumerator FadeOutMusic(float seconds)
    {
        float startVol = musicController.AudioSource.volume;

        while (musicController.AudioSource.volume > 0)
        {
            musicController.AudioSource.volume -= startVol * (Time.deltaTime / seconds);
            yield return null;
        }
        musicController.AudioSource.Stop();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (gameButtons.Count > 0 && _timer * 1000 > gameButtons.Keys[0] && GameManager.Instance.IsPlaying)
        {
            float keyTime = gameButtons.Keys[0];

            CreateButton(_timer * 1000, gameButtons[keyTime].position);

            gameButtons.Remove(keyTime);
        }
        else if (gameButtons.Count == 0)
        {
            StartCoroutine(FadeOutMusic(200f));
        }
    }

    private bool LoadGameData()
    {
        gameDataFileName = GameManager.Instance.CurrentMusic.name.Substring(1) + ".json";
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            ButtonData buttonData = JsonUtility.FromJson<ButtonData>(dataAsJson);

            for (int i = 0; i < buttonData.buttons.Count; ++i)
            {
                gameButtons.Add(buttonData.buttons[i].time, buttonData.buttons[i]);
            }

            return true;
        }

        return false;
    }

    public void CreateButton(float startTime, float[] startPos)
    {
        GameObject button = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        button.transform.SetParent(transform, false);
        ButtonController buttonController = button.GetComponent<ButtonController>();

        buttonController.duration = gameSpeed;
        buttonController.InitializeButton(startTime, startPos[0], startPos[1]);

        if (TutorialController.IsTutorial && _isFirstBubble)
        {
            TutorialController.OnTutorial?.Invoke(Define.TutorialType.Bubble, buttonController.transform.localPosition);
            _isFirstBubble = false;
        }
    }

    public void OnGameButtonClick(ButtonController button)
    {
        _bubblePopSound.Play();
        gameScore += (int)button.buttonScore;
        UpdateScoreLabel(gameScore);
    }

    public void OnBrokenSoundPlay()
    {
        _brokenSound.Play();
    }

    private void UpdateScoreLabel(int scoreValue)
    {
        scoreLabel.text = scoreValue.ToString();
    }

    private void OnGameOver()
    {
        musicController.StopAudio();
        GameManager.Instance.Score = gameScore;
        GameManager.Instance.OnResultPanelActive?.Invoke();
    }

    void MusicPause(bool flag)
    {
        if (flag)
        {
            musicController.AudioSource.Pause();
        }
        else
        {
            musicController.AudioSource.Play();
        }
    }

    private void OnDestroy()
    {
        ButtonController.OnClicked -= OnGameButtonClick;
        ButtonController.OnPlayerMissClicked -= OnBrokenSoundPlay;
        OnPreferencePanelSet -= MusicPause;
        if (Application.isPlaying)
        {
            GameManager.Instance.OnScoreUpdate -= OnGameOver;
        }
    }
}
