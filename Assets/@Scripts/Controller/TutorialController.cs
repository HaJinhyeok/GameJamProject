using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] Sprite[] _tutorialSprites;
    [SerializeField] GameObject _transparentPanel;
    [SerializeField] GameObject _tutorialImage;

    GameController _gameController;

    public static bool IsTutorial;
    public static Action<Define.TutorialType, Vector2> OnTutorial;

    private void Start()
    {
        // 튜토리얼 아직 안깼으면 진행
        if (!GameManager.Instance.IsTutorialClear)
        {
            IsTutorial = true;
        }
        OnTutorial += StartTutorial;
        _gameController = GameObject.FindGameObjectWithTag(Define.GameControllerTag).GetComponent<GameController>();
        if (_gameController == null)
        {
            Debug.LogWarning("Cannot Find GameController!!!");
        }
        if (IsTutorial)
        {
            Invoke("StartPercentageTutorial", 1.5f);
        }
    }

    // 1. 일반 버블
    // 2. 방해 버블
    // 3. 라이프 강제 깎기
    // 4. 방향 전환
    // 5. 곡 진행도

    void StartTutorial(Define.TutorialType type, Vector2 pos)
    {
        // 화면 정지
        Time.timeScale = 0f;
        // 노래 정지
        GameController.OnPreferencePanelSet?.Invoke(true);

        Image image = Instantiate(_tutorialImage, _gameController.transform).GetComponent<Image>();
        image.sprite = _tutorialSprites[(int)type];
        // 방해 노트의 경우, 랜덤 위치 생성되므로 위치에 따라 튜토리얼 안내문 생성 위치 조정
        if (type == Define.TutorialType.Disturbance && pos.y > 240)
        {
            image.gameObject.transform.localPosition = new Vector3(pos.x, pos.y) - Define.TutorialPosition[(int)type];
        }
        else
        {
            image.gameObject.transform.localPosition = new Vector3(pos.x, pos.y) + Define.TutorialPosition[(int)type];
        }


        // 라이프 튜토리얼은 타이밍을 별도로 지정
        if (type == Define.TutorialType.Arrow)
        {
            Invoke("StartLifeTutorial", 4f);
        }

        SetTransparentPanel(true);
        StartCoroutine(CoVanishTutorialImage(image));
    }

    void StartLifeTutorial()
    {
        StartTutorial(Define.TutorialType.Life, Vector2.zero);
    }

    void StartPercentageTutorial()
    {
        StartTutorial(Define.TutorialType.Percentage, Vector2.zero);
    }

    IEnumerator CoVanishTutorialImage(Image image)
    {
        yield return new WaitForSecondsRealtime(Define.TutorialPauseTime);
        Destroy(image.gameObject);
        Time.timeScale = 1.0f;
        GameController.OnPreferencePanelSet?.Invoke(false);
        SetTransparentPanel(false);
    }

    void SetTransparentPanel(bool flag)
    {
        if (flag)
        {
            _transparentPanel.SetActive(true);
        }
        else
        {
            _transparentPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        IsTutorial = false;
        OnTutorial -= StartTutorial;
    }
}
