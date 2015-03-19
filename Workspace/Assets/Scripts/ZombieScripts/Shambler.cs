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
		InvokeRepeating ("ChangeTracks", 2f, 2f);
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
			// outer/inner lanes can only go to middle
			if( TrackNumber == 0 || TrackNumber == 2)
				SwitchToLane(1);

			// middle lane can go to either other lane
			if( TrackNumber == 1 ) // if in middle track
			{
				diceRoll = Random.Range(0, 2);
				if( diceRoll == 0 ) SwitchToLane(0);
				else SwitchToLane(2);
			}
		}
	}

	protected void SwitchToLane(int n)
	{
		int oldTrackIndex = TrackIndex;

		if( n == 0 )
		{
			Track = BlackBoard.OuterTrack;
			TrackNumber = 0;
		}
		else if( n == 1)
		{
			Track = BlackBoard.MiddleTrack;
			TrackNumber = 1;
		}
		else if( n == 2)
		{
			Track = BlackBoard.InnerTrack;
			TrackNumber = 2;
		}

		MoveIntoLane (TrackIndex, oldTrackIndex);

		// once destination is set:
		if( stopped )
		{
			stopped = false;
			Nav.Resume();
		}
	}

	protected void MoveIntoLane( int dest, int origin)
	{
		// TODO: bugged, fix
		Nav.Stop ();
		if( origin < dest )
		{
			gameObject.transform.Rotate( new Vector3(0,-45,0));
		}

		else 
		{
			gameObject.transform.Rotate( new Vector3(0,45,0));
		}

		Vector3 nextlanecenter = gameObject.transform.position + gameObject.transform.forward * 4;
		Nav.SetDestination (nextlanecenter);
		CancelInvoke ("ChangeTracks");
		InvokeRepeating ("ChangingTracks", 0f, 0.25f);
	}

	protected void ChangingTracks()
	{
		// Check to see if arrived
		Vector3 pos = gameObject.transform.position;
		if( pos.x == Nav.destination.x && pos.z == Nav.destination.z)
		{
			Nav.SetDestination( Track[TrackIndex].position);
			CancelInvoke("ChangingTracks");
			InvokeRepeating ("ChangeTracks", 2f, 2f);
		}
	}
}
