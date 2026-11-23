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
    [SerializeField] TMP_Text rankingText;

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

        subtitle.text += $"\n\n<size=150%><b>Pontuação: {gm.Score}</b></size>";

        if (rankingText)
        {
            var top = ScoreBoard.GetTop();
            var sb = new System.Text.StringBuilder("<b>Ranking</b>\n");
            const int Mostrar = 5;
            for (int i = 0; i < Mathf.Min(Mostrar, top.Count); i++)
            {
                string linha = $"{i + 1}. {top[i].score}  ·  {top[i].difficulty}  ·  {top[i].date}";
                sb.AppendLine(i == gm.RankPosition ? $"<color=#FFD54A><b>{linha}  (você)</b></color>" : linha);
            }

            if (gm.RankPosition >= Mostrar)   // quedó del 6.º al 10.º: igual se lo mostramos
            {
                var e = top[gm.RankPosition];
                sb.AppendLine("...");
                sb.AppendLine($"<color=#FFD54A><b>{gm.RankPosition + 1}. {e.score}  ·  {e.difficulty}  ·  {e.date}  (você)</b></color>");
            }
            else if (gm.RankPosition < 0) sb.AppendLine("\n(não entrou no top 10)");

            rankingText.text = sb.ToString();   
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
