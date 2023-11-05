using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Image UpgradePanel;
    [SerializeField] GameObject sawStage1;
    [SerializeField] GameObject sawStage2;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenMenu;
    }

    public void OnButtonClick(string str)
    {
        if (str == "Exit")
        {
            CloseMenu();
        }
        if (str == "UpgradeSaw")
        {
            sawStage1.SetActive(false);
            sawStage2.SetActive(true);

        }

    }
    void OpenMenu(GameState state)
    {
        if (state == GameState.Upgrading)
            UpgradePanel.gameObject.SetActive(true);
    }
    void CloseMenu()
    {
        UpgradePanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }
}
