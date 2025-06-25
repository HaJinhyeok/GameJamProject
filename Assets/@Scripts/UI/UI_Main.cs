using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    [SerializeField] TMP_Text _pressAnyKeyText;
    [SerializeField] Image _pressAnyKeyImage;

    bool _flag = false;
    Color _currentColor;


    void Start()
    {
        Screen.SetResolution(1080, 720, false);
        // 처음엔 (0,0,0,1)
        //_currentColor = _pressAnyKeyText.color;
        _currentColor = _pressAnyKeyImage.color;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            // 프롤로그 씬 연결
            SceneManager.LoadScene(Define.PrologueScene);
            //SceneManager.LoadScene(Define.ChapterChoiceScene);
        }
        if (_flag)
        {
            _currentColor.a += Time.deltaTime / 1.5f;
            if (_currentColor.a >= 1f)
            {
                _flag = false;
            }
        }
        else
        {
            _currentColor.a -= Time.deltaTime / 1.5f;
            if (_currentColor.a <= 0f)
            {
                _flag = true;
            }
        }
        //_pressAnyKeyText.color = _currentColor;
        _pressAnyKeyImage.color = _currentColor;
    }
}
