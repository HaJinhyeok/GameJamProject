using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    [SerializeField] Button _preferenceButton;
    [SerializeField] TMP_Text _percentageText;
    [SerializeField] Image _percentageFill;
    [SerializeField] Image _playerLifeImage;
    [SerializeField] Sprite[] _playerLifeSpriteList = new Sprite[4];
    [SerializeField] GameObject _popUpPreference;
    [SerializeField] GameObject _popUpEpilogue;
    [SerializeField] GameObject _popUpResult;
    [SerializeField] Image _arrow;
    [SerializeField] Sprite[] _arrowSprites = new Sprite[2];

    int _currentLife;
    Animator _arrowAnimator;

    public static Action<KeyCode> OnArrowActivated;
    public static Action<float> OnPercentageChanged;

    void Start()
    {
        _currentLife = 3;
        _playerLifeImage.sprite = _playerLifeSpriteList[_currentLife];
        _arrowAnimator = _arrow.GetComponent<Animator>();
        _arrow.gameObject.SetActive(false);

        _preferenceButton.onClick.AddListener(OnPreferenceButtonClick);

        ButtonController.OnPlayerMissClicked += OnPlayerLifeMiss;
        GameManager.Instance.OnResultPanelActive += OnGameOver;
        OnArrowActivated += ActivateArrow;
        OnPercentageChanged += ChangePercentageText;

        _popUpPreference.SetActive(false);
        _popUpEpilogue.SetActive(false);
        _popUpResult.SetActive(false);
    }

    void OnPreferenceButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        _popUpPreference.SetActive(true);
        Time.timeScale = 0f;
        GameController.OnPreferencePanelSet?.Invoke(true);
    }

    void OnPlayerLifeMiss()
    {
        _playerLifeImage.sprite = _playerLifeSpriteList[--_currentLife];
        if (_currentLife == 0)
        {
            // ���� ����
            Time.timeScale = 0f;
            GameManager.Instance.IsSuccess = false;
            GameManager.Instance.IsPlaying = false;
        }
    }

    void OnGameOver()
    {
        _popUpEpilogue.SetActive(true);
        _popUpResult.SetActive(true);
    }

    void ActivateArrow(KeyCode key)
    {
        _arrow.color = Color.white;
        if (key == KeyCode.A)
        {
            _arrow.gameObject.SetActive(true);
            _arrow.sprite = _arrowSprites[0];
            _arrowAnimator.SetTrigger("Left");
        }
        else if (key == KeyCode.D)
        {
            _arrow.gameObject.SetActive(true);
            _arrow.sprite = _arrowSprites[1];
            _arrowAnimator.SetTrigger("Right");
        }
    }

    void ChangePercentageText(float progress)
    {
        _percentageText.text = $"{progress * 100:F0} %";
        _percentageFill.fillAmount = progress;
    }

    private void OnDestroy()
    {
        OnArrowActivated -= ActivateArrow;
        OnPercentageChanged -= ChangePercentageText;
        ButtonController.OnPlayerMissClicked -= OnPlayerLifeMiss;
        if (Application.isPlaying)
        {
            GameManager.Instance.OnResultPanelActive -= OnGameOver;
        }
    }
}
