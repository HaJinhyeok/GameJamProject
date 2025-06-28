using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Result : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _percentageText;
    [SerializeField] Button _retryButton;
    [SerializeField] Button _backButton;
    [SerializeField] AudioSource _successSound;
    [SerializeField] AudioSource _failureSound;

    void OnEnable()
    {
        if (GameManager.Instance.IsSuccess)
        {
            _successSound.Play();
        }
        else
        {
            _failureSound.Play();
        }
        _scoreText.text = $"Á¡¼ö : {GameManager.Instance.Score}";
        _percentageText.text = $"{GameManager.Instance.Percentage * 100:F0} %";
        _retryButton.onClick.AddListener(OnRetryButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    void OnRetryButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        GameManager.Instance.GameStart();
    }

    void OnBackButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        SceneManager.LoadScene(Define.ChapterChoiceScene);
        Time.timeScale = 1f;
    }
}
