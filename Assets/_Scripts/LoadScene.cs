using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadScene : MonoBehaviour
{
    public static LoadScene instance;
    [SerializeField] Image loadingFillBar;
    //public int sceneID;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    

    private IEnumerator Start()
    {

        loadingFillBar.DOFillAmount(1, 3).SetDelay(0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(4f);
        loadingFillBar.fillAmount = 0;
        gameObject.SetActive(false);
        //StartCoroutine(LoadSceneAsync(sceneID));

    }


    //IEnumerator LoadSceneAsync(int sceneID)
    //{
    //    yield return new WaitForSeconds(2f);
    //    //ES3AutoSaveMgr.Current.Save();
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
    //    while (!operation.isDone)
    //    {
    //        float fillValue = Mathf.Clamp01(operation.progress / 0.9f);
    //        loadingFillBar.fillAmount = fillValue;
    //        yield return null;
    //    }


    //}
}
