using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour {
  public TextMeshProUGUI fpsText;
  private float deltaTime = 0.0f;

  private void Awake () {
    fpsText = GetComponent<TextMeshProUGUI>();
  }

  void Update () {
    deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

    float msec = deltaTime * 1000.0f;
    float fps = 1.0f / deltaTime;
    string text = string.Format("{0:0.} FPS - {1:0.0} ms ", fps, msec);
    fpsText.text = text;

  }
}
