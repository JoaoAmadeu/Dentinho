using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// State that spawns Food waves, each in an interval of time. Will also
/// change the player score based on which food is destroyed.
/// </summary>
[RequireComponent (typeof (AudioSource))]
public class SpawnWaveState : GameState
{
    [Title("Wave properties")]
	[SerializeField]
    [Tooltip("How many waves will be spawned.")]
	private int repeat;

	[SerializeField]
    [Tooltip("Time between each wave.")]
	private float interval;

	[SerializeField]
    [Tooltip("Movement speed of the food.")]
	private float speed;

	[SerializeField]
    [Tooltip("Sound which will be played when a wave is spawned.")]
	private AudioClip spawnSound;

	[Title ("Food")]
	[SerializeField]
	[Tooltip ("Sound which will be played when good food is destroyed.")]
	private AudioClip goodFoodDeathSound;

	[SerializeField]
	[Tooltip ("Sound which will be played when a bad food is destroyed.")]
	private AudioClip badFoodDeathSound;

	[SerializeField]
	[Tooltip ("Food factory.")]
	private FoodFactory factory;

	[SerializeField]
	[Tooltip ("A negative five sprite, spawned when a good food is destroyed.")]
	private GameObject negativeFivePrefab;

	[SerializeField]
	[Tooltip ("A positive ten sprite, spawned when a bad food is destroyed.")]
	private GameObject positiveTenPrefab;

    [Title ("Others")]
    [SerializeField]
    [ReadOnly]
    [Tooltip("Timer for the wave spawn.")]
	private float timer;

    [SerializeField]
	private G1Central central;

	[SerializeField]
	private Tooth [] teeth;

	/// <summary>
	/// How many food will be spawned in a wave.
	/// </summary>
	private int repeatCount;

	private AudioSource audioSource;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             Methods
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake ()
    {
		audioSource = GetComponent <AudioSource> ();
    }

    public override void StateStart (G1Settings settings, UnityAction endCallback = null)
    {
        base.StateStart (settings, endCallback);
        foreach (var item in teeth) {
			item.OnDestroy += ToothDestroyedCallback;
        }

		CreateWave ();
    }

    public override void StateUpdate () 
	{
		timer += Time.deltaTime;

		if (timer > interval)
		{
			if (repeatCount >= repeat) {
				repeatCount = 0;
				StateEnd ();
			}
			else {
				CreateWave ();
			}
		}
	}

    public override void StateEnd ()
    {
		base.StateEnd ();
		foreach (var item in teeth) {
			item.OnDestroy -= ToothDestroyedCallback;
		}
	}

	private void CreateWave ()
	{
		bool isUp = Random.value > 0.5f ? true : false;
		Vector2 velocity = new Vector2 (0, isUp ? -speed : speed);

		Food [] spawn = factory.SpawnWave (settings.spawnCount, isUp, velocity);
		audioSource.clip = spawnSound;
		audioSource.Play ();

		foreach (var item in spawn) {
			item.OnDestroy += FoodDeathCallback;
		}

		timer = 0.0f;
		repeatCount++;
	}

	private void ToothDestroyedCallback (Tooth tooth)
    {
		central.GameOver ();
    }

	/// <summary>
	/// When a food with good morality is destroyed, the player will receive minus five points in the score.
	/// When a food with bad morality is destroyed, the player will receive plus ten points in the score.
	/// </summary>
	/// <param name="food"></param>
	private void FoodDeathCallback (Food food)
    {
		if (food.LastDamageReceived.Type == DamageType.Toothpaste)
        {
			if (food.Morality == Morality.Bad)
            {
				central.AddScore (10);
				var goodScore = Instantiate (positiveTenPrefab, food.transform.position, Quaternion.identity);
				var audio = goodScore.AddComponent<AudioSource> ();
				audio.clip = badFoodDeathSound;
				audio.spatialBlend = 0;
				audio.Play ();
			}
			else if (food.Morality == Morality.Good)
            {
				central.AddScore (-5);
				var goodScore = Instantiate (negativeFivePrefab, food.transform.position, Quaternion.identity);
				var audio = goodScore.AddComponent<AudioSource> ();
				audio.clip = goodFoodDeathSound;
				audio.spatialBlend = 0;
				audio.Play ();
			}
        }
    }
}