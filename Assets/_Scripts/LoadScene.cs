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
    public int sceneID;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    private void Start()
    {
        //ES3AutoSaveMgr.Current.Load();
        StartCoroutine(LoadSceneAsync(sceneID));

    }


    IEnumerator LoadSceneAsync(int sceneID)
    {
        yield return new WaitForSeconds(2f);
        ES3AutoSaveMgr.Current.Save();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        while (!operation.isDone)
        {
            float fillValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingFillBar.fillAmount = fillValue;
            yield return null;
        }


    }
}
