using UnityEngine;
using System.Collections;

//Destroy the sphere when the player enters
public class Collectable : MonoBehaviour 
{
	void OnCollisionEnter(Collision collision)
	{
		if( collision.transform.tag == "Player")
		{
			Destroy(gameObject);
		}
	}
}
