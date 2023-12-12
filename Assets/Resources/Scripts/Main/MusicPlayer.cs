using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public AudioClip music;

	private AudioSource musicSource;

	private float initialSourceVolume = 1f;

	private float timeStartPlaying;

	private const float LEAD_IN_TIME = 2f; 

	public void Awake()
	{
		musicSource = GetComponent<AudioSource>();
		if (musicSource != null)
		{
			initialSourceVolume = musicSource.volume;
		}
	}

	public void Start()
	{
		PlayMusic(music);
	}

	public void Update()
	{
		if (musicSource != null)
		{
			musicSource.volume = Mathf.MoveTowards(musicSource.volume, initialSourceVolume, 2f * Time.deltaTime);
		}
	}

	public void PlayMusic(AudioClip clip)
	{
		if (musicSource != null)
		{
			musicSource.clip = clip;
			musicSource.Play();
			timeStartPlaying = Time.time;
			musicSource.volume = 0f;
		}
	}
}
