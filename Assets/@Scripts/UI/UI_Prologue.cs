using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Prologue : MonoBehaviour
{
    [SerializeField] Sprite[] _prologueSprites;
    [SerializeField] Image _prologueImage;
    [SerializeField] AudioSource _tiktokSound;

    Color _currentColor;

    void Start()
    {
        _currentColor = new Color(1, 1, 1, 0);
        _prologueImage.color = _currentColor;
        StartCoroutine(CoPlayPrologue());
    }

    IEnumerator CoPlayPrologue()
    {
        int i = 0;
        while (i < _prologueSprites.Length)
        {
            _prologueImage.sprite = _prologueSprites[i++];
            while (_currentColor.a < 1f)
            {
                _currentColor.a += Time.deltaTime / Define.PrologueFadeTime;
                _prologueImage.color = _currentColor;
                yield return null;
            }
            yield return new WaitForSeconds(Define.PrologueKeepTime);
            if (i == _prologueSprites.Length)
            {
                StartCoroutine(MoveToChapterChoiceScene());
            }
            while (_currentColor.a > 0f)
            {
                if (i == _prologueSprites.Length)
                {
                    _currentColor.a -= Time.deltaTime / (Define.PrologueFadeTime * 3f);
                }
                else
                {
                    _currentColor.a -= Time.deltaTime / Define.PrologueFadeTime;
                }

                _prologueImage.color = _currentColor;
                yield return null;
            }
        }
        MoveToChapterChoiceScene();
    }
    IEnumerator MoveToChapterChoiceScene()
    {
        _tiktokSound.Play();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(Define.ChapterChoiceScene);
    }
}
