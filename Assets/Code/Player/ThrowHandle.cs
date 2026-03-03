using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum StoneType {
  NONE, NORMAL, LAVA, ICE
}
public class ThrowHandle : MonoBehaviour {
  [SerializeField] private GameObject rockThrowable;
  [SerializeField] private Vector2 offset;
  [SerializeField] private float maxDistance, throwPower;
  [SerializeField] private int predictionStepCount;

  private Vector2 mouseWorldPos;
  private BoxCollider2D clickBounds;
  private SpriteRenderer rend;
  private StoneType stoneType;
  private LineRenderer prediction;
  private Rigidbody2D rockRB;

  public static bool holding;
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
      if (clicked != value && !value) OnLaunch();
      clicked = value;
    }
  }
  #endregion
  
  private void Start() {
    clickBounds = GetComponent<BoxCollider2D>();
    rend = GetComponent<SpriteRenderer>();
    rockRB = rockThrowable.GetComponent<Rigidbody2D>();
    prediction = GetComponent<LineRenderer>();
    prediction.positionCount = predictionStepCount;
    prediction.enabled = false;
    rend.enabled = false;
    clickBounds.enabled = false;
  }

  private GameObject original;
  public void PickSomethingUp(StoneType type, SpriteRenderer sprite) {
    prediction.enabled = true;
    rend.enabled = true;
    clickBounds.enabled = true;
    stoneType = type;
    rend.sprite = sprite.sprite;
    original = sprite.gameObject;
    holding = true;
  }

  private void Update() {
    if (!Clicked) {
      prediction.enabled = false;
      return;
    }
    Vector3 returnPos = transform.parent.position + (Vector3)offset;
    transform.position = mouseWorldPos;
    
    if (Vector2.Distance(transform.position, returnPos) > maxDistance) 
      transform.localPosition = (Vector3)offset + ((Vector3)mouseWorldPos - returnPos).normalized * maxDistance;
    
    if (transform.localPosition.y > offset.y)
      transform.localPosition = new Vector3(transform.localPosition.x, offset.y);
    Vector2 throwVel = ((Vector3)offset - transform.localPosition) * throwPower;
    prediction.enabled = true;
    prediction.SetPositions(Plot(rockRB, transform.parent.position + (Vector3)offset, throwVel, predictionStepCount));
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
  
  private void OnLaunch() {
    Vector2 throwDirection = (Vector3)offset - transform.localPosition;
    GameObject instance = Instantiate(rockThrowable);
    StoneThrowable throwable = instance.GetComponent<StoneThrowable>();
    throwable.reenable = original;
    throwable.type = stoneType;
    Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
    rb.linearVelocity = throwDirection * throwPower;
    rb.angularVelocity = Random.Range(-20f, 20f);
    transform.localPosition = offset;
    instance.transform.position = transform.position + (Vector3)throwDirection;
    prediction.enabled = false;
    rend.enabled = false;
    clickBounds.enabled = false;
    original = null;
    holding = false;
  }
}
