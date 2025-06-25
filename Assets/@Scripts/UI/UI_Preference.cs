using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Preference : MonoBehaviour
{
    [SerializeField] Button _backButton;
    [SerializeField] Button _firstButton;
    [SerializeField] Button _secondButton;
    [SerializeField] TMP_Text _musicNameText;
    [SerializeField] Slider _sliderBGM;
    [SerializeField] Slider _sliderVFX;

    void Start()
    {
        _backButton.onClick.AddListener(OnBackButtonClick);
        _firstButton.onClick.AddListener(OnFirstButtonClick);
        _secondButton.onClick.AddListener(OnSecondButtonClick);
        _sliderBGM.onValueChanged.AddListener((value) =>
        AudioManager.Instance.SetBGMVolume(value));
        _sliderVFX.onValueChanged.AddListener((value) =>
        AudioManager.Instance.SetVFXVolume(value));
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == Define.GameScene)
        {
            _musicNameText.text = GameManager.Instance.CurrentMusic.name.Substring(1);
            _musicNameText.text += $"\n{GameManager.Instance.Percentage} %";
        }
        if (AudioManager.Instance.MasterMixer.GetFloat(Define.BGM, out float currentBGM))
        {
            float linear = Mathf.Pow(10f, currentBGM / 12f);
            _sliderBGM.SetValueWithoutNotify(linear); // �����̴� �ʱⰪ ����
        }
        if (AudioManager.Instance.MasterMixer.GetFloat(Define.VFX, out float currentVFX))
        {
            float linear = Mathf.Pow(10f, currentVFX / 12f);
            _sliderVFX.SetValueWithoutNotify(linear); // �����̴� �ʱⰪ ����
        }

    }

    void OnBackButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        if (SceneManager.GetActiveScene().name == Define.GameScene)
        {
            GameController.OnPreferencePanelSet?.Invoke(false);
        }
    }

    void OnFirstButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        // ���þ����� ���� ����
        if (SceneManager.GetActiveScene().name == Define.ChapterChoiceScene)
        {
            ExitGame();
        }
        // ���Ӿ����� �����
        else if (SceneManager.GetActiveScene().name == Define.GameScene)
        {
            GameManager.Instance.GameStart();
        }
    }

    void OnSecondButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        // ���þ����� ���ѷα׾�����
        if (SceneManager.GetActiveScene().name == Define.ChapterChoiceScene)
        {
            SceneManager.LoadScene(Define.PrologueScene);
        }
        // ���Ӿ����� ���ξ�����
        else if (SceneManager.GetActiveScene().name == Define.GameScene)
        {
            SceneManager.LoadScene(Define.ChapterChoiceScene);
        }
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
