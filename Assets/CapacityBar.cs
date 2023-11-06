using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CapacityBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Text collected;

    private void Start()
    {
        _slider.maxValue = HayStack.instance.maxHayCapacity;
        collected.text = HayStack.instance.hayCollected.ToString() + " /" + HayStack.instance.maxHayCapacity.ToString();
    }

    private void OnEnable()
    {
        HayStack.OnHayCollect += UpdateCapacityBar;
    }
    private void OnDisable()
    {
        HayStack.OnHayCollect -= UpdateCapacityBar;
    }

    void UpdateCapacityBar(int value)
    {
        _slider.value = value;
        collected.text = HayStack.instance.hayCollected.ToString() + " /" + HayStack.instance.maxHayCapacity.ToString();

    }

    public void UpdateMaxCapacityUI()
    {
        collected.text = HayStack.instance.hayCollected.ToString() + " /" + HayStack.instance.maxHayCapacity.ToString();

    }
}
