using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    AudioManager AM;
    public Image shopPanel;
    //===================================
    [Title("Select Button References")]
    public Button[] previewButtons;
    [Title("Buy Button References")]
    public GameObject[] BuyButtons;
    [Title("Select Button References")]
    public GameObject[] selectButtons;
    [Title("Skin Objects")]
    public GameObject[] truckSkins;
    [Title("Text References")]
    public Text[] skins_CT;
    [Title("Value Collections")]
    public int[] skins_CR;
    //===================================
    public int selectedIndex = 0;
    public int previousSelectedIndex = 0;
    private int previewIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenShopMenu;
    }
    private void Start()
    {
        //ES3AutoSaveMgr.Current.Load();
        AM=AudioManager.instance;
        UpdateSkinVisibility();
        SetDeafult();
        CheckTextColor();
    }
    private void CheckTextColor()
    {
        for (int i = 0; i < skins_CT.Length; i++)
        {
            CurrencyManager.UpdateAffordabilityStatus(skins_CT[i], skins_CR[i]);
        }

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
            AM.Play("Pop");
            CloseShopMenu();

        }
    }
    void OpenShopMenu(GameState state)
    {
        if (state == GameState.InShop)
        {
            if (shopPanel != null)
            {

                shopPanel.gameObject.SetActive(true);
                TruckUpgradeManager.Instance.truckUpgradePanel.gameObject.SetActive(false);
            }
            CheckTextColor();

        }
    }
    void CloseShopMenu()
    {
        shopPanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
        ApplySkin(selectedIndex);
    }
    public void PreviewSkin(int index)
    {
        AM.Play("Pop");
        ApplySkin(index);
    }
    public void BuySkin(int index)
    {
        AM.Play("Pop");
        if (CurrencyManager.CheckRequiredCoins(skins_CR[index]))
        {
            for (int i = 0; i < selectButtons.Length; i++)
            {
                if (i == index)
                    selectButtons[i].GetComponentInChildren<Text>().text = "Selected";
                else
                    selectButtons[i].GetComponentInChildren<Text>().text = "Use";

            }
            BuyButtons[index].SetActive(false);
            previousSelectedIndex = selectedIndex;
            selectedIndex = index;
            UpdateSkinVisibility();
            CurrencyManager.Instance.DeductCoins(skins_CR[index]);
            CheckTextColor();
        }
    }
    public void SelectSkin(int index)
    {
        AM.Play("Pop");
        for (int i = 0; i < selectButtons.Length; i++)
        {
            if (i == index)
                selectButtons[i].GetComponentInChildren<Text>().text = "Selected";
            else
                selectButtons[i].GetComponentInChildren<Text>().text = "Use";

        }
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
