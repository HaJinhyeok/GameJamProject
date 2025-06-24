using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject _disturbanceButtonPrefab;

    public MusicController musicController;

    public GameObject buttonPrefab;
    public Text scoreLabel;
    public float gameSpeed;

    private string gameDataFileName;
    private int gameScore = 0;
    private int roundedButtonCount;
    public Stopwatch gameTimer = new Stopwatch();

    private SortedList<float, ButtonItem> gameButtons = new SortedList<float, ButtonItem>();

    void Start()
    {
        if (!LoadGameData())
        {
            UnityEngine.Debug.Log("Failed to Load Game Data!!!");
            return;
        }

        StartCoroutine(PlayMusicOnDelay(2f));
        gameTimer.Start();

        ButtonController.OnClicked += OnGameButtonClick;
        GameManager.Instance.OnScoreUpdate += OnGameOver;

        roundedButtonCount = ButtonCountInitializer();
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
        if (gameButtons.Count > 0 && gameTimer.ElapsedMilliseconds > gameButtons.Keys[0] && GameManager.Instance.IsPlaying)
        {
            int buttonNum = 4 - System.Math.Abs(roundedButtonCount) % 4;
            float keyTime = gameButtons.Keys[0];

            CreateButton(gameTimer.ElapsedMilliseconds, gameButtons[keyTime].position, gameButtons[keyTime].endPosition, buttonNum);

            if (gameButtons[keyTime].isDrag)
            {
                roundedButtonCount--;
            }

            gameButtons.Remove(keyTime);
            roundedButtonCount--;
        }
        else if (gameButtons.Count == 0)
        {
            StartCoroutine(FadeOutMusic(200f));
        }
    }

    private bool LoadGameData()
    {
        //gameDataFileName = GameManager.Instance.CurrentMusic.name.Substring(1) + ".json";
        gameDataFileName = "babyshark.json";
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

    public void CreateButton(float startTime, float[] startPos, float[] endPos, int buttonNum)
    {
        GameObject button = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        button.transform.SetParent(GameObject.FindGameObjectWithTag("GameController").transform, false);
        ButtonController buttonController = button.GetComponent<ButtonController>();

        buttonController.startButtonText.text = buttonNum.ToString();

        buttonController.duration = gameSpeed;
        buttonController.InitializeButton(startTime, startPos[0], startPos[1], endPos[0], endPos[1]);
    }

    public void OnGameButtonClick(ButtonController button)
    {
        gameScore += (int)button.buttonScore;
        UpdateScoreLabel(gameScore);
    }

    private void UpdateScoreLabel(int scoreValue)
    {
        scoreLabel.text = scoreValue.ToString();
    }

    private int ButtonCountInitializer()
    {
        int count = gameButtons.Count;
        int nearestMultiple = (int)System.Math.Round((count / (double)4), System.MidpointRounding.AwayFromZero) * 4;
        return nearestMultiple - 1;
    }

    private void OnGameOver()
    {
        GameManager.Instance.Score = gameScore;
        GameManager.Instance.OnResultPanelActive?.Invoke();
        ButtonController[] buttons = FindObjectsByType<ButtonController>(FindObjectsSortMode.None);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].StopAllCoroutines();
        }
    }

    private void OnDestroy()
    {
        ButtonController.OnClicked -= OnGameButtonClick;
        if (Application.isPlaying)
        {
            GameManager.Instance.OnScoreUpdate -= OnGameOver;
        }
    }
}
