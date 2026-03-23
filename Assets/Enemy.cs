using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float range = 5f;
    private float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        float x = startX + Mathf.PingPong(Time.time * speed, range * 2) - range;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}