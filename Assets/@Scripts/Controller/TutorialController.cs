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
        // Ʃ�丮�� ���� �Ȳ����� ����
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

    // 1. �Ϲ� ����
    // 2. ���� ����
    // 3. ������ ���� ���
    // 4. ���� ��ȯ
    // 5. �� ���൵

    void StartTutorial(Define.TutorialType type, Vector2 pos)
    {
        // ȭ�� ����
        Time.timeScale = 0f;
        // �뷡 ����
        GameController.OnPreferencePanelSet?.Invoke(true);

        Image image = Instantiate(_tutorialImage, _gameController.transform).GetComponent<Image>();
        image.sprite = _tutorialSprites[(int)type];
        // ���� ��Ʈ�� ���, ���� ��ġ �����ǹǷ� ��ġ�� ���� Ʃ�丮�� �ȳ��� ���� ��ġ ����
        if (type == Define.TutorialType.Disturbance && pos.y > 240)
        {
            image.gameObject.transform.localPosition = new Vector3(pos.x, pos.y) - Define.TutorialPosition[(int)type];
        }
        else
        {
            image.gameObject.transform.localPosition = new Vector3(pos.x, pos.y) + Define.TutorialPosition[(int)type];
        }


        // ������ Ʃ�丮���� Ÿ�̹��� ������ ����
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
