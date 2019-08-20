using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GodRays : MonoBehaviour {
  public GameObject ray1, ray2;
  public float duration = 7;

	// Use this for initialization
	void Start () {
    ray1.transform.localEulerAngles = new Vector3(0, 0, 180);
    ray2.transform.localEulerAngles = Vector3.zero;

    ray1.transform.DORotate(Vector3.zero, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    ray2.transform.DORotate(Vector3.forward * 180, duration * 1.25f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
  }
	
}
