using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
  public static SoundManager instance;
  public GameObject soundPlayer;

  private void Awake() {
    if (!instance) instance = this;
    else Destroy(gameObject);
  }

  public static void PlaySound(AudioResource sound) {
    GameObject go = Instantiate(instance.soundPlayer, instance.transform);
    AudioSource player = go.GetComponent<AudioSource>();
    player.resource = sound;
    player.Play();
    instance.StartCoroutine(WaitForSoundToStop(player));
  }

  private static IEnumerator WaitForSoundToStop(AudioSource sound) {
    yield return new WaitUntil(() => !sound.isPlaying);
    Destroy(sound.gameObject);
  }
}
