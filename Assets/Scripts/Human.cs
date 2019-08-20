using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Human : MonoBehaviour {
  private NavMeshAgent agent;
  private Animator animator;

  private float radius = 2.5f;

  private void Awake () {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
  }

  private void Start () {
    transform.eulerAngles += Vector3.up * Random.Range(0, 360);
  }

  void Update () {
    animator.SetFloat("Velocity", agent.velocity.sqrMagnitude);

  }

  public void GoToDestination (Vector3 touchPosition) {
    if (GameManager.instance.numHumans != 1) { 
      Vector3 randomDirection = Random.insideUnitSphere * radius;
      Vector3 position = randomDirection + touchPosition;
      NavMeshHit hit;
      NavMesh.SamplePosition(position, out hit, radius, 1);
      Vector3 finalPosition = hit.position; 
      agent.SetDestination(finalPosition);
    } else { // if its only one human left dont randomize position
      agent.SetDestination(touchPosition);
    }
  }

  public void PickItem(Transform item) {
    animator.GetComponent<Animator>().SetTrigger("Pickup");
    item.parent = transform;

    Sequence pickAnimation = DOTween.Sequence();
    pickAnimation.Append(item.DOScale(0, 0.5f).SetEase(Ease.InBack));
    pickAnimation.Join(item.DOLocalMove(Vector3.up, 0.5f));
    pickAnimation.OnComplete(() => {
      Destroy(item.gameObject);
      GameManager.instance.addGem();
    });
  }

  private void OnCollisionEnter (Collision collision) {
    if (collision.gameObject.GetComponent<Mummy>()) {
      GameManager.instance.HumanDied(collision.contacts[0].point);

      animator.SetTrigger("Die");
      agent.isStopped = true;
      agent.ResetPath();
      enabled = false;
      GetComponent<Collider>().enabled = false;
      transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetDelay(2).OnComplete(()=> {
        GameManager.instance.explosionParticle.transform.position = transform.position;
        GameManager.instance.explosionParticle.Play();
        Destroy(this.gameObject);
      });
    }
  }
}
