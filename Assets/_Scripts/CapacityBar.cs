using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CapacityBar : MonoBehaviour
{
    public static CapacityBar Instance;
    public Slider _slider;
    [SerializeField] Text collected;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        HayStack.OnHayCollect += UpdateCapacityBar;

    }
    private void OnDisable()
    {
        HayStack.OnHayCollect -= UpdateCapacityBar;

    }

    public void UpdateCapacityBar(int value)
    {
        _slider.value = value;
        collected.text = HayStack.instance.totalHayCollected.ToString() + "/" + HayStack.instance.maxHayCapacity.ToString();

    }

    public void UpdateMaxCapacityUI()
    {
        collected.text = HayStack.instance.totalHayCollected.ToString() + "/" + HayStack.instance.maxHayCapacity.ToString();

    }
}
