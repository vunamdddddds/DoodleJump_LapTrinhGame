using UnityEngine;

public class Elevator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
      public float speed ;
    public float range ;

      private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {   float y = startY + Mathf.PingPong(Time.time * speed, range * 2) - range;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        
    }
}
