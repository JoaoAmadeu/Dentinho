using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Will spawn bubbles in front of the teeth and a toothbrush to clean these bubbles.
/// Player input will move the toothbrush and once all the bubbles from a tooth is
/// clean, the tooth will regain one life.
/// </summary>
public class SpawnBrushAndBubblesState : GameState
{
    [SerializeField]
    private G1_Toothbrush toothBrushInstance;

    [SerializeField]
    private Vector3 toothBrushSpawnPoint;

    [SerializeField]
    private Tooth [] teeth;

    [SerializeField]
    private float bubbleSpawnDelay = 1.5f;

    public override void StateStart (G1Settings settings, UnityAction endCallback = null)
    {
        base.StateStart (settings, endCallback);

        toothBrushInstance.gameObject.SetActive (true);
        toothBrushInstance.transform.position = toothBrushSpawnPoint;
        toothBrushInstance.Init ();

        StartCoroutine (LateBubbleSpawn ());
    }

    private IEnumerator LateBubbleSpawn ()
    {
        yield return new WaitForSeconds (bubbleSpawnDelay);

        foreach (var item in teeth)
        {
            item.SpawnBubbles ();
        }
    }

    public override void StateEnd ()
    {
        base.StateEnd ();
        toothBrushInstance.gameObject.SetActive (false);
    }
}
