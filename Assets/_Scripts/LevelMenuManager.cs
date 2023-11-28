using PT.Garden;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    public static LevelMenuManager Instance;
    [TabGroup("Collections")][SerializeField] GameObject[] levelObjects;
    [TabGroup("Collections")][SerializeField] GameObject[] levelButtons;
    //===============================================
    [SerializeField] Image levelSelectionPanel;
    [SerializeField] GameObject loadingPanel;
    //===============================================
    public static int currentLevel = 0;
    public static int loadedLevel = 0;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OpenLevelSelectionMenu;
        PercentageChecker.OnFirstStarUnlock += UnlockNextLevelButton;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OpenLevelSelectionMenu;
        PercentageChecker.OnFirstStarUnlock -= UnlockNextLevelButton;

    }
    private void Start()
    {

        currentLevel=PlayerPrefs.GetInt("CurrentLevel");
        levelObjects[currentLevel].SetActive(true);

        for (int i = 0; i <= PlayerPrefs.GetInt("TotalLevelsUnlocked"); i++)
        {
            levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
            levelButtons[i].GetComponent<Button>().enabled = true;
        }

    }
    public void OnButtonClick(string button)
    {
        if (button == "Exit")
        {
            CloseLevelSelectionMenu();
        }

    }


    public void LoadLevel(int level)
    {

        if (currentLevel != level)
        {
            loadingPanel.SetActive(true);
            CloseLevelSelectionMenu();
            levelObjects[currentLevel].SetActive(false);
            levelObjects[level].SetActive(true);
            currentLevel = level;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        }



    }

    void OpenLevelSelectionMenu(GameState state)
    {
        if (state == GameState.InLevelMenu)
            levelSelectionPanel.gameObject.SetActive(true);
    }
    void CloseLevelSelectionMenu()
    {
        levelSelectionPanel.gameObject.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }

    void UnlockNextLevelButton()
    {
        if (currentLevel == PlayerPrefs.GetInt("TotalLevelsUnlocked"))
        {
            PlayerPrefs.SetInt("TotalLevelsUnlocked", PlayerPrefs.GetInt("TotalLevelsUnlocked") + 1);
            for (int i = 0; i <= PlayerPrefs.GetInt("TotalLevelsUnlocked"); i++)
            {
                levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                levelButtons[i].GetComponent<Button>().enabled = true;
            }
        }
    }


}
