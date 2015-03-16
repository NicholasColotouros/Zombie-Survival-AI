using UnityEngine;
using System.Collections;

// Inherits from classic because aside from the movement speed,
// and lane changing (done in additional setup) the behaviour
// is the same.
public class Shambler : Classic 
{
	protected override void AdditionalSetup ()
	{
		Nav.speed = speed / 2;
		InvokeRepeating ("ChangeTracks", 0f, 2f);
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
		if( TrackNumber == 0 )
			Track = BlackBoard.OuterTrack;
		else if( TrackNumber == 1)
			Track = BlackBoard.MiddleTrack;
		else if( TrackNumber == 2)
			Track = BlackBoard.InnerTrack;

		// TODO: move into that lane

		Nav.SetDestination (Track [TrackIndex].position);

		// once destination is set:
		if( stopped )
		{
			stopped = false;
			Nav.Resume();
		}
	}
}
