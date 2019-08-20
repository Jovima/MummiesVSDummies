using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gem : MonoBehaviour {
  [SerializeField] private Transform gemModel;
  private float animYOffset = 0.25f;

  // Start is called before the first frame update
  void Start () {
    Sequence idleAnim = DOTween.Sequence();
    transform.DOLocalRotate(new Vector3(-90, 0, 180), 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    transform.DOLocalMoveY(transform.position.y + animYOffset, 2).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);

  }

  private void OnTriggerEnter (Collider other) {
    Human human = other.GetComponent<Human>();
    if (human) {
      GetComponent<Collider>().enabled = false;
      human.PickItem(this.transform);
      GameManager.instance.cameraController.DramaticAnimation(human.transform.position);
    }
  }
}
