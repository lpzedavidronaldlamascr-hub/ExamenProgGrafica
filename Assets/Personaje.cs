using UnityEngine;
using UnityEngine.InputSystem;

// Controla movimiento, salto, sonidos y caida del personaje.
public class Personaje : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public AudioClip sonidoSaltar;
    public AudioClip sonidoChocar;
    public GameObject mensajeCaida;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (mensajeCaida != null) mensajeCaida.SetActive(false);
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        // Flecha arriba: mover izquierda. A: mover derecha.
        float movimiento = 0f;
        if (teclado.upArrowKey.isPressed) movimiento = -1f;
        if (teclado.aKey.isPressed) movimiento = 1f;
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);

        // V: saltar.
        if (teclado.vKey.wasPressedThisFrame && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            if (sonidoSaltar != null) audioSource.PlayOneShot(sonidoSaltar);
        }

        // Al caer, mostrar mensaje.
        if (transform.position.y < -6f && mensajeCaida != null)
            mensajeCaida.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Al chocar con un tile, reproducir sonido.
        if (sonidoChocar != null) audioSource.PlayOneShot(sonidoChocar);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // Considera suelo si el contacto viene desde abajo.
        foreach (var contacto in col.contacts)
            if (contacto.normal.y > 0.5f) { enSuelo = true; return; }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        enSuelo = false;
    }
}
