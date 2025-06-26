using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager s_instance = null;

    public static TutorialManager Instance
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

                s_instance = FindAnyObjectByType<TutorialManager>();
                if (s_instance == null)
                {
                    GameObject tutorialManager = new GameObject("TutorialManager");
                    TutorialManager comp = tutorialManager.AddComponent<TutorialManager>();
                    tutorialManager.transform.SetParent(go.transform);
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
    [SerializeField] Sprite _bubbleTutorialImage;
    [SerializeField] Sprite _disturbanceTutorialImage;
    [SerializeField] Sprite _lifeTutorialImage;
    [SerializeField] Sprite _arrowTutorialImage;
    GameObject _gamePanel;

    public bool IsTutorial = true;

    // 1. 일반 버블
    // 2. 방해 버블
    // 3. 라이프 강제 깎기
    // 4. 방향 전환

    public void StartBubbleTutorial(Vector2 pos)
    {
        Image image = new GameObject().AddComponent<Image>();
        image.sprite = _bubbleTutorialImage;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 200);
        if (_gamePanel == null)
        {
            _gamePanel = GameObject.Find("GamePanel");
        }
        image.transform.SetParent(_gamePanel.transform);
        image.transform.position = new Vector3(pos.x + 250, pos.y);
        StartCoroutine(CoVanishTutorialImage(image));
    }

    public void StartDisturbanceTutorial(Vector2 pos)
    {
        Image image = new GameObject().AddComponent<Image>();
        image.sprite = _disturbanceTutorialImage;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 200);
        if (_gamePanel == null)
        {
            _gamePanel = GameObject.Find("GamePanel");
        }
        image.transform.SetParent(_gamePanel.transform);
        image.transform.position = new Vector3(pos.x, pos.y + 150);
        StartCoroutine(CoVanishTutorialImage(image));

    }

    public void StartLifeTutorial()
    {
        Image image = new GameObject().AddComponent<Image>();
        image.sprite = _lifeTutorialImage;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 200);
        if (_gamePanel == null)
        {
            _gamePanel = GameObject.Find("GamePanel");
        }
        image.transform.SetParent(_gamePanel.transform);
        image.transform.position = new Vector3(-200, -200);
        StartCoroutine(CoVanishTutorialImage(image));
    }

    public void StartArrowTutorial(Vector2 pos)
    {
        Image image = new GameObject().AddComponent<Image>();
        image.sprite = _arrowTutorialImage;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 200);
        if (_gamePanel == null)
        {
            _gamePanel = GameObject.Find("GamePanel");
        }
        image.transform.SetParent(_gamePanel.transform);
        image.transform.position = new Vector3(400, 200);
        StartCoroutine(CoVanishTutorialImage(image));
        Invoke("StartLifeTutorial", 3f);
    }

    IEnumerator CoVanishTutorialImage(Image image)
    {
        yield return new WaitForSecondsRealtime(2f);
        Destroy(image.gameObject);
        Time.timeScale = 1.0f;
        GameController.OnPreferencePanelSet?.Invoke(false);
    }

}
