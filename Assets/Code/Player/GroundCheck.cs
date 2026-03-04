using System;
using UnityEngine;
using UnityEngine.Audio;

public class GroundCheck : MonoBehaviour {
  private Transform playerTransform;
  [SerializeField] private Vector3 offset;
  [SerializeField] private AudioResource bonkSound;

  private bool playedSound;
  private void Start() {
    playerTransform = PlayerController.instance.transform;
  }

  private void OnTriggerStay2D(Collider2D other) {
    if (!other.gameObject.CompareTag("Ground")) return;
    PlayerController.grounded = true;
    if (playedSound) return;
    playedSound = true;
    SoundManager.PlaySound(bonkSound);
  }
  private void OnTriggerExit2D(Collider2D other) {
    if (!other.gameObject.CompareTag("Ground")) return;
    playedSound = false;
    PlayerController.grounded = false;
  }

  private void Update() {
    transform.position = playerTransform.position + offset;
  }
}
