using System.Collections;
using UnityEngine;
using TMPro;

public class ContadorMonedas : MonoBehaviour
{
    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI textoPuntaje;
    public GameObject mensajeVictoria;

    [Header("Configuración de Meta y Caída")]
    public float tiempoEsperaReinicio = 2f; // Segundos antes de volver al inicio al ganar
    public float alturaCaida = -10f;        // Altura Y en la que se considera que cayó al vacío

    private int puntos = 0;
    private int totalMonedas;
    private bool juegoGanado = false;

    private Vector3 posicionInicial;
    private Rigidbody rb;
    private GameObject[] todasLasMonedas;
    private GameObject monedaMeta;

    void Start()
    {
        Time.timeScale = 1f;

        // Guarda la posición inicial de la pelota y su Rigidbody
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody>();

        // Guarda las 7 monedas con tag "Moneda" y la última moneda con tag "Meta"
        todasLasMonedas = GameObject.FindGameObjectsWithTag("Moneda");
        monedaMeta = GameObject.FindGameObjectWithTag("Meta");

        // Suma ambas para que el total sea 8
        totalMonedas = todasLasMonedas.Length + (monedaMeta != null ? 1 : 0);

        ActualizarUI();

        if (mensajeVictoria != null)
        {
            mensajeVictoria.SetActive(false);
        }
    }

    void Update()
    {
        // Mantiene el mensaje de victoria visible durante los 2 segundos de espera
        if (juegoGanado)
        {
            textoPuntaje.text = "¡Ganaste! Puntos: " + puntos + " / " + totalMonedas;
            return;
        }

        // Si la pelota cae por debajo de la altura límite, reinicia todo
        if (transform.position.y < alturaCaida)
        {
            ReiniciarProgreso();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (juegoGanado) return;

        // 1. Recoger las monedas normales
        if (other.CompareTag("Moneda"))
        {
            other.gameObject.SetActive(false);
            puntos++;
            ActualizarUI();
        }
        // 2. Recoger la última moneda con el tag "Meta"
        else if (other.CompareTag("Meta"))
        {
            // Verifica que ya hayas juntado todas las monedas anteriores
            if (puntos >= totalMonedas - 1)
            {
                other.gameObject.SetActive(false);
                puntos++;
                StartCoroutine(Victoria());
            }
            else
            {
                textoPuntaje.text = "¡Te faltan monedas! (" + puntos + " / " + totalMonedas + ")";
            }
        }
    }

    IEnumerator Victoria()
    {
        juegoGanado = true;
        textoPuntaje.text = "¡Ganaste! Puntos: " + puntos + " / " + totalMonedas;

        if (mensajeVictoria != null)
        {
            mensajeVictoria.SetActive(true);
        }

        // Espera 2 segundos reales (funciona incluso si otro script pausa el juego)
        yield return new WaitForSecondsRealtime(tiempoEsperaReinicio);

        ReiniciarProgreso();
    }

    public void ReiniciarProgreso()
    {
        Time.timeScale = 1f;

        // 1. Reinicia el contador a 0, quita el estado de victoria y actualiza el texto
        puntos = 0;
        juegoGanado = false;
        ActualizarUI();

        if (mensajeVictoria != null)
        {
            mensajeVictoria.SetActive(false);
        }

        // 2. Vuelve a activar las 7 monedas normales y la moneda final ("Meta")
        foreach (GameObject moneda in todasLasMonedas)
        {
            moneda.SetActive(true);
        }

        if (monedaMeta != null)
        {
            monedaMeta.SetActive(true);
        }

        // 3. Devuelve la pelota al punto de partida y frena su impulso
        transform.position = posicionInicial;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void ActualizarUI()
    {
        textoPuntaje.text = "Puntos: " + puntos + " / " + totalMonedas;
    }
}