using UnityEngine;

public class RotatorYColeccionable : MonoBehaviour
{
    void Update()
    {
        // Gira suavemente sobre el eje vertical del mundo (como una moneda de pie)
        transform.Rotate(new Vector3(0, 100, 0) * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Jugador")
        {
            gameObject.SetActive(false);
        }
    }
}