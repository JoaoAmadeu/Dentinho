using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Object spawned by the tooth. When cleaned, it will increase the tooth life.
/// </summary>
public class ToothBubble : MonoBehaviour, IDamageable<ToothBubble>
{
    public event UnityAction<ToothBubble> OnDestroy;

    private new SpriteRenderer renderer;

	private void Awake ()
	{
		renderer = GetComponent <SpriteRenderer> ();
	}

	public void Activate ()
	{
		gameObject.SetActive (true);
	}

	public void Deactivate ()
	{
		gameObject.SetActive (false);
	}

    public void ReceiveDamage (Damage source)
    {
		OnDestroy?.Invoke (this);
		Deactivate ();
	}
}