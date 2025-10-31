using UnityEngine;

public class Paddle : MonoBehaviour
{
    // =========================================================
    // CONFIGURAÇÕES GERAIS
    // =========================================================
    [Header("Configurações Gerais")]
    public bool isPlayer1;           // Define se este paddle é o jogador 1
    private float speed;             // Velocidade de movimento do paddle
    public Rigidbody2D rb;           // Referência ao Rigidbody2D
    private string gameControlers;   // Tipo de controle atual (JvsJ, JvsAI)
    private string actualControler;  // Controlador ativo (player1, player2, IA)
    private float movement;          // Entrada vertical do jogador
    private Vector2 startPosition;   // Posição inicial do paddle

    // =========================================================
    // CONFIGURAÇÕES DA IA
    // =========================================================
    [Header("Configurações da IA")]
    public Transform ball;           // Referência à bola na cena


    // =========================================================
    // MÉTODOS UNITY
    // =========================================================

    /// <summary>
    /// Inicializa o paddle e define o tipo de controle baseado no modo de jogo.
    /// </summary>
    private void Start()
    {
        // Armazena a posição inicial do paddle
        startPosition = transform.position;

        // Lê as configurações salvas de velocidade e modo de jogo
        speed = PlayerPrefs.GetFloat("VelocidadeDoPaddle", 6);
        gameControlers = PlayerPrefs.GetString("GameMode", "JvsJ");

        // Define o tipo de controlador de acordo com o modo e o jogador
        if (gameControlers == "JvsJ")
        {
            actualControler = isPlayer1 ? "player1" : "player2";
        }
        else if (gameControlers == "JvsAI")
        {
            actualControler = isPlayer1 ? "player1" : "IA";
        }

        Debug.Log($"Paddle ({name}) — speed: {speed}, mode: {gameControlers}, controller: {actualControler}");
    }

    /// <summary>
    /// Atualiza o comportamento do paddle conforme o controlador ativo.
    /// </summary>
    private void Update()
    {
        switch (actualControler)
        {
            case "player1":
                Player1();
                break;
            case "player2":
                Player2();
                break;
            case "IA":
                IA();
                break;
        }
    }


    // =========================================================
    // MÉTODOS DE JOGO
    // =========================================================

    /// <summary>
    /// Reseta o paddle para sua posição inicial e zera a velocidade.
    /// </summary>
    public void Reset()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = startPosition;
    }

    /// <summary>
    /// Controle do jogador 1 (usa o eixo "Vertical").
    /// </summary>
    void Player1()
    {
        movement = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, movement * speed);
    }

    /// <summary>
    /// Controle do jogador 2 (usa o eixo "Vertical2").
    /// </summary>
    void Player2()
    {
        movement = Input.GetAxisRaw("Vertical2");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, movement * speed);
    }

    /// <summary>
    /// Controle da IA — segue a bola com base na trajetória prevista.
    /// </summary>
    void IA()
    {
        if (ball == null) return;

        Ball ballScript = ball.GetComponent<Ball>();
        if (ballScript == null || ballScript.predictedPath.Count == 0) return;

        // Obtém o ponto final previsto da trajetória da bola
        Vector2 finalPoint = ballScript.predictedPath[ballScript.predictedPath.Count - 1];

        // Calcula o alvo e a direção de movimento
        float targetY = finalPoint.y;
        float direction = Mathf.Sign(targetY - transform.position.y);

        // Se estiver próximo o suficiente do alvo, para o movimento
        if (Mathf.Abs(targetY - transform.position.y) < 0.1f)
            direction = 0f;

        // Aplica o movimento vertical da IA
        rb.linearVelocity = new Vector2(0f, direction * speed);
    }
}


