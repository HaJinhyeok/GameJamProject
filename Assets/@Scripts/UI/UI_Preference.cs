using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Preference : MonoBehaviour
{
    [SerializeField] Button _backButton;
    [SerializeField] Button _restartButton;
    [SerializeField] Button _backToTitleButton;

    void Start()
    {
        _backButton.onClick.AddListener(OnBackButtonClick);
        _restartButton.onClick.AddListener(OnRestartButtonClick);
        _backToTitleButton.onClick.AddListener(OnBackToTitleButtonClick);
    }

    void OnBackButtonClick()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void OnRestartButtonClick()
    {

    }

    void OnBackToTitleButtonClick()
    {
        SceneManager.LoadScene(Define.MainScene);
    }
}
