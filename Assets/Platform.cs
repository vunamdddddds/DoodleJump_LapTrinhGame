using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

	public float jumpForce;

	void Start()
	{
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
				velocity.y = jumpForce;
				rb.linearVelocity = velocity;

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
			default:
				break;
		}

	}




}
