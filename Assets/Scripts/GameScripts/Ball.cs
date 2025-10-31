using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // =========================================================
    // CONFIGURAÇÕES GERAIS DA BOLA
    // =========================================================
    [Header("Configurações")]
    public float speed;                        // Velocidade da bola
    public Rigidbody2D rb;                     // Referência ao Rigidbody2D
    public LayerMask collisionMask;            // Camadas que a bola pode colidir
    public float rayCastDistance = 100f;       // Distância máxima do Raycast usado na previsão


    // =========================================================
    // CONFIGURAÇÕES DE SOM
    // =========================================================
    [Header("Som")]
    public AudioClip[] audioClips;             // Lista de sons (parede, jogador, gol)
    private AudioSource audioSource;           // Fonte de áudio
    private float vol;                         // Volume atual do jogo


    // =========================================================
    // DEPURAÇÃO E TRAJETÓRIA
    // =========================================================
    [Header("Depuração")]
    public Vector2 direction;                  // Direção atual da bola
    public List<Vector2> predictedPath = new List<Vector2>(); // Pontos previstos de ricochete

    private Vector2 startPosition;             // Posição inicial da bola
    private bool isLauched;                    // Indica se a bola já foi lançada


    // =========================================================
    // MÉTODOS UNITY
    // =========================================================

    /// <summary>
    /// Inicializa variáveis e carrega configurações salvas.
    /// </summary>
    void Start()
    {
        speed = PlayerPrefs.GetFloat("VelocidadeDaBola");
        isLauched = false;
        startPosition = transform.position;

        audioSource = gameObject.AddComponent<AudioSource>();
        vol = PlayerPrefs.GetFloat("VolumeDoJogo");
        audioSource.volume = vol;
    }

    /// <summary>
    /// Atualização fixa de física — mantém o movimento e calcula a trajetória.
    /// </summary>
    void FixedUpdate()
    {
        if (isLauched)
        {
            // Mantém a bola em movimento contínuo
            rb.linearVelocity = direction * speed;
        }

        // Atualiza a previsão da trajetória para a IA
        PredictTrajectory(5);
    }


    // =========================================================
    // COLISÕES E REBOTES
    // =========================================================

    /// <summary>
    /// Detecta colisões e ajusta a direção da bola de acordo.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        if (collision.gameObject.CompareTag("Player"))
        {
            // Calcula o ponto de impacto relativo no paddle
            float paddleY = collision.transform.position.y;
            float ballY = transform.position.y;
            float paddleHeight = collision.collider.bounds.size.y;

            // Define o fator de impacto (-1 a 1)
            float hitFactor = (ballY - paddleY) / (paddleHeight / 2f);

            // Direção horizontal (para o lado oposto ao paddle)
            float directionX = collision.transform.position.x < transform.position.x ? 1 : -1;

            // Define a nova direção da bola
            direction = new Vector2(directionX, hitFactor).normalized;
        }
        else
        {
            // Rebote normal (paredes ou gols)
            direction = Vector2.Reflect(direction, normal).normalized;
        }

        // Toca o som correspondente à colisão
        if (collision.gameObject.CompareTag("wall"))
            audioSource.PlayOneShot(audioClips[0]);
        else if (collision.gameObject.CompareTag("Player"))
            audioSource.PlayOneShot(audioClips[1]);
        else if (collision.gameObject.CompareTag("gol"))
            audioSource.PlayOneShot(audioClips[2]);
    }


    // =========================================================
    // PREVISÃO DE TRAJETÓRIA (USADA PELA IA)
    // =========================================================

    /// <summary>
    /// Calcula a trajetória prevista da bola considerando múltiplos ricochetes.
    /// </summary>
    public void PredictTrajectory(int maxBounces)
    {
        predictedPath.Clear();
        Vector2 currentPos = transform.position;
        Vector2 currentDir = direction.normalized;

        for (int i = 0; i < maxBounces; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, rayCastDistance, collisionMask);
            if (hit.collider == null) break;

            // Armazena ponto de impacto e desenha linha de depuração
            predictedPath.Add(hit.point);
            Debug.DrawLine(currentPos, hit.point, Color.red);

            // Interrompe caso atinja um gol ou paddle
            if (hit.collider.CompareTag("gol") || hit.collider.CompareTag("Player"))
                break;

            // Reflete a direção para continuar o cálculo
            currentDir = Vector2.Reflect(currentDir, hit.normal);
            currentPos = hit.point;
        }
    }


    // =========================================================
    // LANÇAMENTO E RESET
    // =========================================================

    /// <summary>
    /// Lança a bola em uma direção aleatória.
    /// </summary>
    public void Lauch()
    {
        isLauched = true;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(-0.1f, 0.1f);
        direction = new Vector2(x, y).normalized;

        rb.linearVelocity = direction * speed;
    }

    /// <summary>
    /// Reseta a bola para a posição inicial.
    /// </summary>
    public void Reset()
    {
        isLauched = false;
        rb.linearVelocity = Vector2.zero;
        transform.position = startPosition;
    }
}
