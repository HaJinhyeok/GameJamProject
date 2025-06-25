using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Help : MonoBehaviour
{
    [SerializeField] Image _explanationImage;
    [SerializeField] Image _explanationTextImage;
    [SerializeField] Button _backButton;
    [SerializeField] Button[] _informationButtons = new Button[5];
    [SerializeField] TMP_Text _explanationText;
    [SerializeField] Sprite[] _explanationSpriteList = new Sprite[5];
    [SerializeField] Sprite[] _explanationTextSpriteList = new Sprite[5];

    void Start()
    {
        _backButton.onClick.AddListener(OnBackButtonClick);
        for (int i = 0; i < _informationButtons.Length; i++)
        {
            int idx = i;
            _informationButtons[idx].onClick.AddListener(() =>
            {
                OnInformationButtonClick(idx);
            });
        }
        OnInformationButtonClick(0);
    }

    void OnBackButtonClick()
    {
        AudioManager.Instance.PlayButtonSound();
        gameObject.SetActive(false);
    }

    void OnInformationButtonClick(int idx)
    {
        AudioManager.Instance.PlayButtonSound();
        _explanationImage.sprite = _explanationSpriteList[idx];
        _explanationTextImage.sprite = _explanationTextSpriteList[idx];
    }
}
