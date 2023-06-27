using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.24f, -1.5f);

    [Range(0.01f, 1f)] [SerializeField] private float smoothSpeed = 0.125f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desirePosition = player.position + offset; 

        transform.position = Vector3.SmoothDamp(transform.position, desirePosition, ref velocity, smoothSpeed);
    }

    public void CenterOnTarget()
    {
        transform.position = player.position + offset;
    }
}
