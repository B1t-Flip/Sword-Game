using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevel : MonoBehaviour {
  [SerializeField] private GameObject winScreen;
  private void OnTriggerEnter2D(Collider2D other) {
    if (!other.CompareTag("Player")) return;
    if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    else winScreen.SetActive(true);
  }
}
