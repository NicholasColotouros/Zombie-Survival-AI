using UnityEngine;
using System.Collections;

// Used to make sure that two zombies don't spawn in the same area
public class SpawnAreaCounter : MonoBehaviour 
{
	public int Counter = 0;
	private Object CounterLock;

	// Use this for initialization
	void Start () 
	{
		CounterLock = new Object ();
	}
	
	void OnTriggerEnter () 
	{
		IncrementCounter ();
	}

	void OnTriggerExit ()
	{
		DecrementCounter ();
	}

	public void IncrementCounter ()
	{
		lock(CounterLock)
		{
			Counter++;
		}
	}

	public void DecrementCounter ()
	{
		lock(CounterLock)
		{
			Counter--;
		}
	}
}
