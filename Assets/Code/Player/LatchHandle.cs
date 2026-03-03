using UnityEngine;
using UnityEngine.InputSystem;

public class LatchHandle : MonoBehaviour {
  [SerializeField] private Vector2 offset;
  [SerializeField] private float maxDistance, throwPower;
  [SerializeField] private int predictionStepCount;

  private Vector2 mouseWorldPos;
  private BoxCollider2D clickBounds;
  private SpriteRenderer rend;
  private StoneType stoneType;
  private LineRenderer prediction;

  #region Input
  [SerializeField] private InputActionReference mouseMove, mouseClick;
  private void OnMouseMoved(InputAction.CallbackContext ctx) =>
    mouseWorldPos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
  private void OnMouseClicked(InputAction.CallbackContext ctx) =>
    Clicked = ctx.performed && clickBounds.bounds.Contains(mouseWorldPos);
  
  private void OnEnable() {
    mouseMove.action.performed += OnMouseMoved;
    mouseClick.action.performed += OnMouseClicked;
    mouseClick.action.canceled += OnMouseClicked;
    transform.localPosition = offset;
  }
  private void OnDisable() {
    mouseMove.action.performed -= OnMouseMoved;
    mouseClick.action.performed -= OnMouseClicked;
    mouseClick.action.canceled -= OnMouseClicked;
  }

  private bool clicked;
  private bool Clicked {
    get => clicked;
    set {
      if (clicked != value && !value) OnLaunch(PlayerController.rb);
      clicked = value;
    }
  }
  #endregion
  
  private void Start() {
    clickBounds = GetComponent<BoxCollider2D>();
    rend = GetComponent<SpriteRenderer>();
    prediction = GetComponent<LineRenderer>();
    prediction.positionCount = predictionStepCount;
    prediction.enabled = false;
  }

  private void Update() {
    if (!Clicked) {
      prediction.enabled = false;
      return;
    }
    transform.position = mouseWorldPos;
    if (Vector2.Distance(transform.position, transform.parent.TransformPoint(offset)) > maxDistance)
      transform.position = transform.parent.TransformPoint(offset) + 
                           (transform.position - transform.parent.TransformPoint(offset)).normalized * maxDistance;
    if (transform.localPosition.y > 0)
      transform.localPosition = new Vector3(transform.localPosition.x, 0);
    
    if (transform.localPosition.y < offset.y)
      transform.localPosition = new Vector3(transform.localPosition.x, offset.y);
    Vector2 throwVel = (transform.parent.TransformPoint(offset) - transform.position) * throwPower;
    prediction.enabled = true;
    prediction.SetPositions(Plot(PlayerController.rb, transform.TransformPoint(offset), throwVel, predictionStepCount));
  }

  private Vector3[] Plot(Rigidbody2D rb, Vector2 pos, Vector2 vel, int steps) {
    Vector3[] result = new Vector3[steps];

    float timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;
    Vector2 gravityAccel = Physics2D.gravity * (rb.gravityScale * timeStep * timeStep);

    float drag = 1f - timeStep * rb.linearDamping;
    Vector2 moveStep = vel * timeStep;

    for (int i = 0; i < steps; i++) {
      moveStep += gravityAccel;
      moveStep *= drag;
      pos += moveStep;
      result[i] = pos;
    }
    
    return result;
  }
  
  private void OnLaunch(Rigidbody2D player) {
    Vector2 throwDirection = (transform.parent.TransformPoint(offset) - transform.position) * throwPower;
    player.linearVelocity = throwDirection;
    gameObject.SetActive(false);
    PlayerController.instance.WallUnstick();
  }
}
