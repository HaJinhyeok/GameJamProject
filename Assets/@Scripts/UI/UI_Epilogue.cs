using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Epilogue : MonoBehaviour
{
    [SerializeField] Button _nextButton;
    [SerializeField] Image _epilogueBackground;
    [SerializeField] Sprite[] _successSprites;
    [SerializeField] Sprite[] _failureSprites;
    [SerializeField] AudioSource _alarmSound;

    void Start()
    {
        _nextButton.onClick.AddListener(OnNextButtonClick);
        _alarmSound.Play();
        GameManager.Instance.IsPlaying = false;
        if(GameManager.Instance.IsSuccess)
        {
            _epilogueBackground.sprite = _successSprites[Random.Range(0, _successSprites.Length)];
        }
        else
        {
            _epilogueBackground.sprite = _failureSprites[Random.Range(0, _failureSprites.Length)];
        }
    }

    void OnNextButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        gameObject.SetActive(false);
    }
}
