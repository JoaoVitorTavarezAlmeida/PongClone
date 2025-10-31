using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayMenu : MonoBehaviour
{
    // =========================================================
    // CONFIGURAÇÕES SALVAS (PlayerPrefs)
    // =========================================================
    [Header("Configurações salvas")]
    public static int pontosParaVencer;
    public static float velocidadeDaBola;
    public static float velocidadeDoPaddle;
    public static string gameMode;

    // =========================================================
    // SLIDERS DA UI
    // =========================================================
    [Header("Sliders da UI")]
    public Slider sliderPontos;
    public Slider sliderBola;
    public Slider sliderPaddle;

    private TextMeshProUGUI txtPontos;
    private TextMeshProUGUI txtBola;
    private TextMeshProUGUI txtPaddle;

    // =========================================================
    // MÉTODOS UNITY
    // =========================================================

    /// <summary>
    /// Inicializa os sliders e textos com valores salvos e adiciona listeners.
    /// </summary>
    private void Awake()
    {
        // Carrega configurações salvas
        pontosParaVencer = PlayerPrefs.GetInt("PontosParaVencer", 5);
        velocidadeDaBola = PlayerPrefs.GetFloat("VelocidadeDaBola", 10);
        velocidadeDoPaddle = PlayerPrefs.GetFloat("VelocidadeDoPaddle", 6);
        gameMode = PlayerPrefs.GetString("GameMode", "JvsJ");

        // Busca referências aos textos filhos dos sliders
        txtPontos = sliderPontos.GetComponentInChildren<TextMeshProUGUI>();
        txtBola = sliderBola.GetComponentInChildren<TextMeshProUGUI>();
        txtPaddle = sliderPaddle.GetComponentInChildren<TextMeshProUGUI>();

        // Configura sliders com valores iniciais
        sliderPontos.wholeNumbers = true;
        sliderPontos.value = pontosParaVencer;
        sliderBola.value = velocidadeDaBola;
        sliderPaddle.value = velocidadeDoPaddle;

        // Atualiza textos da UI
        AtualizarUI();

        // Adiciona listeners para alterar os valores em tempo real
        sliderPontos.onValueChanged.AddListener(OnPontosMudou);
        sliderBola.onValueChanged.AddListener(OnBolaMudou);
        sliderPaddle.onValueChanged.AddListener(OnPaddleMudou);
    }

    // =========================================================
    // MÉTODOS DE ATUALIZAÇÃO DE SLIDERS
    // =========================================================

    /// <summary>
    /// Atualiza o valor de pontos e salva no PlayerPrefs.
    /// </summary>
    private void OnPontosMudou(float valor)
    {
        pontosParaVencer = Mathf.RoundToInt(valor);
        txtPontos.text = pontosParaVencer.ToString();
        PlayerPrefs.SetInt("PontosParaVencer", pontosParaVencer);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Atualiza a velocidade da bola e salva no PlayerPrefs.
    /// </summary>
    private void OnBolaMudou(float valor)
    {
        velocidadeDaBola = valor;
        txtBola.text = valor.ToString("0.0");
        PlayerPrefs.SetFloat("VelocidadeDaBola", valor);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Atualiza a velocidade do paddle e salva no PlayerPrefs.
    /// </summary>
    private void OnPaddleMudou(float valor)
    {
        velocidadeDoPaddle = valor;
        txtPaddle.text = valor.ToString("0.0");
        PlayerPrefs.SetFloat("VelocidadeDoPaddle", valor);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Atualiza os textos da UI com os valores atuais.
    /// </summary>
    private void AtualizarUI()
    {
        txtPontos.text = pontosParaVencer.ToString();
        txtBola.text = velocidadeDaBola.ToString("0.0");
        txtPaddle.text = velocidadeDoPaddle.ToString("0.0");
    }

    // =========================================================
    // MÉTODOS DE BOTÕES DE SELEÇÃO DE MODO
    // =========================================================

    /// <summary>
    /// Seleciona o modo Jogador vs Jogador e carrega a cena do jogo.
    /// </summary>
    public void OnJogadorVsJogadorClicked()
    {
        gameMode = "JvsJ";
        PlayerPrefs.SetString("GameMode", gameMode);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Seleciona o modo Jogador vs IA e carrega a cena do jogo.
    /// </summary>
    public void OnJogadorVsAIClicked()
    {
        gameMode = "JvsAI";
        PlayerPrefs.SetString("GameMode", gameMode);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Fecha o menu passado como parâmetro.
    /// </summary>
    public void OnXClicked(GameObject menu)
    {
        menu.SetActive(false);
    }
}


