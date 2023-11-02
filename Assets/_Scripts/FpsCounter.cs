using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fps;
    string label = "";
    float count;
    private void Awake()
    {
        Application.targetFrameRate=60;
    }

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            count = (1 / Time.deltaTime);
            label = "FPS :" + (Mathf.Round(count));
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnGUI()
    {
        fps.text = label.ToString();
    }
}
