using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Help : MonoBehaviour
{
    [SerializeField] Button _backButton;
    [SerializeField] Image _explanationImage;
    [SerializeField] TMP_Text _explanationText;

    void Start()
    {
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    void OnBackButtonClick()
    {
        gameObject.SetActive(false);
    }
}
