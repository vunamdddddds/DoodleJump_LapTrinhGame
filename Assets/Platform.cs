using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

	public float jumpForce;

	    private Animator GrountCrashAnim;

	public static int springShoeBootValue=0;

	void Start()
	{
		if (tag=="GrountCrash")
		{
			    GrountCrashAnim=GetComponent<Animator>();    
		}
	}
	void OnCollisionEnter2D(Collision2D collision)
	{


		if (collision.relativeVelocity.y <= 0f)//kiểm tra nếu nhân vật đang rơi xuống
		{
			// 
			Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				DeliverSfx();
				Vector2 velocity = rb.linearVelocity;
				velocity.y = jumpForce+springShoeBootValue;
				rb.linearVelocity = velocity;

			}
			if (tag=="GrountCrash")
			{
				 if (collision.gameObject.CompareTag("Player"))
        {
         
            GrountCrashAnim.SetTrigger("BreakTrigger");
			DeliverSfx();
            Destroy(gameObject, 0.18f); 
        }  
			}

			
		}
	}

	private void DeliverSfx()
	{
		string myTag = tag;
		switch (myTag)
		{
			case "GroundGreen":
				GamePlayManager.instance.Sfx(EnumSfxType.GroundGreen);
				break;
			case "GroundBlue":
				GamePlayManager.instance.Sfx(EnumSfxType.GroundBlue);
				break;
			case "Spring":
				GamePlayManager.instance.Sfx(EnumSfxType.Spring);
				break;
			case "Trampoline":
				GamePlayManager.instance.Sfx(EnumSfxType.Trampoline);
				break;	
			case "WinPoint":
				GamePlayManager.instance.Sfx(EnumSfxType.WinGame);
				break;
			case "GrountCrash":
				GamePlayManager.instance.Sfx(EnumSfxType.GrountCrash);
				break;	
			default:
				break;
		}

	}




}
