using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;
    public SpecialVibrationTypes SpecialVibrationTypes;
    private void Awake()
    {
        Instance = this;
        Vibration.Init();
    }

    public static void Vibrate()
    {
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            Vibration.Vibrate();
        }

    }
    public static void Vibrate(long ms)
    {
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            Vibration.VibrateAndroid(ms);
        }

    }
    public static void SpecialVibrate(SpecialVibrationTypes type)
    {
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            if (type == SpecialVibrationTypes.Peek)
                Vibration.VibratePeek();
            if (type == SpecialVibrationTypes.Nope)
                Vibration.VibrateNope();
            if (type == SpecialVibrationTypes.Pop)
                Vibration.VibratePop();
        }
    }
}
public enum SpecialVibrationTypes { Peek, Nope, Pop }
