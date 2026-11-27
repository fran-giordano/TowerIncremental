using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TowerIncremental.Core;

namespace TowerIncremental.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] Button resumeButton;
        [SerializeField] Button menuButton;
        [SerializeField] string menuScene = "MainMenu";

        void Awake()
        {
            panel.SetActive(false);
            resumeButton.onClick.AddListener(() => SetPaused(false));
            menuButton.onClick.AddListener(() => SceneManager.LoadScene(menuScene));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SetPaused(!GameManager.I.Paused);
        }

        void SetPaused(bool paused)
        {
            GameManager.I.SetPaused(paused);
            panel.SetActive(GameManager.I.Paused);
        }
    }
}
