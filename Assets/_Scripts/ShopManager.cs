using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    [SerializeField] Image shopPanel;
    //===================================
    [Title("Select Button References")]
    public Button[] previewButtons;
    [Title("Select Button References")]
    public GameObject[] selectButtons;
    [Title("Skin Objects")]
    public GameObject[] truckSkins;
    [Title("Text References")]
    public Text[] skins_CT;
    [Title("Value Collections")]
    public int[] skins_CR;
    //===================================
    private int selectedIndex = 0;
    private int previousSelectedIndex = 0;
    private int previewIndex = 0;



    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenShopMenu;
    }
    private void Start()
    {
        UpdateSkinVisibility();
        SetDeafult();
    }
    void SetDeafult()
    {
        for (int i = 0; i < skins_CT.Length; i++)
        {
            skins_CT[i].text = "$" + skins_CR[i].ToString();

        }
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
        if (CurrencyManager.CheckRequiredCoins(skins_CR[index]))
        {
            selectButtons[index].SetActive(false);
            previousSelectedIndex = selectedIndex;
            selectedIndex = index;
            UpdateSkinVisibility();
        }
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
