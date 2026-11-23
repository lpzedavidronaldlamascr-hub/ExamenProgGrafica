using UnityEngine; // Acceso a clases base de Unity.
using UnityEngine.InputSystem; // Acceso al nuevo sistema de entrada.

// Controla movimiento, salto, sonidos y caida del personaje.
public class Personaje : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad horizontal del personaje.
    public float fuerzaSalto = 7f; // Fuerza vertical aplicada al saltar.
    public AudioClip sonidoSaltar; // Clip de audio para el salto.
    public AudioClip sonidoChocar; // Clip de audio para el choque.
    public GameObject mensajeCaida; // UI que se muestra al caer.
    public GameObject usuario; // UI que se muestra todo el tiempo.
    private Rigidbody2D rb; // Referencia al Rigidbody2D para mover con fisica.
    private AudioSource audioSource; // Referencia al AudioSource para reproducir sonidos.
    private bool enSuelo; // Indica si el personaje esta tocando el suelo.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del objeto.
        audioSource = GetComponent<AudioSource>(); // Obtiene el AudioSource del objeto.
        if (mensajeCaida != null) mensajeCaida.SetActive(false); // Oculta el mensaje al iniciar.
        if (usuario != null) usuario.SetActive(true); // Muestra el usuario todo el tiempo.
    }

    void Update()
    {
        var teclado = Keyboard.current; // Obtiene el teclado actual.
        if (teclado == null) return; // Si no hay teclado, no hace nada.

        // Flecha arriba: mover izquierda. A: mover derecha.
        float movimiento = 0f; // Valor de movimiento horizontal.
        if (teclado.upArrowKey.isPressed) movimiento = -1f; // Mueve a la izquierda.
        if (teclado.aKey.isPressed) movimiento = 1f; // Mueve a la derecha.
        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y); // Aplica velocidad horizontal.

        // V: saltar.
        if (teclado.vKey.wasPressedThisFrame && enSuelo) // Salta solo al presionar V en el suelo.
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto); // Aplica fuerza de salto.
            if (sonidoSaltar != null) audioSource.PlayOneShot(sonidoSaltar); // Reproduce sonido de salto.
        }

        // Al caer, mostrar mensaje.
        if (transform.position.y < -6f && mensajeCaida != null) // Detecta si cayo debajo del limite.
            mensajeCaida.SetActive(true); // Muestra el mensaje de caida.
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Al chocar con un tile, reproducir sonido.
        if (sonidoChocar != null) audioSource.PlayOneShot(sonidoChocar); // Reproduce sonido de choque.
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // Considera suelo si el contacto viene desde abajo.
        foreach (var contacto in col.contacts) // Revisa cada punto de contacto.
            if (contacto.normal.y > 0.5f) { enSuelo = true; return; } // Marca que esta en suelo.
    }

    void OnCollisionExit2D(Collision2D col)
    {
        enSuelo = false; // Al dejar de colisionar, ya no esta en suelo.
    }
}
