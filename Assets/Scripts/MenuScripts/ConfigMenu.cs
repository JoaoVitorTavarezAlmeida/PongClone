using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour
{
    // -------------------------------------------------
    // Configurações salvas no PlayerPrefs
    // -------------------------------------------------
    [Header("Configurações Salvas")]
    public static float volumeMusic;
    public static float volumeGame;

    // -------------------------------------------------
    // Referências aos sliders da UI
    // -------------------------------------------------
    [Header("Slider UI")]
    public Slider musicSlider;
    public Slider gameSlider;

    // Textos que exibem os valores numéricos dos volumes
    private TextMeshProUGUI gameVoltxt;
    private TextMeshProUGUI musicVoltxt;

    // -------------------------------------------------
    // Inicializa os valores de volume e associa os eventos dos sliders
    // -------------------------------------------------
    private void Awake()
    {
        // Carrega volumes salvos ou usa valores padrão (0.5)
        volumeGame = PlayerPrefs.GetFloat("VolumeDoJogo", 0.5f);
        volumeMusic = PlayerPrefs.GetFloat("VolumeDaMusica", 0.5f);

        // Obtém referências aos textos filhos dos sliders
        gameVoltxt = gameSlider.GetComponentInChildren<TextMeshProUGUI>();
        musicVoltxt = musicSlider.GetComponentInChildren<TextMeshProUGUI>();

        // Define os valores iniciais dos sliders
        musicSlider.value = volumeMusic;
        gameSlider.value = volumeGame;

        // Adiciona eventos para atualizar volumes quando o slider mudar
        musicSlider.onValueChanged.AddListener(OnMusicVolumeMudou);
        gameSlider.onValueChanged.AddListener(OnGameVolumeMudou);

        // Atualiza o texto da UI
        AtualizarUI();
    }

    // -------------------------------------------------
    // Atualiza os textos de volume na tela
    // -------------------------------------------------
    private void AtualizarUI()
    {
        gameVoltxt.text = gameSlider.value.ToString("0.0");
        musicVoltxt.text = musicSlider.value.ToString("0.0");
    }

    // -------------------------------------------------
    // Chamado quando o volume do jogo é alterado
    // -------------------------------------------------
    public void OnGameVolumeMudou(float volume)
    {
        volumeGame = volume;
        gameVoltxt.text = volume.ToString("0.0");
        PlayerPrefs.SetFloat("VolumeDoJogo", volume);
        PlayerPrefs.Save();
    }

    // -------------------------------------------------
    // Chamado quando o volume da música é alterado
    // -------------------------------------------------
    public void OnMusicVolumeMudou(float volume)
    {
        volumeMusic = volume;
        musicVoltxt.text = volume.ToString("0.0");
        PlayerPrefs.SetFloat("VolumeDaMusica", volume);
        PlayerPrefs.Save();
    }
}
