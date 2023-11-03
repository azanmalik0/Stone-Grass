using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgradeManager : MonoBehaviour
{
    [SerializeField] Image UpgradePanel;
    [SerializeField] GameObject sawStage1;
    [SerializeField] GameObject sawStage2;

    private void OnEnable()
    {
        Upgrade.OnEnteringUpgradeZone += OpenUpgradePanel;
    }

    public void OnButtonClick(string str)
    {
        if (str == "Exit")
        {
            UpgradePanel.gameObject.SetActive(false);

        }
        if (str == "UpgradeSaw")
        {
            sawStage1.SetActive(false);
            sawStage2.SetActive(true);

        }

    }
    void OpenUpgradePanel()
    {
        UpgradePanel.gameObject.SetActive(true);
    }
}
