using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour {
  [SerializeField] private Transform animPath;

  private float yRotation;
  private float cameraSpeedFactor = 10f;
  private Vector2 screenSize;
  private Camera cam;
  private bool enableTouch = false;
  private bool isAnimationPlaying = false;
  private Vector3 startPosition, startRotation;
  private Sequence patrolAnim;

  private Vector3 delta;
  private Vector3 lastPos;

  private void Awake () {
    cam = GetComponent<Camera>();
  }

  void Start () {
    yRotation = transform.eulerAngles.y;
    startPosition = transform.position;
    startRotation = transform.eulerAngles;
    screenSize = new Vector2(Screen.width, Screen.height);
  }
  
  void Update () {
    if (!enableTouch) return;

    if (!isAnimationPlaying && Input.touchCount == 1 && Input.GetTouch(0).phase.Equals(TouchPhase.Moved)) {
      moveHumans(Input.GetTouch(0).deltaPosition);
    }

    if (Input.GetMouseButtonDown(1)) {
      lastPos = Input.mousePosition;
    } else if (Input.GetMouseButton(1)) {
      delta = Input.mousePosition - lastPos;

      moveHumans(delta);

      lastPos = Input.mousePosition;
    }
  }

  private void moveHumans (Vector2 deltaPosition) {
    deltaPosition = new Vector2(deltaPosition.x / screenSize.x, deltaPosition.y / screenSize.y) * cameraSpeedFactor;
    Vector3 deltaPosFixed = new Vector3(deltaPosition.x, 0, deltaPosition.y);
    deltaPosFixed = Quaternion.Euler(0, yRotation, 0) * deltaPosFixed;
    transform.position -= deltaPosFixed;
  }

  // Camera animation in first menu
  public void PatrolAnimation () {
    patrolAnim = DOTween.Sequence();
    transform.position = animPath.GetChild(0).position;
    transform.eulerAngles = animPath.GetChild(0).eulerAngles;

    patrolAnim.Append(transform.DOMove(animPath.GetChild(1).position, 13));
    patrolAnim.AppendCallback(() => transform.position = animPath.GetChild(2).position);
    patrolAnim.Append(transform.DOMove(animPath.GetChild(3).position, 13));
    patrolAnim.SetLoops(-1, LoopType.Restart);
  }

  public void setGameCamera () {
    patrolAnim.Kill();
    transform.DOMove(startPosition, 0.5f);
    transform.DORotate(startRotation, 0.5f);
    enableTouch = true;
  }

  // Animation for slowmo effects
  public void DramaticAnimation (Vector3 target) {
    if (isAnimationPlaying) return;

    isAnimationPlaying = true;
    Sequence animation = DOTween.Sequence();
    animation.Append(cam.DOFieldOfView(20, 0.25f));
    animation.Join(transform.DOLookAt(target, 0.25f));
    animation.Join(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.25f, 0.5f));
    animation.AppendInterval(1);
    animation.Join(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.5f));
    animation.Join(cam.DOFieldOfView(cam.fieldOfView, 0.25f));
    animation.Join(transform.DORotate(transform.eulerAngles, 0.25f));
    animation.OnComplete(() => isAnimationPlaying = false);
  }
}
