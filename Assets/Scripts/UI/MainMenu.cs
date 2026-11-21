using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string gameScene = "projecOutdate";

    [Header("Dificultades")]
    [SerializeField] DifficultyData easy;
    [SerializeField] DifficultyData hard;

    [Header("Paneles")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject difficultyPanel;

    public TMP_Text highscoreTXT;

    void Start()
    {
        highscoreTXT.text = "Highscore: " + PlayerPrefs.GetInt("highscore", 0);
        ShowMain();
    }

    // Botón Jogar: no carga el juego, abre la elección de dificultad
    public void PlayGame()
    {
        mainPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void SelectEasy() => StartGame(easy);
    public void SelectHard() => StartGame(hard);

    // Botón Voltar
    public void ShowMain()
    {
        mainPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }

    void StartGame(DifficultyData difficulty)
    {
        DifficultyData.Selected = difficulty;   // el GameManager la lee en su Awake
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}