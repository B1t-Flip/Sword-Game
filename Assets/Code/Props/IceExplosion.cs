using System;
using UnityEngine;

public class IceExplosion : MonoBehaviour {
  private float timer = .5f;

  private void Update() {
    timer -= Time.deltaTime;
    transform.localScale = Vector3.one * Mathf.Sin(timer * 2);
    if(timer < 0) Destroy(gameObject);
  }
}
