using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PopUp : MonoBehaviour {

  public Image background;
  public Transform content;

  public void Show (float delay = 0) {
    gameObject.SetActive(true);
    Sequence seq = DOTween.Sequence();
    seq.AppendInterval(delay);
    seq.Append(background.DOFade(0, 0.5f).From());
    seq.Join(content.transform.DOScale(0, 0.5f).SetEase(Ease.OutBack).From());
  }

}
