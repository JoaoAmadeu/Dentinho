using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Toothbrush that will clean the bubbles that will appear on the teeth.
/// </summary>
public class G1_Toothbrush : MonoBehaviour
{
	private Animator animator;

	private void Awake ()
	{
		animator = GetComponentInChildren<Animator> ();
	}

	private void Update ()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		pos.z = 0.0f;
		transform.position = pos;
	}

	public void Init ()
	{
		if (animator == null)
			Awake ();

		animator.SetTrigger ("Spawn");
	}

	private void OnTriggerEnter2D (Collider2D hit)
	{
		if (hit.CompareTag ("Bubble"))
		{
			if (hit.TryGetComponent<IDamageable<ToothBubble>> (out var bubble))
			{
				bubble.ReceiveDamage (new Damage (1, DamageType.Toothbrush));
			}
		}
	}
}