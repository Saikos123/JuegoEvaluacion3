using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraController : MonoBehaviour
{
    public GameObject jugador;
    public float sensibilidadGiro = 0.25f;
    public float velocidadZoom = 0.5f;
    public float distanciaMinima = 3f;
    public float distanciaMaxima = 18f;

    private Vector3 offset;

    void Start()
    {
        // Calcula la distancia inicial entre la cámara y el jugador
        offset = transform.position - jugador.transform.position;
    }

    void LateUpdate()
    {
        if (jugador == null) return;

        if (Mouse.current != null)
        {
            // 1. Giro horizontal con clic derecho mantenido
            if (Mouse.current.rightButton.isPressed)
            {
                float movimientoX = Mouse.current.delta.x.ReadValue() * sensibilidadGiro;
                offset = Quaternion.AngleAxis(movimientoX, Vector3.up) * offset;
            }

            // 2. Zoom con la rueda del ratón
            float scrollY = Mouse.current.scroll.y.ReadValue();
            if (scrollY != 0)
            {
                // Normalizamos el valor de la rueda (1 o -1)
                float direccionZoom = Mathf.Sign(scrollY);
                float distanciaActual = offset.magnitude;
                
                // Calculamos la nueva distancia respetando los límites mínimo y máximo
                float nuevaDistancia = Mathf.Clamp(distanciaActual - (direccionZoom * velocidadZoom), distanciaMinima, distanciaMaxima);
                
                // Aplicamos la nueva distancia manteniendo la dirección actual de la cámara
                offset = offset.normalized * nuevaDistancia;
            }
        }

        // Mantiene la posición relativa actualizada
        transform.position = jugador.transform.position + offset;

        // Hace que la cámara siempre mire hacia la esfera
        transform.LookAt(jugador.transform.position);
    }
}