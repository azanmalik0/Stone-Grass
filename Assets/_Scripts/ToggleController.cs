using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{

    enum ToggleType { Sound, Vibration }
    [SerializeField] ToggleType toggleType;
    public string playerPref;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    Toggle toggle;
    Image imageComponent;
    AudioManager AM;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        toggle = GetComponent<Toggle>();
        toggle.isOn = PlayerPrefs.GetInt(playerPref, 1) == 1 ? true : false;
    }

    private void Start()
    {
        AM = AudioManager.instance;
    }
    public void OnSwitch(bool on)
    {
        switch (toggleType)
        {
            case ToggleType.Sound:
                if (on)
                {
                    AM.Play("Pop");
                    imageComponent.sprite = onSprite;
                    PlayerPrefs.SetInt(playerPref, 1);
                    AudioListener.pause = false;
                }
                else
                {
                    imageComponent.sprite = offSprite;
                    PlayerPrefs.SetInt(playerPref, 0);
                    AudioListener.pause = true;
                }
                break;
            case ToggleType.Vibration:
                if (on)
                {
                    Handheld.Vibrate();
                    imageComponent.sprite = onSprite;
                    PlayerPrefs.SetInt(playerPref, 1);
                }
                else
                {
                    imageComponent.sprite = offSprite;
                    PlayerPrefs.SetInt(playerPref, 0);
                }
                break;


        }




    }


}
