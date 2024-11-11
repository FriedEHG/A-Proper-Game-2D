using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] AudioClip musicClip1;
	bool hasPlayed1;
	[SerializeField] AudioClip musicClip2;
	bool hasPlayed2;
	[SerializeField] AudioClip musicClip3;
	bool hasPlayed3;
	[SerializeField] AudioClip musicClip4;
	bool hasPlayed4;

	AudioClip[] musicClips;

	AudioSource audioSource;

	[SerializeField] InputActionAsset JukeboxActionsAsset;
	InputActionMap JukeboxActionMap;
	InputAction Select1Action;
	InputAction Select2Action;
	InputAction Select3Action;
	InputAction Select4Action;
	InputAction ShuffleAction;
	InputAction PauseAction;
	InputAction VolumeControl;


	private void OnEnable()
	{
		JukeboxActionsAsset.Enable();
	}

	private void OnDisable()
	{
		JukeboxActionsAsset.Disable();
	}

	void Start()
	{//Debug.Log("AudioSource Start Methoid");

		MusicPlayer[] musicPlayers = FindObjectsOfType<MusicPlayer>();
		if (musicPlayers.Length > 1)
		{
			Destroy(gameObject);
		}

		ControlsInitialize();
		VariablesInitialize();
	}
	void ControlsInitialize()
	{
		JukeboxActionMap = JukeboxActionsAsset.FindActionMap("JukeBoxMap");
		Select1Action = JukeboxActionMap.FindAction("Select1");
		Select2Action = JukeboxActionMap.FindAction("Select2");
		Select3Action = JukeboxActionMap.FindAction("Select3");
		Select4Action = JukeboxActionMap.FindAction("Select4");
		ShuffleAction = JukeboxActionMap.FindAction("Shuffle");
		PauseAction = JukeboxActionMap.FindAction("Pause");
		VolumeControl = JukeboxActionMap.FindAction("VolumeControl");
	}
	void VariablesInitialize()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.loop = false;

		hasPlayed1 = false;
		hasPlayed2 = false;
		hasPlayed3 = false;
		hasPlayed4 = false;

		musicClips = new AudioClip[4];
		musicClips[0] = musicClip1;
		musicClips[1] = musicClip2;
		musicClips[2] = musicClip3;
		musicClips[3] = musicClip4;
	}

	void Update()
	{
		AutomaticSongChangeUpdate();
		ManualSongChangeUpdate();
	}
	void AutomaticSongChangeUpdate()
	{
		if (audioSource != null)
		{
			if (!audioSource.isPlaying)
			{   //The sudiosource doesn't loop, so if it is not playing that means we have hit the end of the current song and need a new one.
				RandomNewSong();
			}
		}
	}
	void ManualSongChangeUpdate()
	{
		if (Select1Action.WasPressedThisFrame())
		{
			SelectSong(1);
		}
		else if (Select2Action.WasPressedThisFrame())
		{
			SelectSong(2);
		}
		else if (Select3Action.WasPressedThisFrame())
		{
			SelectSong(3);
		}
		else if (Select4Action.WasPressedThisFrame())
		{
			SelectSong(4);
		}
	}

	void RandomNewSong()
	{
		int random = Mathf.RoundToInt(Random.Range(0.51f, 4.49f));  //Subtract 0.49 from min and add 0.49 to max in order to have more even random distribution

		if (hasPlayed1 && hasPlayed2 && hasPlayed3 && hasPlayed4)
		{
			hasPlayed1 = false;
			hasPlayed2 = false;
			hasPlayed3 = false;
			hasPlayed4 = false;
		}

		if (random == 1 && !hasPlayed1)     //in the future we could optimize this by making song Objects, with the audioclip and bool hasPlayed
		{
			audioSource.clip = musicClip1;
			hasPlayed1 = true;
		}
		else if (random == 2 && !hasPlayed2)
		{
			audioSource.clip = musicClip2;
			hasPlayed2 = true;
		}
		else if (random == 3 && !hasPlayed3)
		{
			audioSource.clip = musicClip3;
			hasPlayed3 = true;
		}
		else if (random == 4 && !hasPlayed4)
		{
			audioSource.clip = musicClip4;
			hasPlayed4 = true;
		}
		audioSource.Play();

		Debug.Log($"Song: {random}");
	}
	void SelectSong(int songNumb)
	{
		Debug.Log("Selected " + songNumb);
		Cleanse();
		audioSource.clip = musicClips[songNumb - 1];
		audioSource.Play();
	}
	void Shuffle()
	{
		Cleanse();
		RandomNewSong();
	}
	void Cleanse()
	{
		hasPlayed1 = false;
		hasPlayed2 = false;
		hasPlayed3 = false;
		hasPlayed4 = false;
	}
}
