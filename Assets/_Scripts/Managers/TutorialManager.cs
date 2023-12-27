using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    [SerializeField] GameObject tutorialParent;
    [SerializeField] RectTransform tutorialHand;
    [SerializeField] CanvasGroup tutorialArrowCanvasGroup;
    [SerializeField] Image[] tutorialArrowImages;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (PlayerPrefs.GetInt("TutorialShown") == 0 && LevelMenuManager.Instance.currentLevel == 0)
        {
            tutorialParent.SetActive(true);
            EnableTutorial();
        }
    }

    void EnableTutorial()
    {
        if (PlayerPrefs.GetInt("TutorialHandShown") == 0)
        {
            tutorialHand.gameObject.SetActive(true);
            Sequence seq = DOTween.Sequence();
            seq.Append(tutorialHand.DOAnchorPosY(503, 1.5f).SetEase(Ease.Linear))
                .Insert(1f, tutorialHand.gameObject.GetComponent<Image>().DOFade(0, 0.3f).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Restart);
        }
        if (PlayerPrefs.GetInt("TutorialArrowsShown") == 0)
        {
            tutorialArrowCanvasGroup.gameObject.SetActive(true);
            //tutorialArrowCanvasGroup.DOFade(0.5f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            for (int i = 0; i < tutorialArrowImages.Length; i++)
            {
                tutorialArrowImages[i].DOFade(0f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            }

        }
    }

    public void DisableTutorialHand()
    {
        if (PlayerPrefs.GetInt("TutorialHandShown") == 0)
        {
            tutorialHand.gameObject.SetActive(false);
            PlayerPrefs.SetInt("TutorialHandShown", 1);
        }
    }
    public void DisableTutorialArrows()
    {
        if (PlayerPrefs.GetInt("TutorialArrowsShown") == 0)
        {
            tutorialArrowCanvasGroup.gameObject.SetActive(false);
            PlayerPrefs.SetInt("TutorialArrowsShown", 1);
        }
        if (PlayerPrefs.GetInt("TutorialShown") == 0)
        {
            tutorialParent.SetActive(false);
            PlayerPrefs.SetInt("TutorialShown", 1);
        }
    }


}
