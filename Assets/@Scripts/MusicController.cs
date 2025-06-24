using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource AudioSource;
    //public string clipName;

	void Start ()
    {
        LoadMusic();
	}

    public void PlayAudio()
    {
        if (AudioSource.clip != null)
        {
            AudioSource.Play();
        }
    }

    private void LoadMusic()
    {
        //AudioClip clip = GameManager.Instance.CurrentMusic;
        AudioClip clip = Resources.Load<AudioClip>("Musics/0babyshark");
        if (clip != null)
        {
            this.AudioSource.clip = clip;
        }
    }
}
