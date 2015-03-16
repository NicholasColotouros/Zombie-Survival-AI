using UnityEngine;
using System.Collections;

// Sets a boolean if the survivor enters the field of vision
// which indicates that he has been spotted
public class VisionTrigger : MonoBehaviour
{
	public bool SurvivorEntered;

	// Use this for initialization
	void Start () 
	{
		SurvivorEntered = false;
	}

	void OnTriggerEnter( Collider other )
	{
		if (other.tag == "Player")
			SurvivorEntered = true;
	}
}
