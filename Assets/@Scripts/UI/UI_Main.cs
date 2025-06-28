using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    [SerializeField] Image _pressAnyKeyImage;

    bool _flag = false;
    Color _currentColor;


    void Start()
    {
        Screen.SetResolution(1080, 720, false);
        // ó���� (0,0,0,1)
        _currentColor = _pressAnyKeyImage.color;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            // ���ѷα� �� ����
            SceneManager.LoadScene(Define.PrologueScene);
        }
        if (_flag)
        {
            _currentColor.a += Time.deltaTime / Define.MainPressAnyKeyImageBlinkTime;
            if (_currentColor.a >= 1f)
            {
                _flag = false;
            }
        }
        else
        {
            _currentColor.a -= Time.deltaTime / Define.MainPressAnyKeyImageBlinkTime;
            if (_currentColor.a <= 0f)
            {
                _flag = true;
            }
        }
        _pressAnyKeyImage.color = _currentColor;
    }
}
