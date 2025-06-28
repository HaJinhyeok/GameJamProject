using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager s_instance = null;

    public static AudioManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject("@Managers");
                    DontDestroyOnLoad(go);
                }

                s_instance = FindAnyObjectByType<AudioManager>();
                if (s_instance == null)
                {
                    GameObject gameManager = new GameObject("AudioManager");
                    AudioManager comp = gameManager.AddComponent<AudioManager>();
                    gameManager.transform.SetParent(go.transform);
                    s_instance = comp;
                }
                else
                {
                    s_instance.gameObject.transform.SetParent(go.transform);
                }
            }
            return s_instance;
        }
    }

    public AudioMixer MasterMixer;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = Resources.Load<AudioClip>(Define.ChoiceSoundPath);
        MasterMixer = Resources.Load<AudioMixer>(Define.MasterMixerPath);
    }

    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        MasterMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 12f);
    }

    public void SetVFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        MasterMixer.SetFloat("VFXVolume", Mathf.Log10(volume) * 12f);
    }

    public void PlayButtonSound()
    {
        _audioSource.Play();
    }
}
