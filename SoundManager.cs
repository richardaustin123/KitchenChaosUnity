using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour {

    private const string PLAYER_REFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsRefsSO audioClipsRefsSO;

    private float volume = 1f;

    // Awake()
    private void Awake() {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_REFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    // Start()
    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed  += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    // DeliveryManager_OnRecipeSuccess()
    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    // DeliveryManager_OnRecipeFailed()
    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    // CuttingCounter_OnAnyCut()
    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipsRefsSO.chop, cuttingCounter.transform.position);
    }

    // Player_OnPickedSomething()
    private void Player_OnPickedSomething(object sender, System.EventArgs e) {
        PlaySound(audioClipsRefsSO.objectPickup, Player.Instance.transform.position);
    }

    // BaseCounter_OnAnyObjectPlacedHere()
    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipsRefsSO.objectdrop, baseCounter.transform.position);
    }

    // TrashCounter_OnAnyObjectTrashed()
    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipsRefsSO.trash, trashCounter.transform.position);
    }

    // PlaySound()
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    // PlaySound()
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    // PlayFootstepsSound()
    public void PlayFootstepsSound(Vector3 position, float volume) {
        PlaySound(audioClipsRefsSO.footstep, position, volume);
    }

    // PlayCountdownSound()
    public void PlayCountdownSound() {
        PlaySound(audioClipsRefsSO.warning, Vector3.zero);
    }

    // PlayWarningSound()
    public void PlayWarningSound(Vector3 position) {
        PlaySound(audioClipsRefsSO.warning, position);
    }

    // ChangeVolume()
    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1f) {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_REFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // GetVolume()
    public float GetVolume() {
        return volume;
    }

}
