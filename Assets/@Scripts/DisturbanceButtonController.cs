using UnityEngine;

public class DisturbanceButtonController : ButtonController
{
    float _currentTime;
    private void OnEnable()
    {
        _currentTime = 0;
    }
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= duration)
        {
            StartCoroutine(FadeAway());
        }
    }

    public override void ButtonClicked()
    {
        OnPlayerMissClicked?.Invoke();
        StartCoroutine(FadeAway());
    }
}
