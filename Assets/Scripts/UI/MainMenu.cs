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
    [SerializeField] GameObject rankingPanel;
    
    [SerializeField] TMP_Text rankingText;

    public TMP_Text highscoreTXT;

    void Start()
    {
        ShowMain();                 // primero: el menú arranca bien aunque falle lo demás
    }

    // Botón Jogar: no carga el juego, abre la elección de dificultad
    public void PlayGame()
    {
        mainPanel.SetActive(false);
        rankingPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void SelectEasy() => StartGame(easy);
    public void SelectHard() => StartGame(hard);

    // Botón Voltar
    public void ShowMain()
    {
        mainPanel.SetActive(true);
        difficultyPanel.SetActive(false);
        rankingPanel.SetActive(false);
    }

    void StartGame(DifficultyData difficulty)
    {
        DifficultyData.Selected = difficulty;   // el GameManager la lee en su Awake
        SceneManager.LoadScene(gameScene);
    }

    public void ShowRanking()
    {
        mainPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        rankingPanel.SetActive(true);

        var top = ScoreBoard.GetTop();
        if(top.Count == 0)
        {
            rankingText.text = "Sem recordes ainda.";
        }
        else
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < top.Count; i++)
                sb.AppendLine($"{i + 1}. {top[i].score}  —  {top[i].difficulty}  —  {top[i].date}");
            rankingText.text = sb.ToString();       
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}