using UnityEngine;

public class GrountCrash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Animator anim;


    void Start()
    {
    anim=GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
         
            anim.SetTrigger("BreakTrigger");
            Destroy(gameObject, 0f); // delay 0.3s
        }  
    }

}
