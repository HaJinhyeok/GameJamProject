using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ChapterChoice : MonoBehaviour
{
    [SerializeField] Button _helpButton;
    [SerializeField] Button _preferenceButton;
    [SerializeField] Button[] _chapterButton;
    [SerializeField] GameObject _popUpChapterConfirm;
    [SerializeField] GameObject _popUpHelp;
    [SerializeField] GameObject _popUpPreference;
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;
    [SerializeField] Button _backButton;
    [SerializeField] TMP_Text _chapterConfirmText;

    int _selectedChapter;

    void Start()
    {
        _helpButton.onClick.AddListener(OnHelpButtonClick);
        _preferenceButton.onClick.AddListener(OnPreferenceButtonClick);
        for (int i = 0; i < _chapterButton.Length; i++)
        {
            int idx = i;
            _chapterButton[idx].onClick.AddListener(() =>
            {
                _selectedChapter = idx;
                OnChapterButtonClick();
            });
        }
        for (int i = 0; i <= GameManager.Instance.MaxChapter; i++)
        {
            _chapterButton[i].GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
        }
        _yesButton.onClick.AddListener(OnYesButtonClick);
        _noButton.onClick.AddListener(OnNoButtonClick);
        _backButton.onClick.AddListener(OnNoButtonClick);

        _popUpChapterConfirm.SetActive(false);
        _popUpHelp.SetActive(false);
        _popUpPreference.SetActive(false);
    }

    void OnHelpButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        _popUpHelp.SetActive(true);
    }

    void OnPreferenceButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        _popUpPreference.SetActive(true);
    }

    void OnChapterButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        _popUpChapterConfirm.SetActive(true);
        if (_selectedChapter <= GameManager.Instance.GameMusics.Length - 1 && _selectedChapter <= GameManager.Instance.MaxChapter)
        {
            _chapterConfirmText.text = Define.ChapterConfirm;
            _backButton.gameObject.SetActive(false);
            _yesButton.gameObject.SetActive(true);
            _noButton.gameObject.SetActive(true);
        }
        else
        {
            _chapterConfirmText.text = Define.ChapterUndefined;
            _backButton.gameObject.SetActive(true);
            _yesButton.gameObject.SetActive(false);
            _noButton.gameObject.SetActive(false);
        }
    }

    void OnYesButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        // 선택된 챕터 정보를 가지고 게임 씬 실행
        GameManager.Instance.GameStart(_selectedChapter);
    }

    void OnNoButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        _popUpChapterConfirm.gameObject.SetActive(false);
    }
}
