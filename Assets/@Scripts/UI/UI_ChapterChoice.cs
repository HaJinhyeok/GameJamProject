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

    int _selectedChapter;

    void Start()
    {
        _helpButton.onClick.AddListener(OnHelpButtonClick);
        _preferenceButton.onClick.AddListener(OnPreferenceButtonClick);
        for (int i = 0; i < _chapterButton.Length; i++)
        {
            int idx = i;
            _chapterButton[idx].onClick.AddListener(()=>
            {
                _selectedChapter = idx;
                OnChapterButtonClick();
            });
        }
        _yesButton.onClick.AddListener(OnYesButtonClick);
        _noButton.onClick.AddListener(OnNoButtonClick);

        _popUpChapterConfirm.SetActive(false);
        _popUpHelp.SetActive(false);
        _popUpPreference.SetActive(false);
    }

    void OnHelpButtonClick()
    {
        _popUpHelp.SetActive(true);
    }

    void OnPreferenceButtonClick()
    {
        _popUpPreference.SetActive(true);
    }

    void OnChapterButtonClick()
    {
        _popUpChapterConfirm.SetActive(true);
    }

    void OnYesButtonClick()
    {
        // 선택된 챕터 정보를 가지고 게임 씬 실행
        //SceneManager.LoadScene(Define.GameScene);
        GameManager.Instance.GameStart(_selectedChapter);
    }

    void OnNoButtonClick()
    {
        _popUpChapterConfirm.gameObject.SetActive(false);
    }
}
