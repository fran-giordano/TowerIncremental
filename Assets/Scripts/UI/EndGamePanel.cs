using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Va en un objeto SIEMPRE activo (el Canvas), no en el panel:
// un objeto desactivado no recibe eventos ni ejecuta Start.
public class EndGamePanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text subtitle;
    [SerializeField] Button restartButton;
    [SerializeField] Button menuButton;              // opcional, hasta que exista el menú
    [SerializeField] string menuScene = "MainMenu";

    void Awake()
    {
        panel.SetActive(false);
        restartButton.onClick.AddListener(Restart);
        if (menuButton) menuButton.onClick.AddListener(() => SceneManager.LoadScene(menuScene));
    }

    // En Start y no en OnEnable: acá ya corrieron todos los Awake, así GameManager.I existe.
    void Start() => GameManager.I.GameEnded += Show;
    void OnDestroy() { if (GameManager.I) GameManager.I.GameEnded -= Show; }

    void Show(GameState result)
    {
        var gm = GameManager.I;

        if (result == GameState.Won)
        {
            title.text = "Vitória!";
            subtitle.text = $"Tempo restante: {FormatTime(gm.TimeLeft)}\nCastelo: {gm.BaseHealth}";
        }
        else
        {
            title.text = "Derrota";
            subtitle.text = gm.BaseHealth <= 0 ? "O castelo foi destruído" : "Tempo esgotado";
        }

        panel.SetActive(true);
    }

    // Por nombre: funciona aunque la escena no tenga índice en la lista de build.
    void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    static string FormatTime(float s)
    {
        int t = Mathf.CeilToInt(s);
        return $"{t / 60}:{t % 60:00}";
    }
}
