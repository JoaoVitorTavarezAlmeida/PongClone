using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // =========================================================
    // REFERÊNCIAS A PAINEIS DE UI
    // =========================================================
    public GameObject configPanel;   // Painel de configurações
    public GameObject playPanel;     // Painel de seleção de jogo

    // =========================================================
    // MÉTODOS DE BOTÕES DO MENU
    // =========================================================

    /// <summary>
    /// Ativa o painel de jogo quando o botão "Play" é clicado.
    /// </summary>
    public void OnPlayClicked()
    {
        playPanel.SetActive(true);
    }

    /// <summary>
    /// Ativa o painel de configurações quando o botão "Config" é clicado.
    /// </summary>
    public void OnConfigclicked()
    {
        configPanel.SetActive(true);
    }

    /// <summary>
    /// Encerra o jogo quando o botão "Quit" é clicado.
    /// </summary>
    public void OnQuitClicked()
    {
        Debug.Log("Closing the game!");
        Application.Quit();
    }
}

