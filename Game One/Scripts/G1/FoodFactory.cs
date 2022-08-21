using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Class with factory pattern to handle Food lifetime.
/// </summary>
public class FoodFactory : MonoBehaviour
{
	[Title ("Prefabs")]
	[SerializeField]
	private Food[] badFoodPrefabs;

	[SerializeField]
	private Food[] goodFoodPrefabs;

	[Title ("Spawn Positions")]
	[SerializeField]
    [Tooltip("Food will spawn only inside this boundaries.")]
	private Rect spawnBounds;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             Methods
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	// Will show a gray box representing the area where the food will spawn.
    private void OnDrawGizmosSelected ()
	{
		GUIStyle style = new GUIStyle (GUI.skin.label);
		style.normal.textColor = Color.red;
		style.fontStyle = FontStyle.Bold;
		Handles.Label (spawnBounds.min, new GUIContent ("Spawn Box"), style);

		Gizmos.color = new Color (1f, 1f, 1f, 0.5f);
		Gizmos.DrawCube (spawnBounds.center, spawnBounds.size);
		Gizmos.color = Color.white;
	}

	private Vector2 SpawnBoundsRandomPosition (bool isUp)
	{
		return new Vector2 (Random.Range (spawnBounds.xMin + 0.7f, spawnBounds.xMax - 0.7f),
							isUp ? spawnBounds.yMin : spawnBounds.yMax);
	}

	/// <summary>
	/// Spawn any number of food, with parameters to adjust it's trajectory.
	/// </summary>
	/// <param name="count">The quantity of food spawned.</param>
	/// <param name="isUp">Will the food come from above</param>
	/// <param name="velocity">The constant velocity of the food</param>
	/// <param name="badProbability">The chance of spawning bad food, otherwise it will spawn good food.</param>
	/// <returns></returns>
	public Food [] SpawnWave (int count, bool isUp, Vector2 velocity, float badProbability = 0.75f)
    {
		Food[] wave = new Food [count];
        for (int i = 0; i < wave.Length; i++)
        {
			bool isBad = Random.value > badProbability ? false : true;
			wave [i] = SpawnFood (isUp, isBad);
			wave [i].Construct (velocity, isUp ? Verticality.Up : Verticality.Down);
			wave [i].SortingOrder = isUp ? 8 : 6;
        }

		// Avoid spawning on top of each other
        if (count == 2)
        {
			Vector2 rectPosition = new Vector2 (wave [1].transform.position.x - 0.7f, wave [1].transform.position.y - 0.7f);
			Rect foodRect = new Rect (rectPosition, new Vector2 (1.4f, 1.4f));
			Vector2 position = wave [0].transform.position;

			while (foodRect.Contains (position)) {
				position = SpawnBoundsRandomPosition (isUp);
            }
			wave [0].transform.position = position;
		}

		return wave;
	}

	/// <summary>
	/// Spawn a single food with properties set.
	/// </summary>
	/// <param name="isUp"></param>
	/// <param name="isBad"></param>
	/// <returns></returns>
	public Food SpawnFood (bool isUp, bool isBad)
    {
		Food food;
		Food [] prefabs = isBad ? badFoodPrefabs : goodFoodPrefabs;
		food = Instantiate (prefabs [Random.Range (0, prefabs.Length)], SpawnBoundsRandomPosition (isUp), Quaternion.identity);

		return food;
	}
}