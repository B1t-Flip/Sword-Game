using System;
using UnityEngine;

public class Wood : MonoBehaviour {
  private ParticleSystem.EmissionModule burnParticles;
  public bool burning;

  private float timer = 1f;

  private void Start() {
    burnParticles = GetComponent<ParticleSystem>().emission;
    burnParticles.enabled = false;
  }

  private void Update() {
    if (!burning) return;
    timer -= Time.deltaTime;
    burnParticles.enabled = true;
    if (timer < 0) Destroy(gameObject);
  }


  private void OnTriggerStay2D(Collider2D other) {
    if (burning && timer < .5f && other.TryGetComponent(out Wood wood)) wood.burning = true;
  }
}
