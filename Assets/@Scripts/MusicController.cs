using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource AudioSource;
    float _musicTimer;
    int _currentPercentage;

    void Start()
    {
        LoadMusic();
    }

    private void Update()
    {
        if (AudioSource.isPlaying)
        {
            _musicTimer += Time.deltaTime;
            int newPercentage = (int)(_musicTimer / AudioSource.clip.length * 100);
            if (newPercentage > _currentPercentage)
            {
                _currentPercentage = newPercentage;
                UI_Game.OnPercentageChanged?.Invoke(_currentPercentage);
                GameManager.Instance.Percentage = _currentPercentage;
            }
            if (_currentPercentage >= 100)
            {
                GameManager.Instance.IsSuccess = true;
                GameManager.Instance.Percentage = _currentPercentage;
                GameManager.Instance.OnScoreUpdate?.Invoke();
            }
        }
    }

    public void PlayAudio()
    {
        if (AudioSource.clip != null)
        {
            AudioSource.Play();
            _musicTimer = 0f;
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
        //AudioClip clip = Resources.Load<AudioClip>("Musics/0babyshark");
        if (clip != null)
        {
            AudioSource.clip = clip;
        }
    }
}
