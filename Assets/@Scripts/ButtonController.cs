using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] protected Sprite[] _buttonSprites = new Sprite[2];
    public Button startButton;
    public Image indicator;
    public IndicatorCollision indicatorCollision;

    public float duration;
    public float buttonScore = 0;

    protected float _currentTime;
    // 인디케이터가 줄어들고 있는지 여부
    protected bool _isScaling;
    // FadOut 중인지 여부
    protected bool _isFadingOut;
    // indicator scale 값
    Vector3 _originalScale;
    Vector3 _destinationScale;

    protected float _fadeElapsedTime = 0f;
    protected float _fadeTotalTime = 0.6f;
    Color _fadeStartColor;
    Color _fadeEndColor;
    Color _fadeEndTextColor;

    public static Action<ButtonController> OnClicked;
    public static Action OnPlayerMissClicked;


    public void InitializeButton(float start, float startX, float startY, float endX, float endY)
    {
        transform.SetAsFirstSibling();
        gameObject.transform.position = new Vector3(startX, startY);

        startButton.transform.SetParent(gameObject.transform, false);

        startButton.gameObject.SetActive(true);
        startButton.image.sprite = _buttonSprites[UnityEngine.Random.Range(0, _buttonSprites.Length)];

        _currentTime = 0f;

        transform.localScale = Define.RandomScale[UnityEngine.Random.Range(0, Define.RandomScale.Length)];

        if (indicator != null)
        {
            _originalScale = indicator.transform.localScale;
        }
        _destinationScale = Vector3.one * 0.6f;
        _isScaling = true;
        _isFadingOut = false;
    }

    void Update()
    {
        if (startButton != null && startButton.gameObject.activeSelf)
        {
            _currentTime += Time.deltaTime;
            if (_isScaling)
            {
                // ScalingIndicator
                if (_currentTime < CalcPerfectTime())
                {
                    indicator.transform.localScale = Vector3.Lerp(_originalScale, _destinationScale, _currentTime / CalcPerfectTime());
                }
                if (_currentTime > duration)
                {
                    UnityEngine.Debug.Log("Missed Button!");
                    OnPlayerMissClicked?.Invoke();
                    _isScaling = false;
                    _isFadingOut = true;

                    // Fade 초기화
                    _fadeElapsedTime = 0f;
                    _fadeStartColor = startButton.image.color;
                    _fadeEndColor = new Color(_fadeStartColor.r, _fadeStartColor.g, _fadeStartColor.b, 0f);
                    _fadeEndTextColor = new Color(_fadeStartColor.r, _fadeStartColor.g, _fadeStartColor.b, 0.25f);

                    // 콜라이더 비활성화
                    Collider2D buttonCollider = indicator?.GetComponent<CircleCollider2D>();
                    if (buttonCollider != null)
                        buttonCollider.enabled = false;
                }
            }
            if (_isFadingOut)
            {
                _fadeElapsedTime += Time.deltaTime;

                float t = Mathf.Clamp01(_fadeElapsedTime / _fadeTotalTime);
                startButton.image.color = Color.Lerp(_fadeStartColor, _fadeEndColor, t);
                if (indicator != null)
                    indicator.color = Color.Lerp(_fadeStartColor, _fadeEndColor, t);

                if (_fadeElapsedTime >= _fadeTotalTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public virtual void ButtonClicked()
    {
        Define.JudgementType judgement = GetJudgement(_currentTime);
        buttonScore = GetFixedScoreFromJudgement(judgement);

        UnityEngine.Debug.Log($"Judgement: {judgement}, Score: {buttonScore}");
        if (judgement == Define.JudgementType.Miss)
        {
            OnPlayerMissClicked?.Invoke();
        }

        OnClicked?.Invoke(this);
        _isFadingOut = true;
    }

    public Define.JudgementType GetJudgement(float clickTime)
    {
        float delta = Mathf.Abs(clickTime - CalcPerfectTime());

        if (delta <= 0.1f) return Define.JudgementType.Perfect300;
        else if (delta <= 0.2f) return Define.JudgementType.Good100;
        else if (delta <= 0.3f) return Define.JudgementType.Poor50;
        else return Define.JudgementType.Miss;
    }

    public float GetFixedScoreFromJudgement(Define.JudgementType judgement)
    {
        switch (judgement)
        {
            case Define.JudgementType.Perfect300: return 300;
            case Define.JudgementType.Good100: return 100;
            case Define.JudgementType.Poor50: return 50;
            default: return 0;
        }
    }

    public float CalcPerfectTime()
    {
        return ((duration) / 2f);
    }
}
