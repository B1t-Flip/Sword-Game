using System;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour {
  [SerializeField] private UnityEvent OnPressed;

  private void OnTriggerEnter2D(Collider2D other) {
    if(other.TryGetComponent(out StoneThrowable _))
      OnPressed.Invoke();
  }
}
