using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
  private void Update() {
    transform.position = Vector3.Lerp(transform.position, new Vector3(
      PlayerController.instance.transform.position.x,
      PlayerController.instance.transform.position.y,
      -10), .25f);
  }
}
