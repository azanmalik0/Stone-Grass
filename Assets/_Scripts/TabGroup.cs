using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> objectsToSwap;

    public void Subscribe(TabButton button)
    {
        tabButtons ??= new List<TabButton>();
        objectsToSwap ??= new List<GameObject>();
        tabButtons.Add(button);
    }
    public void OnTabSelected(TabButton button)
    {
        HandleTabSwitching(button);

    }

    private void HandleTabSwitching(TabButton button)
    {
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
                objectsToSwap[i].SetActive(true);
            else
                objectsToSwap[i].SetActive(false);

        }
    }
}
