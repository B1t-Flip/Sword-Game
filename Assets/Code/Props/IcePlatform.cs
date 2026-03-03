using System;
using UnityEngine;

public class IcePlatform : MonoBehaviour {
  [SerializeField] private float speed;
  private void OnTriggerStay2D(Collider2D other) {
    if(other.TryGetComponent(out Water _)) transform.position += Vector3.up * speed;
  }
}
