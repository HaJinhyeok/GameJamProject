using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    [SerializeField] Button _preferenceButton;
    [SerializeField] TMP_Text _percentageText;
    [SerializeField] Image[] _playerLifeImage;
    [SerializeField] GameObject _popUpPreference;
    [SerializeField] GameObject _popUpResult;

    int _currentLife;

    void Start()
    {
        _currentLife = _playerLifeImage.Length;
        for (int i = 0; i < _currentLife; i++)
        {
            _playerLifeImage[i].gameObject.SetActive(true);
        }

        _preferenceButton.onClick.AddListener(OnPreferenceButtonClick);

        ButtonController.OnPlayerMissClicked += OnPlayerLifeMiss;
        GameManager.Instance.OnResultPanelActive += OnGameOver;
    }

    void OnPreferenceButtonClick()
    {
        _popUpPreference.SetActive(true);
        Time.timeScale = 0f;
    }

    void OnPlayerLifeMiss()
    {
        _playerLifeImage[--_currentLife].gameObject.SetActive(false);
        if (_currentLife == 0)
        {
            // 게임 오버
            Time.timeScale = 0f;
            GameManager.Instance.IsPlaying = false;
        }
    }

    void OnGameOver()
    {
        _popUpResult.SetActive(true);
    }

    private void OnDisable()
    {
        ButtonController.OnPlayerMissClicked -= OnPlayerLifeMiss;
        if (Application.isPlaying)
        {
            GameManager.Instance.OnResultPanelActive -= OnGameOver;
        }
    }

}
