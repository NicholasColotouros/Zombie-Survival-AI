using UnityEngine;
using System.Collections;

//Destroy the sphere when the player enters
public class Collectable : MonoBehaviour 
{
	void OnTriggerEnter(Collider collision)
	{
		if( collider.transform.tag == "Player")
			Destroy(gameObject);
	}
}
