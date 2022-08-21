using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Objects that will appear inside the mouth background. Can be damaged by food and
/// cleaned by a toothbrush.
/// </summary>
public class Tooth : MonoBehaviour, IDamageable<Tooth>
{
    [Title ("Properties")]
	[SerializeField]
    [Tooltip("Current life")]
	private int life = 3;

	[SerializeField]
    [Tooltip("Maximum number of lifes.")]
	private int maxLife = 3;

	[SerializeField]
    [Tooltip("The color of the teeth, depending of the number of lifes left.")]
	private Color [] lifeColor;

	[SerializeField]
    [Tooltip("Member to help avoiding collision with recently spawned objects.")]
	private	Verticality origin;

	/// <summary>
	/// Member to help avoiding collision with recently spawned objects.
	/// </summary>
	public Verticality Origin { get { return origin; } }

    [Title("Bubbles")]
	[SerializeField]
    [Tooltip("Objects that will be spawned. If the toothbrush clean them, it will increase the life count.")]
	private ToothBubble bubblePrefab;

	[SerializeField]
	[Tooltip ("Number of bubbles spawned at the same time.")]
	private int bubbleSpawnCount = 8;

	[SerializeField]
    [Tooltip("The bubbles will be spawned in a circle, each at a specific angle. This is the circle radius.")]
	private float bubbleSpawnRadius = 0.33f;

	/// <summary>
	/// How many bubbles have been destroyed. When it reaches 0, it will spawn all the bubbles again.
	/// </summary>
	private int bubblesConsumed;

	private new SpriteRenderer renderer;

	private ToothBubble[] bubbles;

	public event UnityAction<Tooth> OnDestroy;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             Methods
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake ()
	{
		renderer = GetComponent <SpriteRenderer> ();

		bubbles = new ToothBubble [bubbleSpawnCount];
		UpdateColor ();

		// Spawn the bubbles in a circle, each at given angle, deactivate them and apply
		// the correct callback to when they are wiped.
		for (int i = 0; i < bubbleSpawnCount; i++)
		{
			float angle = (360 / bubbleSpawnCount * i) * Mathf.Deg2Rad;
			Vector3 pos = transform.position;
			pos += new Vector3 (Mathf.Sin (angle), Mathf.Cos (angle), 0) * bubbleSpawnRadius;

			bubbles [i] = Instantiate (bubblePrefab, pos, Quaternion.identity, transform) as ToothBubble;
			bubbles [i].OnDestroy += BubbleDestroyed;
			bubbles [i].Deactivate ();
		}
	}

	/// <summary>
	/// Callback for when a bubble is cleaned. When all the bubbles from this tooth is cleaned,
	/// it will respawn again after a delay.
	/// </summary>
	/// <param name="bubble"></param>
	private void BubbleDestroyed (ToothBubble bubble)
    {
		bubblesConsumed++;

		if (bubblesConsumed >= bubbleSpawnCount)
		{
			StartCoroutine (DelayedSpawn ());

			life += 1;
			if (life >= maxLife) {
				life = maxLife;
			}	

			UpdateColor ();
		}
	}

    private IEnumerator DelayedSpawn ()
	{
		yield return new WaitForSeconds (0.2f);
		SpawnBubbles ();
	}

	public void SpawnBubbles ()
	{
		bubblesConsumed = 0;

		foreach (ToothBubble bubble in bubbles)
			bubble.Activate ();
	}

	public void DestroyBubbles ()
	{
		StopCoroutine (DelayedSpawn ());

		foreach (ToothBubble bubble in bubbles)
			bubble.Deactivate ();
	}

    public void ReceiveDamage (Damage source)
    {
		if (life > 0)
        {
			life -= 1;
			UpdateColor ();

			if (life <= 0) {
				OnDestroy?.Invoke (this);
			}
		}
	}

	public void UpdateColor ()
    {
		renderer.color = lifeColor [maxLife - life];
	}
}