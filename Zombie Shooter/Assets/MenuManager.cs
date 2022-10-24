using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public static Menu CurrentMenu = Menu.Main;

    public GameObject menuContainer = null;
    public GameObject mainMenu = null;
    public GameObject pauseMenu = null;
    public GameObject endMenu = null;
    public GameObject tutorialMenu = null;

    public TextMeshProUGUI currentScoreEndScreen = null;
    public TextMeshProUGUI highScoreOnStart = null;

    public void Start()
    {
        Instance = this;
    }

    public void Update()
    {
        currentScoreEndScreen.text = "FINAL SCORE: " + GameManager.currentScore;
        highScoreOnStart.text = "HIGH SCORE: " + GameManager.highScore;
    }

    public static void ShowMenu(Menu menu)
    {
        Instance.HideAllMenus();
        HudHelper.HideHud();
        Instance.menuContainer.gameObject.SetActive(true);

        switch (menu)
        {
            case Menu.Main:
                Instance.mainMenu.gameObject.SetActive(true);
                break;

            case Menu.Tutorial:
                Instance.tutorialMenu.gameObject.SetActive(true);
                break;

            case Menu.Pause:
                Instance.pauseMenu.gameObject.SetActive(true);
                break;

            case Menu.End:
                Instance.endMenu.gameObject.SetActive(true);
                break;

            case Menu.None:
                Instance.menuContainer.gameObject.SetActive(false);
                HudHelper.ShowHud();
                break;
        }
    }

    public void HideAllMenus()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        endMenu.gameObject.SetActive(false);
        tutorialMenu.gameObject.SetActive(false);
    }

    public enum Menu
    {
        Main,
        Pause,
        Tutorial,
        End,
        None
    }
}


