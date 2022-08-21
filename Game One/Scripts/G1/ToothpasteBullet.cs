using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile shoot by the Toothpaste. Collides with Food only.
/// </summary>
public class ToothpasteBullet : MonoBehaviour 
{
	private new Collider2D collider;
	private new Rigidbody2D rigidbody;
	private Animator animator;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             Methods
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake ()
	{
		collider = GetComponent <Collider2D> ();
		rigidbody = GetComponent <Rigidbody2D> ();
		animator = GetComponent <Animator> ();
	}

	public void Init (float velocity)
	{
		rigidbody.velocity = Vector2.right * velocity;
		Destroy (gameObject, 5.0f);
	}

	private void Destroy ()
	{
		collider.enabled = false;
		rigidbody.velocity = Vector2.zero;
		animator.SetTrigger ("Death");
		Destroy (gameObject, 0.5f);
	}

	private void OnCollisionEnter2D (Collision2D hit)
	{
		if (hit.transform.CompareTag ("Food"))
		{
			if (hit.gameObject.TryGetComponent<Food> (out var food))
			{
				food.ReceiveDamage (new Damage (1, DamageType.Toothpaste));
				Destroy ();
			}
		}
	}
}