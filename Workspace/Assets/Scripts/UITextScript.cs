using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Displays success and failure messages
public class UITextScript : MonoBehaviour 
{
	private SurvivorController Survivor;
	private Initializer ZombieHorde;
	private Text UIText;
	private bool SimulationComplete = false;

	// Use this for initialization
	void Start () 
	{
		Survivor = GameObject.Find ("Survivor").transform.GetComponent<SurvivorController> ();
		ZombieHorde = GameObject.Find ("Level").transform.GetComponent<Initializer> ();
		UIText = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( ZombieHorde.SurvivorSpotted && ! SimulationComplete)
		{
			UIText.color = Color.red;
			UIText.text = "FAILURE";
			SimulationComplete = true;
		}
		else if( Survivor.TimeOut < 0  && ! SimulationComplete)
		{
			UIText.color = Color.red;
			UIText.text = "TIME UP";
			SimulationComplete = true;
		}
		else if (Survivor.CheckSuccess() && ! SimulationComplete)
		{
			UIText.color = Color.green;
			UIText.text = "SUCCESS";
			SimulationComplete = true;
		}
	}
}
