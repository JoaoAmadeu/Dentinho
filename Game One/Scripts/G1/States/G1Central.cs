using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Class to handle all the GameStates, which will create the gameplay.
/// Also handle the player score and the pause state.
/// </summary>
public class G1Central : MonoBehaviour
{
	[Title ("States")]
	private G1Settings settings;

	[SerializeField]
	private int stateIndex;

	[SerializeField]
	private GameState[] states;

	private int score;

	private bool paused;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             Methods
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake ()
	{
		score = 0;
	}

	private void Start ()
	{;
		settings = MainCentral.Instance.G1Settings;
		StartState ();
	}

	private void StartNextState ()
    {
		stateIndex++;
		StartState ();
    }

	private void StartState ()
    {
		if (stateIndex < states.Length && stateIndex >= 0) {
			states [stateIndex].StateStart (settings, StartNextState);
		}
	}

    private void Update ()
    {
		if (stateIndex < 0 || stateIndex >= states.Length)
			return;

		states [stateIndex].StateUpdate ();

		// KeyCode.Escape also represents the back key of a cellphone
		if (Input.GetKeyDown (KeyCode.Escape)) {
            TogglePauseState ();
        }
    }

    public void Quit ()
	{
		//GameController.LoadLevel (GameController.GameLevel.Menu);
	}

	public void AddScore (int value)
    {
		score += value;
    }

	public void SetGameOver ()
	{
		Time.timeScale = 0;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void GameOver ()
    {
		Debug.Log ("game over");
    }

	public void TogglePauseState ()
	{
		paused = !paused;

		Time.timeScale = 0;
	}
}