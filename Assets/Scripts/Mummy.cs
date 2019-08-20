using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SensorToolkit;
using DG.Tweening;

public class Mummy : MonoBehaviour {

  // FSM
  private enum State {
    IDLE,
    PATROL,
    CHASE
  }

  public Transform path;
  
  [SerializeField] private TriggerSensor triggerSensor;
  [SerializeField] private ParticleSystem alertParticle;

  private NavMeshAgent agent;
  private Animator animator;

  private int step = 0;
  private float idleTime = 0;
  private float waitTime = 2;
  private State state = State.PATROL;

  private void Awake () {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
  }

  void Start () {
    transform.position = path.GetChild(0).transform.position;
  }
  
  void Update () {
    animator.SetFloat("Velocity", agent.velocity.sqrMagnitude);

    switch (state) {
      case State.IDLE:
        FSM_Idle();
        break;
      case State.PATROL:
        FSM_Patrol();
        break;
      case State.CHASE:
        FSM_Chase();
        break;
      default:
        break;
    }

  }

  // Wait x seconds to continue patrol
  private void FSM_Idle () {
    if (Time.time > idleTime + waitTime) {
      state = State.PATROL;
    }
  }

  // Follow path and check if human is in range to follow
  private void FSM_Patrol () {
    if (HasReachedDestination()) {
      step++;
      if (step >= path.childCount) step = 0;

      agent.SetDestination(path.GetChild(step).transform.position);
    }

    if(triggerSensor.GetDetected().Count != 0) {
      Human human = triggerSensor.GetDetected()[0].GetComponent<Human>();
      if (human) {
        agent.SetDestination(human.transform.position);
        alertParticle.Play();
        state = State.CHASE;
      }
    }

  }

  // Goes to the human first seen position
  private void FSM_Chase () {
    if (HasReachedDestination()) {
      state = State.IDLE;
      idleTime = Time.time;
      alertParticle.Stop();
    }
  }

  private bool HasReachedDestination () {
    if (!agent.enabled) return false;

    return agent.remainingDistance <= agent.stoppingDistance;
  }
}
