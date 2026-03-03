using System;
using UnityEngine;

public class Explosion : MonoBehaviour {
  private float timer = .5f;

  private void Update() {
    timer -= Time.deltaTime;
    transform.localScale = Vector3.one * Mathf.Sin(timer * 2);
    if(timer < 0) Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.TryGetComponent(out Wood wood)) wood.burning = true;
  }
}
