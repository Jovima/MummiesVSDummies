using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour {
  [Header("UI Panels")]
  [SerializeField] private Transform menuPanel;
  [SerializeField] private Transform gamePanel;

  [Header("Menu References")]
  [SerializeField] private Transform logo;
  [SerializeField] private Transform mummieLogo, vsLogo, dummiesLogo, sunRays, tutorial, startButton;

  public void StartAnimation () {
    Sequence animation = DOTween.Sequence();
    animation.Append(mummieLogo.DOScale(0, 0.5f).From().SetEase(Ease.OutBack));
    animation.Append(vsLogo.DOScale(0, 0.5f).From().SetEase(Ease.OutBack));
    animation.Append(dummiesLogo.DOScale(0, 0.5f).From().SetEase(Ease.OutBack));
    animation.Append(sunRays.DOScale(0, 0.5f).From().SetEase(Ease.OutFlash));
    animation.AppendInterval(1);
    animation.Append(logo.DOMove(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f), 1).From());
    animation.Append(tutorial.DOScaleY(0, 0.25f).From());
    animation.Append(startButton.DOScale(0, 0.5f).From().SetEase(Ease.OutBack));
  }

  public void StartGame () { 
    Sequence playAnim = DOTween.Sequence();
    playAnim.Append(logo.DOMoveY(Screen.height * 1.25f, 0.5f));
    playAnim.Join(tutorial.DOScale(0, 0.25f).SetEase(Ease.InBack));
    playAnim.Join(startButton.DOScale(0, 0.25f));
    playAnim.OnComplete(() => {
      menuPanel.gameObject.SetActive(false);
      gamePanel.gameObject.SetActive(true);
      gamePanel.DOScale(2, 1f).From();
    });
  }

  public void StartGameDirectly () {
    menuPanel.gameObject.SetActive(false);
    gamePanel.gameObject.SetActive(true);
    gamePanel.DOScale(2, 1f).From();
  }


}
