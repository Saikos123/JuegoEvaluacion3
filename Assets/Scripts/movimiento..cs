using UnityEngine;
using UnityEngine.InputSystem;

public class movimiento : MonoBehaviour
{
    public float velocidad = 10f;
    public float fuerzaSalto = 6f;

    private Rigidbody rb;
    private bool enElSuelo = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Salto controlado solo cuando toca el suelo
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && enElSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            enElSuelo = false;
        }
    }

    void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        float moverX = 0f;
        float moverZ = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moverZ += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moverZ -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moverX += 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moverX -= 1f;

        // Obtenemos hacia dónde mira la cámara actualmente (ignorando la inclinación vertical)
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Calculamos la dirección final relativa a la vista de la cámara
        Vector3 direccionMovimiento = (camForward * moverZ) + (camRight * moverX);

        rb.AddForce(direccionMovimiento.normalized * velocidad);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detecta cuando vuelve a tocar una plataforma o suelo para habilitar el salto nuevamente
        if (collision.contacts[0].normal.y > 0.5f)
        {
            enElSuelo = true;
        }
    }
}