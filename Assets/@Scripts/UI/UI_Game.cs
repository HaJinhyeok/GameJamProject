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
    //public static Action OnArrowDeactivated;
    public static Action<int> OnPercentageChanged;

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
        //OnArrowDeactivated += DeactivateArrow;
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
            // 게임 오버
            Time.timeScale = 0f;
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

    void ChangePercentageText(int progress)
    {
        _percentageText.text = $"{progress} %";
        _percentageFill.fillAmount = progress / 100f;
    }

    private void OnDestroy()
    {
        OnArrowActivated -= ActivateArrow;
        //OnArrowDeactivated -= DeactivateArrow;
        OnPercentageChanged -= ChangePercentageText;
        ButtonController.OnPlayerMissClicked -= OnPlayerLifeMiss;
        if (Application.isPlaying)
        {
            GameManager.Instance.OnResultPanelActive -= OnGameOver;
        }
    }
}
