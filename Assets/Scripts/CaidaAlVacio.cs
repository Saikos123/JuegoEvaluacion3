using UnityEngine;
using UnityEngine.SceneManagement;

public class CaidaAlVacio : MonoBehaviour
{
    // Altura mínima en el eje Y antes de considerarse caída al vacío
    public float alturaLimite = -5f;

    // Posición inicial donde reaparecerá el jugador
    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos la posición en la que empieza la esfera al dar Play
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Si la posición Y del jugador cae por debajo del límite configurado
        if (transform.position.y < alturaLimite)
        {
            ReiniciarPosicion();
        }
    }

    void ReiniciarPosicion()
    {
        // Si el objeto tiene Rigidbody, le frenamos las fuerzas físicas para que no respawnee rodando con locura
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Devolvemos al jugador a su punto de inicio
        transform.position = posicionInicial;
    }
}