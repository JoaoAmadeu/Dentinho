using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that shoots projectiles, which collide with Food.
/// </summary>
public class Toothpaste : MonoBehaviour 
{
    [Title ("Boundaries")]
	[SerializeField]
    [Tooltip("How far it can go from the initial position. Same distance upwards and downards.")]
	private float verticalLimit = 2;

	[SerializeField]
    [Tooltip("When switching sides, how many units it will it travel to meet the screen end.")]
	private float horizontalLimit = -15;

	[SerializeField]
    [Tooltip("Position representing the center of the screen.")]
	private Vector2 screenCenter;

	[Title ("Bullet")]
	[SerializeField]
    [Tooltip("Speed of the projectile.")]
	private float bulletSpeed = 4f;
	
	[SerializeField]
    [Tooltip("How much time will it take to shoot again.")]
	private float bulletCooldown = 1.0f;

	[SerializeField]
    [Tooltip("Projectile prefab.")]
	private ToothpasteBullet bulletPrefab;

	[SerializeField]
    [Tooltip("Transform representing the initial location the projectile will spawn.")]
	private Transform bulletSpawnTransform;

	private float bulletCooldownTimer;

	private bool isLeftSide = true;

	private bool isReceivingInput = true;

	private Animator animator;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             MonoBehaviour
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake ()
	{
		animator = GetComponent <Animator> ();
	}

	private void Start ()
	{
		bulletCooldownTimer = bulletCooldown;

		// After a certain delay of the animation, spawn the projectile
		AnimationClip[] animCLips = animator.runtimeAnimatorController.animationClips;
		AnimationEvent shootEvent = new AnimationEvent ();
		shootEvent.functionName = "SpawnBullet";
		shootEvent.time = 0.07f;

		foreach (AnimationClip clip in animCLips)
		{
			if (clip.name.Contains ("Shoot")) {
				clip.AddEvent (shootEvent);
			}
		}
	}

	// Input for movement and shooting will be catch inside this method.
	private void Update ()
	{
		if (isReceivingInput == false)
			return;

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch (0);

			// Move the cube if the screen has the finger moving.
			if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
			{
				CheckForFlip (touch);
			}

			if (touch.phase == TouchPhase.Ended)
			{
				if (bulletCooldownTimer >= bulletCooldown)
				{
					animator.SetTrigger ("Shoot");
				}
			}
		}

		if (bulletCooldownTimer < bulletCooldown)
		{
			bulletCooldownTimer += Time.deltaTime;
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
			if (bulletCooldownTimer >= bulletCooldown)
			{
				animator.SetTrigger ("Shoot");
			}
		}
	}

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             Class
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void SpawnBullet ()
	{
		var rotation = isLeftSide == true ? Quaternion.identity : Quaternion.Euler (new Vector3 (0, 0, 180));
		var bulletInstance = Instantiate (bulletPrefab, bulletSpawnTransform.position, rotation);
		float bulletDirection = isLeftSide == true ? bulletSpeed : bulletSpeed * -1;

		bulletInstance.Init (bulletDirection);
		bulletCooldownTimer = 0.0f;
	}

	/// <summary>
	/// If the touch is from the opposite side of this instance, it will change place
	/// to be on the exact opposite side.
	/// </summary>
	/// <param name="touch">Data to get the screen position in pixels of the touch.</param>
	private void CheckForFlip (Touch touch)
    {
		if ((touch.position.x < Screen.width / 2) && isLeftSide == false)
		{
			transform.position = new Vector3 (screenCenter.x - horizontalLimit,
											  transform.position.y,
											  transform.position.z);

			transform.localScale = new Vector3 (-transform.localScale.x,
												transform.localScale.y,
												transform.localScale.z);
			isLeftSide = true;
		}
		else if ((touch.position.x > Screen.width / 2) && isLeftSide == true)
		{
			transform.position = new Vector3 (screenCenter.x + horizontalLimit,
											  transform.position.y,
											  transform.position.z);

			transform.localScale = new Vector3 (-transform.localScale.x,
												transform.localScale.y,
												transform.localScale.z);
			isLeftSide = false;
		}

		float alphaHeight = touch.position.y / Screen.height;
		float height = Mathf.Lerp (screenCenter.y - verticalLimit, screenCenter.y + verticalLimit, alphaHeight);
		transform.position = new Vector3 (transform.position.x, height, transform.position.z);
    }
}
