using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{

    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    Toggle toggle;
    Image imageComponent;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        toggle = GetComponent<Toggle>();
        toggle.isOn = PlayerPrefs.GetInt("Vibration") == 1 ? true : false;



    }
    public void OnSwitch(bool on)
    {

        if (on)
        {
            imageComponent.sprite = onSprite;
            PlayerPrefs.SetInt("Vibration", 1);
        }
        else
        {
            imageComponent.sprite = offSprite;
            PlayerPrefs.SetInt("Vibration", 0);
        }

    }




}

