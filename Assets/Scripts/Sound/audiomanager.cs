using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("GameManager")]
     GameManager gameManager;

    [SerializeField] AudioMixer _mixer;

    [Header("Music")]


    public List<AudioClip> audioClips; // Liste der vorab zugewiesenen Audiotracks
    public float fadeDuration = 2.0f; // Dauer des Fades
    public float loopFadeDuration = 5.0f; // Dauer des Fades innerhalb eines Loops
    float elapsedTime = 0f;

    private AudioSource audioSourceA;
    private AudioSource audioSourceB;
    private AudioSource activeSource;
    private bool isFading = false;
    //private bool loopFading = false;

    public const string volumeParameter = "MasterVolume"; // Der Name des Lautst�rkeparameters im Mixer
    private bool isMuted = false; // Zustand des Mutes


    public AudioMixerGroup musicMixer;


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
        LoadVolume();
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


    private void Start()
    {
         gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        SpawnAudioSources();

        if (audioClips.Count > 0)
        {
            audioSourceA.clip = audioClips[0];
            audioSourceA.loop = true;
            audioSourceA.Play();
            activeSource = audioSourceA;

            // StartLoopFade(audioSourceA);
        }
    }


    // Crossfade zwischen aktiver Quelle und der neuen Quelle
    public void CrossfadeToClip(int clipIndex)
    {
        //Debug.Log("MusicChange");
        if (isFading || clipIndex < 0 || clipIndex >= audioClips.Count)
        {
            Debug.LogWarning("Ung�ltiger Clip-Index oder ein Fade l�uft bereits!");
            return;
        }
        //Debug.Log("5");
        StartCoroutine(FadeToClip(clipIndex));
    }

    private IEnumerator FadeToClip(int clipIndex)
    {
        isFading = true;
        //Debug.Log("6");

        // Clip f�r die n�chste Quelle festlegen
        AudioSource newSource = (activeSource == audioSourceA) ? audioSourceB : audioSourceA;
        newSource.clip = audioClips[clipIndex];
        if (clipIndex == 7)
        {
            newSource.loop = false;
            Debug.Log("Sound1");
        }
        else
        {
            newSource.loop = true;
            Debug.Log("Sound2");
        }
        newSource.Play();

        float elapsedTime = 0f;

    

        // Lautstaerke der beiden Quellen interpolieren
        while (elapsedTime < fadeDuration)
        {
            //Debug.Log("7");
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / fadeDuration;

            activeSource.volume = Mathf.Lerp(1f, 0f, t);
            newSource.volume = Mathf.Lerp(0f, 1f, t);
            //Debug.Log(elapsedTime + " " + fadeDuration);
            yield return null;
        }

        // Lautst�rke korrekt setzen und alte Quelle stoppen
        activeSource.volume = 0f;
        newSource.volume = 1f;
        activeSource.Stop();
        activeSource = newSource;

        isFading = false;
        //Debug.Log("8");
        yield return null;
    }

        //Das wonach es klingt
    public void SpawnAudioSources()
    {
        if (audioSourceA == null)
        {
            GameObject audioObjectA = new GameObject("AudioSourceA");
            audioSourceA = audioObjectA.AddComponent<AudioSource>();
            audioObjectA.transform.SetParent(transform);

            if (musicMixer != null)
            {
                audioSourceA.outputAudioMixerGroup = musicMixer;
            }
        }

        if (audioSourceB == null)
        {
            GameObject audioObjectB = new GameObject("AudioSourceB");
            audioSourceB = audioObjectB.AddComponent<AudioSource>();
            audioObjectB.transform.SetParent(transform);

            if (musicMixer != null)
            {
                audioSourceB.outputAudioMixerGroup = musicMixer;
            }
        }
    }

    public void DespawnAudioSources()
    {
        if (audioSourceA != null)
        {
            Destroy(audioSourceA.gameObject);
            audioSourceA = null;
        }

        if (audioSourceB != null)
        {
            Destroy(audioSourceB.gameObject);
            audioSourceB = null;
        }
    }

        //Für 2D Audio in der UI
    public void PlaySoundFXUIClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject2D, spawnTransform.position, Quaternion.identity);



        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength + 1);

    }
        //Für 3D Sound in Game
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

    public void ChangeTrackOnRound()
    {

        //Defender Turn Music
        if (gameManager.defendersTurn == true)
        {
            if(gameManager.maxTurnCount * 0.5 >= gameManager.currentTurn)
            {
                CrossfadeToClip(1);
            }
            else if (gameManager.maxTurnCount - 2 >= gameManager.currentTurn)
            {
                CrossfadeToClip(2);
                //Debug.Log("RoundMusicChange");
            }
            else
            {
                CrossfadeToClip(3);
            }
        }
        //Attacker Turn Music
        else
        {
            if (gameManager.maxTurnCount * 0.5 >= gameManager.currentTurn)
            {
                CrossfadeToClip(4);
            }
            else if (gameManager.maxTurnCount - 2 >= gameManager.currentTurn)
            {
                CrossfadeToClip(5);
            }
            else
            {
                CrossfadeToClip(6);
            }
        }
    }

    void LoadVolume()
    {
        //Debug.Log("VolumeLoad");
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        _mixer.SetFloat(volumesettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        _mixer.SetFloat(volumesettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }

    public void ToggleMute()
    {

        isMuted = !isMuted;

        if (isMuted)
        {
            // Lautst�rke auf den niedrigsten Wert setzen
            _mixer.SetFloat(volumeParameter, -80f); // -80dB ist praktisch stumm
        }
        else
        {
            // Lautst�rke auf 0dB zur�cksetzen
            _mixer.SetFloat(volumeParameter, 0f);
        }
    }
}