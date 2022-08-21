using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central which retains information in between scene loads.
/// Also responsible for loading new levels.
/// </summary>
//[CreateAssetMenu (fileName = "PersistentCentral", menuName = "ScriptableObjects/CentralObject", order = 1)]
public class MainCentral : ScriptableObject
{
	public Difficulty G1Difficulty;

	public static MainCentral Instance
    {
		get
        {
			if (instance == null)
            {
				MainCentral central = Resources.Load<MainCentral> ("MainCentral");
				instance = central;
            }
			return instance;
        }
		private set { instance = value; }
    }

	private static MainCentral instance;

	public G1Settings G1Settings
	{ 
		get
		{
            switch (G1Difficulty)
            {
				default:
				case Difficulty.Easy: return easySettings;
                case Difficulty.Medium: return mediumSettings;
                case Difficulty.Hard: return hardSettings;
            }
        }
	}

	[SerializeField]
	private G1Settings easySettings;

	[SerializeField]
	private G1Settings mediumSettings;

	[SerializeField]
	private G1Settings hardSettings;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             Methods
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake ()
    {
		Instance = this;
    }

    private void OnEnable ()
    {
		Instance = this;
    }

    public void LoadLevel (SceneName scene)
	{
		//Time.timeScale = 1;

		switch (scene)
		{
			case SceneName.GameSelection:
			{
				SceneManager.LoadScene (0);
                break;
			}

			case SceneName.G1: {
				SceneManager.LoadScene (1);
				break;
			}

			case SceneName.G2_Story: {
				SceneManager.LoadScene (2);
				break;
			}
		}
	}

	public void LoadGame1 (Difficulty difficulty)
	{
        SceneManager.LoadScene (1);
	}
}