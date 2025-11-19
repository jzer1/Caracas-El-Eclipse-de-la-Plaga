using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
