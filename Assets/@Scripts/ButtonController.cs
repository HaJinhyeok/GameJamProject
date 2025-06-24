using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button startButton;
    public Text startButtonText;
    public Image indicator;
    public IndicatorCollision indicatorCollision;

    public float duration;
    public float buttonScore = 0;
    private float startTime;
    protected Stopwatch buttonTimer;

    public static Action<ButtonController> OnClicked;
    public static Action OnPlayerMissClicked;


    public void InitializeButton(float start, float startX, float startY, float endX, float endY)
    {
        transform.SetAsFirstSibling();
        gameObject.transform.position = new Vector3(startX, startY);

        startButton.transform.SetParent(gameObject.transform, false);

        startButton.gameObject.SetActive(true);

        startTime = start;
        buttonTimer = new Stopwatch();
        buttonTimer.Start();

        transform.localScale = Define.RandomScale[UnityEngine.Random.Range(0, Define.RandomScale.Length)];
        StartCoroutine(ScaleIndicator());
    }

    void Update()
    {
        if (startButton != null && startButton.gameObject.activeSelf && GameManager.Instance.IsPlaying && buttonTimer.ElapsedMilliseconds > duration)
        {
            buttonTimer.Stop();
            buttonTimer.Reset();
            OnClicked?.Invoke(this);

            OnPlayerMissClicked?.Invoke();
            StartCoroutine(FadeAway());
        }
        //else if (this.gameButton != null && this.gameButton.gameObject.activeSelf)
        //{
        //    this.gameButton.image.color = new Vector4(1 - CalcColor(), CalcColor(), 0, 1);
        //}
    }

    public virtual void ButtonClicked()
    {
        float clickTime = buttonTimer.ElapsedMilliseconds;
        Define.JudgementType judgement = GetJudgement(clickTime);
        buttonScore = GetFixedScoreFromJudgement(judgement);

        UnityEngine.Debug.Log($"Judgement: {judgement}, Score: {buttonScore}");
        if (judgement == Define.JudgementType.Miss)
        {
            OnPlayerMissClicked?.Invoke();
        }

        OnClicked?.Invoke(this);
        StartCoroutine(FadeAway());
    }

    public Define.JudgementType GetJudgement(float clickTime)
    {
        float delta = Mathf.Abs(clickTime - CalcPerfectTime());

        if (delta <= 20f) return Define.JudgementType.Perfect300;
        else if (delta <= 60f) return Define.JudgementType.Good100;
        else if (delta <= 100f) return Define.JudgementType.Poor50;
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

    //public float CalcScore(float clickTime)
    //{
    //    return 1 - Mathf.Abs(clickTime - CalcPerfectTime()) / CalcPerfectTime();
    //}

    //public float CalcColor()
    //{
    //    if (((buttonTimer.ElapsedMilliseconds) / CalcPerfectTime()) <= 1f)
    //    {
    //        return (buttonTimer.ElapsedMilliseconds) / CalcPerfectTime();
    //    }
    //    else if ((duration - buttonTimer.ElapsedMilliseconds) / CalcPerfectTime() <= 1)
    //    {
    //        return (duration - buttonTimer.ElapsedMilliseconds) / CalcPerfectTime();
    //    }
    //    return 0;

    //}

    private IEnumerator ScaleIndicator()
    {
        Vector3 originalScale = indicator.transform.localScale;
        Vector3 destinationScale = Vector3.one * 0.6f;

        if (buttonTimer.IsRunning)
        {
            while (buttonTimer.ElapsedMilliseconds < CalcPerfectTime())
            {
                indicator.transform.localScale = Vector3.Lerp(originalScale, destinationScale, buttonTimer.ElapsedMilliseconds / CalcPerfectTime());
                yield return null;
            }
        }
    }

    protected IEnumerator FadeAway()
    {
        Collider2D buttonCollider = indicator?.GetComponent<CircleCollider2D>();
        if (buttonCollider != null)
        {
            buttonCollider.enabled = false;
        }

        Color originalColor = startButton.image.color;
        Color finalColor = new Color(startButton.image.color.r, startButton.image.color.g, startButton.image.color.b, 0);
        Color finalTextColor = new Color(startButton.image.color.r, startButton.image.color.g, startButton.image.color.b, 0.25f);

        Vector3 originalPosition = new Vector3();

        Vector3 destination = new Vector3(originalPosition.x, originalPosition.y + 50);

        float ElapsedTime = 0.0f;
        float TotalTime = 0.6f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            startButton.image.color = Color.Lerp(originalColor, finalColor, (ElapsedTime / TotalTime));
            if (indicator != null)
            {
                indicator.color = Color.Lerp(originalColor, finalColor, (ElapsedTime / TotalTime));
            }

            startButtonText.color = Color.Lerp(originalColor, finalColor, (ElapsedTime / TotalTime));

            yield return null;
        }

        Destroy(gameObject);
    }

}
