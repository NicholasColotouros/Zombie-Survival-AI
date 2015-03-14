using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour 
{
	public Transform player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		SeenByPlayer ();
	}

	public void SeenByPlayer ()
	{
		Vector3 playerPos = player.position;
		Vector3 guardPos = gameObject.transform.position + Vector3.up * 3;
		
		Vector3 playerDirection = playerPos - guardPos;

		RaycastHit hit;
		LayerMask mask = ~(1 << LayerMask.NameToLayer ("Zombies"));
		if(Physics.Raycast(guardPos, playerDirection, out hit, 200f, mask))
		{
			bool seen = false;
			if(hit.transform.tag == "Player")
			{
				seen = true;
			}

			Transform Base = gameObject.transform.FindChild("Base");
			if( seen )
			{
				Base.renderer.enabled = true;
			}
			else
			{
				Base.renderer.enabled = false;
			}
		}
	}
}
