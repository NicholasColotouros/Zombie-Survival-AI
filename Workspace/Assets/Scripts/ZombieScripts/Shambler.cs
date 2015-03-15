using UnityEngine;
using System.Collections;

public class Shambler : Zombie 
{
	protected override void AdditionalSetup ()
	{
		Nav.speed = speed / 2;
		InvokeRepeating ("ChangeTracks", 0f, 2f);
	}
	
	protected override void ZombieMovement ()
	{
		// TODO stop when a zombie gets too close
	}

	protected void ChangeTracks()
	{
		if( SurvivorSpotted )
		{
			CancelInvoke("ChangeTracks");
			return;
		}
		int diceRoll = Random.Range (0, 2);

		if( diceRoll == 1 )
		{
			if( TrackNumber == 0 )
			{
				SwitchToLane(1);
			}

			else if( TrackNumber == 1)
			{
				diceRoll = Random.Range(0, 2);
				if( diceRoll == 0 )
				{
					SwitchToLane(0);
				}
				else
				{
					SwitchToLane(2);
				}
			}

			else
			{
				SwitchToLane(1);
			}
		}
	}

	protected void SwitchToLane(int n)
	{
		// TODO
	}
}
