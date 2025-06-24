using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Result : MonoBehaviour
{
    [SerializeField] TMP_Text _resultText;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] Button _retryButton;
    [SerializeField] Button _backButton;

    void OnEnable()
    {
        if(GameManager.Instance.IsSuccess)
        {
            _resultText.text = Define.Success;
        }
        else
        {
            _resultText.text = Define.Fail;
        }
        _scoreText.text = $"SCORE : {GameManager.Instance.Score:D4}";
        _retryButton.onClick.AddListener(OnRetryButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    void OnRetryButtonClick()
    {
        GameManager.Instance.GameStart();
    }

    void OnBackButtonClick()
    {
        SceneManager.LoadScene(Define.ChapterChoiceScene);
    }
}
