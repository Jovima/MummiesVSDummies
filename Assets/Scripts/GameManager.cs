using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager instance;

  [Header("Scene References")]
  public CameraController cameraController;
  [SerializeField] private Transform gemsContainer;
  [SerializeField] private GameHUD gameHUD;
  [SerializeField] private MenuManager menuManager;
  [SerializeField] private HumansController humansController;
  [SerializeField] private PopUp endGamePopUp, GameCompletedPopUp;

  [Header("Shared Particles")]
  public ParticleSystem hitParticle;
  public ParticleSystem explosionParticle;

  [HideInInspector] public int numGems = 0;
  public int gemsObtained { get; private set; } = 0;
  public int numHumans { get; private set; } = 0;

  private static bool firstRun = true;

  void Awake () {
    instance = this;
  }

  private void Start () {
    if (firstRun) {
      humansController.enabled = false;
      menuManager.StartAnimation();
      cameraController.PatrolAnimation();
    } else {
      StartGame();
    }
  }

  public void StartGame () {
    numGems = gemsContainer.childCount;
    numHumans = humansController.humans.Length;
    gameHUD.Init(numHumans, numGems);
    humansController.enabled = true;
    cameraController.setGameCamera();
    if (firstRun) menuManager.StartGame();
    else menuManager.StartGameDirectly();
    firstRun = false;
  }

  public void addGem () {
    gemsObtained++;
    gameHUD.AddGem();
    if(gemsObtained == numGems) {
      GameCompleted();
    }
  }

  public void HumanDied (Vector3 targetPosition) {
    cameraController.DramaticAnimation(targetPosition);
    hitParticle.transform.position = targetPosition;
    instance.hitParticle.Play();

    numHumans--;
    gameHUD.HumanDied();
    if(numHumans <= 0) {
      EndGame();
    }
  }

  public void EndGame () {
    endGamePopUp.Show(0.5f);
  }

  public void GameCompleted () {
    GameCompletedPopUp.Show();
  }

  public void ExitGame () {
    Application.Quit();
  }

  public void Replay () {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}
