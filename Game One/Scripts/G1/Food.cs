using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Objects that appear in the mouth scenario, with abilities to give score
/// to the player and damage the teeth.
/// </summary>
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Rigidbody2D))]
public class Food : MonoBehaviour, IDamageable<Food>
{
    [Title("Properties")]
	[SerializeField]
    [Tooltip("Decides the result of colliding with teeth or being shot by the player.")]
	private Morality morality;
	
	/// <summary>
	/// Decides the result of colliding with teeth or being shot by the player.
	/// </summary>
	public Morality Morality 
	{ 
		get { return morality; }
		private set { morality = value; }
	}

    [SerializeField]
    [Tooltip("This will help avoiding collision with teeth that are close to the spawn area.")]
	private Verticality origin;

	/// <summary>
	/// This will help avoiding collision with teeth that are close to the spawn area.
	/// </summary>
	public Verticality Origin 
	{ 
		get { return origin; }
		private set { origin = value; }
	}
	
	[SerializeField]
    [Tooltip("Direction of movement that is applied constantly.")]
	private Vector2 velocity;

	/// <summary>
	/// Direction of movement that is applied constantly.
	/// </summary>
	public Vector2 Velocity
	{
		get { return velocity; }
		private set {
			velocity = value;
			rigidbody.velocity = velocity;
		}
	}

	/// <summary>
	/// Quick access to change the sorting order of the renderer.
	/// </summary>
	public int SortingOrder 
	{
		set 
		{
			renderer.sortingOrder = value;
		}
	}

	[Title ("Animation")]
	[SerializeField]
	[Tooltip ("Chance per second of triggering the blink animation.")]
	private float blinkChance = 0.66f;

	[SerializeField]
    [Tooltip("How many seconds it has to wait before blinking again.")]
	private float blinkCooldown = 2.0f;

	private float blinkTimer;

	private bool isDead;

	private new SpriteRenderer renderer;
	private new Rigidbody2D rigidbody;
	private Animator animator;

	private Damage lastDamageReceived;

	public Damage LastDamageReceived { get { return lastDamageReceived; } }

    public event UnityAction<Food> OnDestroy;

    /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    // MonoBehaviour
    /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    private void Awake ()
	{
		renderer = GetComponent <SpriteRenderer> ();
		rigidbody = GetComponent <Rigidbody2D> ();
		animator = GetComponent <Animator> ();
	}

    private void Start ()
    {
		// This code exists only for editor testing
        if (velocity != Vector2.zero) {
			Construct (velocity, origin);
        }
    }

    private void Update ()
	{
		if (Time.time > blinkTimer && isDead == false)
		{
			if (Random.value < (Time.deltaTime * blinkChance))
			{
				animator.SetTrigger ("Blink");
				blinkTimer = Time.time + blinkCooldown;
			}
		}
	}

	private void OnCollisionEnter2D (Collision2D hit)
	{
		if (isDead == true)
			return;

		if (hit.transform.CompareTag ("Tooth"))
		{
			if (hit.gameObject.TryGetComponent <Tooth> (out var tooth))
			{
				bool opposite = (tooth.Origin == Verticality.Up && Velocity.y > 0) || 
								(tooth.Origin == Verticality.Down && Velocity.y < 0);

				if (Morality == Morality.Bad && opposite) {
					tooth.ReceiveDamage (new Damage (1, DamageType.Food));
					ReceiveDamage (new Damage (1, DamageType.Tooth));
                }
			}
		}
	}

	/*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
	// Class
	/*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

	public void Construct (Vector2 velocity, Verticality verticality)
	{
		Velocity = velocity;
		Origin = verticality;

		if (Velocity.y > 0) {
			transform.localScale = new Vector3 (transform.localScale.x, -transform.localScale.y, transform.localScale.z);
		}
	}

	private void Die ()
	{
		if (isDead == true)
			return;

		isDead = true;
		rigidbody.velocity = Vector2.zero;
		OnDestroy?.Invoke (this);
		animator.SetTrigger ("Death");
		Destroy (gameObject, 0.25f);
	}

    public void ReceiveDamage (Damage source)
    {
		if (isDead == true)
			return;

		if (source.Type != DamageType.None)
        {
			lastDamageReceived = source;
			Die ();
        }
    }
}