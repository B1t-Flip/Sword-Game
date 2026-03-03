using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
  private Transform playerTransform;
  [SerializeField] private Vector3 offset;

  private void Start() {
    playerTransform = PlayerController.instance.transform;
  }

  private void OnTriggerStay2D(Collider2D other) {
    if (!other.gameObject.CompareTag("Ground")) return;
    PlayerController.grounded = true;
  }
  private void OnTriggerExit2D(Collider2D other) {
    if (!other.gameObject.CompareTag("Ground")) return;
    PlayerController.grounded = false;
  }

  private void Update() {
    transform.position = playerTransform.position + offset;
  }
}
