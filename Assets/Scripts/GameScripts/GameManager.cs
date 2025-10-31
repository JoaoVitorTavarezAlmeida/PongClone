using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // =========================================================
    // REFERÊNCIAS A OBJETOS DO JOGO
    // =========================================================
    [Header("Ball")]
    public GameObject Ball;                     // Referência à bola

    [Header("Player1")]
    public GameObject player1Paddle;            // Paddle do jogador 1
    public GameObject player1Gol;               // Gol do jogador 1

    [Header("Player2")]
    public GameObject player2Paddle;            // Paddle do jogador 2
    public GameObject player2Gol;               // Gol do jogador 2

    [Header("Score")]
    public GameObject player1txt;               // Texto do placar do jogador 1
    public GameObject player2txt;               // Texto do placar do jogador 2
    public float timeBetween = 0.5f;            // Intervalo entre contagens no início da rodada

    private int player1Score;                   // Pontuação do jogador 1
    private int player2Score;                   // Pontuação do jogador 2

    // =========================================================
    // CONFIGURAÇÕES DE ÁUDIO
    // =========================================================
    [Header("Sounds")]
    private AudioSource audioSource;             // Fonte de áudio principal
    public AudioClip clip;                       // Som de ponto
    private float vol;                           // Volume configurado pelo jogador

    // =========================================================
    // INTERFACE DE USUÁRIO (UI)
    // =========================================================
    [Header("UI")]
    public GameObject countDownText;             // Texto da contagem regressiva
    public GameObject pauseMenuUI;               // Menu de pausa
    public TextMeshProUGUI playerWin;            // Texto que mostra o vencedor

    private bool isPaused;                       // Indica se o jogo está pausado
    private bool isGameOver;                     // Indica se o jogo terminou

    // =========================================================
    // CONFIGURAÇÕES DE JOGO (PlayerPrefs)
    // =========================================================
    private int pointsToWin;                     // Pontos necessários para vencer


    // =========================================================
    // MÉTODOS UNITY
    // =========================================================

    /// <summary>
    /// Inicializa o jogo e define as configurações iniciais.
    /// </summary>
    private void Start()
    {
        StartGame();
        vol = PlayerPrefs.GetFloat("VolumeDaMusica");
        audioSource.volume = vol;
        Time.timeScale = 1.0f;
        playerWin.text = "PAUSE!";
    }

    /// <summary>
    /// Atualiza o estado do jogo a cada frame.
    /// </summary>
    public void Update()
    {
        // Controle de pausa com a tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        // Verifica se algum jogador atingiu os pontos necessários para vencer
        if (player1Score == pointsToWin || player2Score == pointsToWin)
        {
            if (player1Score == pointsToWin)
                onPlayerWins("Player 1");
            else
                onPlayerWins("Player 2");
        }
    }


    // =========================================================
    // MÉTODOS PRINCIPAIS DE JOGO
    // =========================================================

    /// <summary>
    /// Configura o início da partida e inicia a música e contagem regressiva.
    /// </summary>
    private void StartGame()
    {
        player1Score = 0;
        player2Score = 0;

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.Play();
        audioSource.volume = 0.5f;

        StartCoroutine(StartCountdown());
        pointsToWin = PlayMenu.pontosParaVencer;
    }

    /// <summary>
    /// Executa a contagem regressiva antes da bola ser lançada.
    /// </summary>
    IEnumerator StartCountdown()
    {
        Ball.GetComponent<Ball>().rb.linearVelocity = Vector2.zero;

        for (int i = 3; i > 0; i--)
        {
            countDownText.GetComponent<TextMeshProUGUI>().text = i.ToString();
            yield return new WaitForSeconds(timeBetween);
        }

        countDownText.GetComponent<TextMeshProUGUI>().text = "GO!";
        yield return new WaitForSeconds(timeBetween);

        countDownText.GetComponent<TextMeshProUGUI>().text = "";
        Ball.GetComponent<Ball>().Lauch();
    }


    // =========================================================
    // CONTROLE DE PAUSA E VITÓRIA
    // =========================================================

    /// <summary>
    /// Pausa o jogo e exibe o menu de pausa.
    /// </summary>
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /// <summary>
    /// Retoma o jogo e oculta o menu de pausa.
    /// </summary>
    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Mostra o vencedor e pausa o jogo.
    /// </summary>
    void onPlayerWins(string player)
    {
        isGameOver = true;
        PauseGame();
        playerWin.text = $"O {player} Venceu!";
    }


    // =========================================================
    // ATUALIZAÇÃO DE PONTUAÇÃO
    // =========================================================

    /// <summary>
    /// Incrementa a pontuação do jogador 1 e reinicia a rodada.
    /// </summary>
    public void Player1Scored()
    {
        player1Score++;
        WinSound();
        player1txt.GetComponent<TextMeshProUGUI>().text = player1Score.ToString();
        ResetPosition();
    }

    /// <summary>
    /// Incrementa a pontuação do jogador 2 e reinicia a rodada.
    /// </summary>
    public void Player2Scored()
    {
        player2Score++;
        WinSound();
        player2txt.GetComponent<TextMeshProUGUI>().text = player2Score.ToString();
        ResetPosition();
    }


    // =========================================================
    // CONTROLE DE REINÍCIO E ÁUDIO
    // =========================================================

    /// <summary>
    /// Reseta as posições dos jogadores e da bola e reinicia a contagem.
    /// </summary>
    private void ResetPosition()
    {
        player1Paddle.GetComponent<Paddle>().Reset();
        player2Paddle.GetComponent<Paddle>().Reset();
        Ball.GetComponent<Ball>().Reset();
        StartCoroutine(StartCountdown());
    }

    /// <summary>
    /// Toca o som de ponto.
    /// </summary>
    private void WinSound()
    {
        audioSource.PlayOneShot(clip);
    }


    // =========================================================
    // TROCA DE CENA
    // =========================================================

    /// <summary>
    /// Retorna ao menu principal.
    /// </summary>
    public void OnMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
