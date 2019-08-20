using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameHUD : MonoBehaviour {
  public TextMeshProUGUI gemsCounter;
  public Image gemsImage;
  public Transform swordsContainer;

  private int totalGems;

  public void Init(int numHumans, int numGems) {
    for (int i = 1; i < numHumans; i++) {
      Instantiate(swordsContainer.GetChild(0).gameObject, swordsContainer);
    }

    totalGems = numGems;
    gemsCounter.text = string.Format("00/{0}", totalGems.ToString());

  }

  public void AddGem () {
    gemsCounter.text = GameManager.instance.gemsObtained.ToString();
    gemsCounter.text = string.Format("{0}/{1}", GameManager.instance.gemsObtained.ToString("00"), GameManager.instance.numGems.ToString());
    gemsImage.transform.DOScale(1.25f, 0.25f).From();
  }

  public void HumanDied () {
    Transform sword = swordsContainer.GetChild(swordsContainer.childCount - 1);
    sword.transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(()=> {
      Destroy(sword.gameObject);
    });
  }
}
