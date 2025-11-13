using UnityEngine;

public class LanternaFollow : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;

    void Start()
    {
        target = Camera.main.transform;
    }

    void Update()
    {
        if (target == null) return;
        transform.position = target.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, speed * Time.deltaTime);
    }
}
