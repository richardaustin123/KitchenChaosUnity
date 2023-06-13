using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour {

    private const string PLAYER_REFS_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private float volume = 0.3f;

    // Awake()
    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_REFS_MUSIC_VOLUME, 0.3f);
        audioSource.volume = volume;
    }

    // ChangeVolume()
    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1f) {
            volume = 0f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_REFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // GetVolume()  
    public float GetVolume() {
        return volume;
    }

}
