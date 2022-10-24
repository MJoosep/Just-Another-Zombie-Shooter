using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public UnityEvent OnGameReset;

    public Animator cameraAnimator;

    public static bool Started = false;
    public static bool Paused = false;

    public static int currentScore = 0;
    public static int highScore = 0;

    public static int playerHealth = 100;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
                ResumeGame();
            else
                PauseGame();
        }

        if (Started && !Paused)
            HudHelper.SetScore(currentScore);

        if (playerHealth < 0)
            EndGame();

        HudHelper.SetHealthBar((float)playerHealth / 100f);

    }

    public void PauseGame()
    {
        MenuManager.ShowMenu(MenuManager.Menu.Pause);
        Paused = true;
        ShowMouse();
    }

    public void ResumeGame()
    {
        MenuManager.ShowMenu(MenuManager.Menu.None);
        Paused = false;
        HideMouse();
    }

    public void ResetGame()
    {
        cameraAnimator.SetTrigger("End");

        playerHealth = 100;
        HudHelper.SetHealthBar(1f);

        if (currentScore > highScore)
            highScore = currentScore;

        OnGameReset?.Invoke();
        Started = false;
        Paused = false;
        currentScore = 0;
        ShowMouse();
        MenuManager.ShowMenu(MenuManager.Menu.Main);
    }

    public void ShowTutorial()
    {
        MenuManager.ShowMenu(MenuManager.Menu.Tutorial);
    }

    public void StartGame()
    {
        cameraAnimator.SetTrigger("Start");
        MenuManager.ShowMenu(MenuManager.Menu.None);
        HideMouse();
        StartCoroutine(WaitForCameraAnimation());
    }

    public IEnumerator WaitForCameraAnimation()
    {
        yield return new WaitForSeconds(2);
        Started = true;
    }

    public void EndGame()
    {
        playerHealth = 100;
        HudHelper.SetHealthBar(1f);
        MenuManager.ShowMenu(MenuManager.Menu.End);
        Started = false;
        Paused = false;
        ShowMouse();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
