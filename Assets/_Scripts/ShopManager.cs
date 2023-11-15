using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    private int selectedIndex = 0;
    private int previousSelectedIndex = 0;
    private int previewIndex = 0;
    [Title("UI References")]
    [SerializeField] Image shopPanel;
    [Space]
    public Button[] previewButtons;
    public Button[] selectButtons;
    [Title("Skins")]
    [SerializeField] GameObject[] truckSkins;
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenShopMenu;
    }
    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {
            CloseShopMenu();

        }
    }
    void OpenShopMenu(GameState state)
    {
        if (state == GameState.InShop)
            shopPanel.gameObject.SetActive(true);
    }
    void CloseShopMenu()
    {
        shopPanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
        if (selectedIndex == previousSelectedIndex)
        {
            ApplySkin(previousSelectedIndex);
        }

    }
    public void PreviewSkin(int index)
    {
        ApplySkin(index);
    }
    public void SelectSkin(int index)
    {
        previousSelectedIndex = selectedIndex;
        selectedIndex = index;
        UpdateSkinVisibility();
    }
    void UpdateSkinVisibility()
    {
        for (int i = 0; i < truckSkins.Length; i++)
        {
            if (i == selectedIndex)
            {
                truckSkins[i].SetActive(true);
            }
            else
            {
                truckSkins[i].SetActive(false);
            }
        }
    }
    void ApplySkin(int index)
    {
        previewIndex = index;
        if (index >= 0 && index < truckSkins.Length)
        {
            for (int i = 0; i < truckSkins.Length; i++)
            {
                if (i == previewIndex)
                {
                    truckSkins[i].SetActive(true);
                }
                else
                {
                    truckSkins[i].SetActive(false);
                }
            }
        }
    }

}
