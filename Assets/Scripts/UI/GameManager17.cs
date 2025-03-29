using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class GameManager17 : Singleton<GameManager17>
{
    [SerializeField] private GameObject loseMenu, pauseMenu;
    [SerializeField] private RectTransform losePanel, pausePanel;
    [SerializeField] private float topPosY = 250f, middlePosY, tweenDuration = 0.3f;

    protected override void Awake()
    {
        base.Awake();
    }

    async void Start()
    {
        await HidePanel(loseMenu, losePanel);
        await HidePanel(pauseMenu, pausePanel);
    }

    public void Home() => SceneManager.LoadScene("Home");
    public void PlayGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void PauseGame() => OpenMenu(pauseMenu, pausePanel, 1);

    public async void ResumeGame()
    {
        SoundManager17.Instance.SoundClick();
        await HidePanel(pauseMenu, pausePanel);
        Time.timeScale = 1f;
    }

    public void GameLose() => OpenMenu(loseMenu, losePanel, 3);

    private void OpenMenu(GameObject menu, RectTransform panel, int soundIndex)
    {
        SoundManager17.Instance.PlaySound(soundIndex);
        ShowPanel(menu, panel);
    }

    private void ShowPanel(GameObject menu, RectTransform panel)
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        menu.GetComponent<CanvasGroup>().DOFade(1, tweenDuration).SetUpdate(true);
        panel.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    private async Task HidePanel(GameObject menu, RectTransform panel)
    {
        if (menu == null || panel == null) return;

        menu.GetComponent<CanvasGroup>().DOFade(0, tweenDuration).SetUpdate(true);
        await panel.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        if (menu) menu.SetActive(false);
    }
}
