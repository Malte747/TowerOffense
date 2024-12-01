using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer _mixer;

    [Header("Music")]
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioClip[] _music;
    [SerializeField] private float _fadeTimerInterval = 0.001f;
    private bool _titleMusicPlaying = false;
    private bool _gameMusicPlaying = false;

    [Header("SFX")]

    [Header("Game")]
    [SerializeField] private List<AudioClip> _gameSFX;

    [Header("UI")]
    [SerializeField] private List<AudioClip> _uiSFX;
    [SerializeField] private AudioSource soundFXObject2D;
    [SerializeField] private AudioSource soundFXObject3D;

    public static AudioManager instance;

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolume();
    }

    public void StartTitleMusic(int hummer)
    {
        if (!_titleMusicPlaying)
        {
            _titleMusic.Play();
            _gameMusic.Stop();
            _titleMusic.volume = 0.5f;
            _gameMusicPlaying = false;
            _titleMusicPlaying = true;
        }
    }

    public void StartGameMusic()
    {
        if (!_gameMusicPlaying)
        {
            _gameMusic.Play();
            _titleMusic.Stop();
            _gameMusic.volume = 0.5f;
            _titleMusicPlaying = false;
            _gameMusicPlaying = true;
        }
    }

   /* public void GamePaused(bool paused)
    {
        if (paused)
        {
            _pause.Play();
            _gameMusic.Pause();
            _titleMusic.Play();
        }

    }
   */
    public void StartFadeTitleMusicOut()
    {
        StartCoroutine(FadeTitleMusic(true));
    }

    public void StartFadeTitleMusicIn()
    {
        StartCoroutine(FadeTitleMusic(false));
    }

    public void StartFadeGameMusicOut()
    {
        StartCoroutine(FadeOutGameMusic(true));
    }

    public void StartFadeGameMusicIn()
    {
        StartCoroutine(FadeOutGameMusic(false));
    }

    private IEnumerator FadeTitleMusic(bool fadeOut)
    {
        if (fadeOut)
        {
            while (_titleMusic.volume > 0)
            {
                _titleMusic.volume -= 0.001f;
                yield return new WaitForSeconds(_fadeTimerInterval);
            }
        }
        else if (!fadeOut)
        {
            while (_titleMusic.volume < 0.5f)
            {
                _titleMusic.volume += 0.001f;
                yield return new WaitForSeconds(_fadeTimerInterval);
            }
        }

    }

    private IEnumerator FadeOutGameMusic(bool fadeOut)
    {
        if (fadeOut)
        {
            Debug.Log("game music fading out.");
            _gameMusic.volume = .5f;
            while (_gameMusic.volume > 0)
            {
                _gameMusic.volume -= 0.001f;
                yield return new WaitForSeconds(_fadeTimerInterval);
            }
        }
        else if (!fadeOut)
        {
            Debug.Log("game music fading in.");
            _gameMusic.volume = 0;
            while (_gameMusic.volume < 0.5f)
            {
                _gameMusic.volume += 0.001f;
                yield return new WaitForSeconds(_fadeTimerInterval);
            }
        }
    }

    /*public void PlayRandomExplosion()
    {
        int clip = Random.Range(0, _explosionClips.Length);
        _explosionSource.pitch -= Random.Range(0.9f, 1.1f);
        _explosionSource.PlayOneShot(_explosionClips[clip]);

    }
    */
    public void PlaySoundFXUIClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject2D, spawnTransform.position, Quaternion.identity);



        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength + 1);

    }

    public void PlaySoundFXGameClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject3D, spawnTransform.position, Quaternion.identity);



        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength + 1);

    }

    public void PlayGameSound(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < _gameSFX.Count)
        {
            PlaySoundFXGameClip(_gameSFX[soundIndex], transform, 1f);
        }
    }


    public void PlayUISound(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < _uiSFX.Count)
        {
            PlaySoundFXUIClip(_uiSFX[soundIndex], transform, 1f);
        }
    }

    void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        _mixer.SetFloat(volumesettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        _mixer.SetFloat(volumesettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}