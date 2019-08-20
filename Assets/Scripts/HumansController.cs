using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumansController : MonoBehaviour {
  public ParticleSystem touchFeedback;
  [HideInInspector] public Human[] humans;

  private Camera mainCamera;
  private int layerMask = 1 << 9;
  private float deltaOffset = 0.5f / Screen.width;

  private void Awake () {
    mainCamera = Camera.main;
    humans = FindObjectsOfType<Human>();
  }

  void Update () {
    if (Input.touchCount == 1 && Input.GetTouch(0).phase.Equals(TouchPhase.Ended)) {
      if (Vector2.SqrMagnitude(Input.GetTouch(0).deltaPosition) < 0.5f) {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
          foreach (Human human in humans) {
            if(human && human.enabled) human.GoToDestination(hit.point);
          }
          touchFeedback.transform.position = hit.point;
          touchFeedback.Play();
        }
      }
    }

  }

}
