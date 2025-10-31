using UnityEngine;

public class Gol : MonoBehaviour
{
    // =========================================================
    // VARIÁVEIS PRINCIPAIS
    // =========================================================
    private GameManager gameManager;   // Referência ao GameManager da cena
    public bool isPlayer1Gol;          // Define se este gol pertence ao jogador 1


    // =========================================================
    // MÉTODOS UNITY
    // =========================================================

    /// <summary>
    /// Obtém a referência ao GameManager na inicialização.
    /// </summary>
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Detecta quando a bola entra no gol e atualiza a pontuação.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que colidiu é a bola
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Se o gol não pertence ao jogador 1, o jogador 1 marca ponto
            if (!isPlayer1Gol)
            {
                Debug.Log("player1 Scored");
                gameManager.Player1Scored();
            }
            // Caso contrário, o jogador 2 marca ponto
            else
            {
                Debug.Log("player2 Scored");
                gameManager.Player2Scored();
            }
        }
    }
}

