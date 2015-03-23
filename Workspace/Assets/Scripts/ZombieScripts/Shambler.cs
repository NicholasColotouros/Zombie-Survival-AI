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
		InvokeRepeating ("ChangeTracks", 5f, 5f);
	}
	
	protected void ChangeTracks()
	{
		int diceroll = Random.Range (0, 2);

		if( diceroll == 1 )
		{
			int oldTrackNumber = TrackNumber;

			// figure out which track we're switching to
			switch(TrackNumber)
			{
			case(0):
				TrackNumber = 1;
				Track = BlackBoard.MiddleTrack;
				break;

			case(1): // in the middle track
				diceroll = Random.Range(0,2);
				if( diceroll == 1 )
				{
					TrackNumber = 0;
					Track = BlackBoard.OuterTrack;
				}
				else
				{
					TrackNumber = 2;
					Track = BlackBoard.InnerTrack;
				}
				break;
			case(2):
				TrackNumber = 1;
				Track = BlackBoard.MiddleTrack;
				break;
			default:
				break;			
			}

			// now switch to it
			Vector3 translationVector = new Vector3(4, 0, 0);

			float modifier = 1;
			if(direction == Initializer.ZombieMovementDirection.Clockwise)
				modifier = -1;

			if( oldTrackNumber > TrackNumber )
				gameObject.transform.Translate(translationVector * modifier);
			else if( oldTrackNumber < TrackNumber )
				gameObject.transform.Translate(translationVector * modifier * -1f);

			lock(NavLock){Nav.SetDestination(Track[TrackIndex].position);}
		}
	}
}
