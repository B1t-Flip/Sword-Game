using System;
using UnityEngine;

public class Water : MonoBehaviour {
  [SerializeField] private GameObject platform, iceExplosion;

  private void OnTriggerEnter2D(Collider2D other) {
    if (!other.TryGetComponent(out StoneThrowable throwable) || throwable.type != StoneType.ICE) return;
    Instantiate(platform, other.transform.position, Quaternion.identity);
    Instantiate(iceExplosion, other.transform.position, Quaternion.identity);
    throwable.reenable.SetActive(true);
    Destroy(other.gameObject);
  }
}
