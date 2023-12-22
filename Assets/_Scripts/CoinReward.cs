using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CoinReward : MonoBehaviour
{
    [SerializeField] private GameObject pileOfCoins;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;
    AudioManager AM;


    private void OnEnable()
    {
        ProgressBarManager.OnFirstStarUnlock += AnimateFirstStarCoinReward;
        ProgressBarManager.OnThirdStarUnlock += AnimateThirdStarCoinReward;
    }
    private void OnDisable()
    {
        ProgressBarManager.OnFirstStarUnlock -= AnimateFirstStarCoinReward;
        ProgressBarManager.OnThirdStarUnlock -= AnimateThirdStarCoinReward;

    }
    void Start()
    {
        AM = AudioManager.instance;
        if (coinsAmount == 0)
            coinsAmount = 7;

        initialPos = new Vector2[coinsAmount];
        initialRotation = new Quaternion[coinsAmount];

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            initialPos[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            initialRotation[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation;
        }

    }
    public void AnimateThirdStarCoinReward(int coins)
    {
        if (PlayerPrefs.GetInt($"SecondCoinsAnimated{LevelMenuManager.Instance.currentLevel}") == 0)
        {
            pileOfCoins.SetActive(true);
            AM.Play("GoldSack");
            var delay = 0f;

            for (int i = 0; i < pileOfCoins.transform.childCount; i++)
            {
                pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
                pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(682, 1351f), 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack);
                pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
                pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);
                if (i == pileOfCoins.transform.childCount - 1)
                {
                    ResetValues();
                }
                delay += 0.1f;
            }

            StartCoroutine(CountDollars(coins));
            PlayerPrefs.SetInt($"SecondCoinsAnimated{LevelMenuManager.Instance.currentLevel}", 1);
        }
    }
    public void AnimateFirstStarCoinReward(int coins)
    {
        if (PlayerPrefs.GetInt($"FirstCoinsAnimated{LevelMenuManager.Instance.currentLevel}") == 0)
        {
            pileOfCoins.SetActive(true);
            AM.Play("GoldSack");
            var delay = 0f;

            for (int i = 0; i < pileOfCoins.transform.childCount; i++)
            {
                pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
                pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(682f, 1351f), 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack);
                pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
                pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);
                if (i == pileOfCoins.transform.childCount - 1)
                {
                    ResetValues();
                }
                delay += 0.1f;
            }

            StartCoroutine(CountDollars(coins));
            PlayerPrefs.SetInt($"FirstCoinsAnimated{LevelMenuManager.Instance.currentLevel}", 1);
        }
    }
    private void ResetValues()
    {
        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = initialPos[i];
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation = initialRotation[i];
        }
    }
    IEnumerator CountDollars(int coins)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        CurrencyManager.Instance.RecieveCoins(coins);
    }
    
}
