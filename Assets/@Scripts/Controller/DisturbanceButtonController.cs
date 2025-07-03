using UnityEngine;

public class DisturbanceButtonController : ButtonController
{
    private void OnEnable()
    {
        _currentTime = 0;
    }
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    public override void ButtonClicked()
    {
        OnPlayerMissClicked?.Invoke();
        Destroy(gameObject);
    }
}
