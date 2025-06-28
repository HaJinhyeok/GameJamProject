using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource AudioSource;
    int _currentPercentage;

    void Start()
    {
        LoadMusic();
    }

    private void Update()
    {
        if (AudioSource.isPlaying)
        {
            float newPercentage = (AudioSource.time / AudioSource.clip.length);
            if (newPercentage > _currentPercentage)
            {
                _currentPercentage = (int)newPercentage;
                UI_Game.OnPercentageChanged?.Invoke(newPercentage);
                GameManager.Instance.Percentage = newPercentage;
            }
            if (newPercentage >= 0.99)
            {
                Time.timeScale = 0f;
                // 튜토리얼 클리어 시
                if (TutorialController.IsTutorial)
                {
                    GameManager.Instance.IsTutorialClear = true;
                }
                GameManager.Instance.Percentage = 1f;
                GameManager.Instance.IsSuccess = true;
                GameManager.Instance.IsPlaying = false;
            }
        }
    }

    public void PlayAudio()
    {
        if (AudioSource.clip != null)
        {
            AudioSource.Play();
            _currentPercentage = 0;
            UI_Game.OnPercentageChanged?.Invoke(_currentPercentage);
            GameManager.Instance.Percentage = _currentPercentage;
        }
    }

    public void StopAudio()
    {
        if (AudioSource.clip != null)
        {
            AudioSource.Stop();
        }
    }

    private void LoadMusic()
    {
        AudioClip clip = GameManager.Instance.CurrentMusic;
        if (clip != null)
        {
            AudioSource.clip = clip;
        }
    }
}
