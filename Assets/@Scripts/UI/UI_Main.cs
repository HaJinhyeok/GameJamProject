using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Main : MonoBehaviour
{
    [SerializeField] TMP_Text _pressAnyKeyText;

    bool _flag = false;
    Color _currentColor;
    

    void Start()
    {
        Screen.SetResolution(1080, 720, false);
        // 처음엔 (0,0,0,1)
        _currentColor = _pressAnyKeyText.color;
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            // 프롤로그 씬 연결
            //SceneManager.LoadScene(Define.PrologueScene);
            SceneManager.LoadScene(Define.ChapterChoiceScene);
        }
        if(_flag)
        {
            _currentColor.a += 0.01f;
            if (_currentColor.a >= 1f)
            {
                _flag = false;
            }
        }
        else
        {
            _currentColor.a -= 0.01f;
            if (_currentColor.a <= 0f)
            {
                _flag = true;
            }
        }
        _pressAnyKeyText.color = _currentColor;
    }
}
