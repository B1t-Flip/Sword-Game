using System;
using UnityEngine;

public class StoneRoamer : MonoBehaviour {
  [SerializeField] private float start, end, speed;
  public StoneType type;
  private Rigidbody2D rb;
  private bool loc;

  private void Start() {
    rb = GetComponent<Rigidbody2D>();
  }

  private void OnDrawGizmos() {
    Gizmos.DrawLine(
      new Vector2(start, transform.position.y),
      new Vector2(end, transform.position.y));
    Gizmos.DrawSphere(new Vector2(start, transform.position.y), .1f);
    Gizmos.DrawSphere(new Vector2(end, transform.position.y), .1f);
  }

  private void FixedUpdate() {
    float y = rb.linearVelocity.y;
    rb.linearVelocity = (new Vector2(loc ? end : start, transform.position.y) - (Vector2)transform.position).normalized * speed;
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
    if (Vector2.Distance(transform.position, new Vector2(loc ? end : start, transform.position.y)) < 0.1f) loc = !loc;
  }
}
