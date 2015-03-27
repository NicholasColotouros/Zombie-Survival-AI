The zombie prefab I used is a free asset from the unity asset store.

======================================
============== CONTROLS ==============
======================================

+------------+-------+
|   ACTION   |  KEY	 | 
+------------+-------+
| Move Up    |   W   | 
| Move Down  |   S   |
| Move Left  |   A   | 
| Move Right |   D   |
| Zoom In    |   E   |
| Zoom OUT   |   Q   |
+------------+-------+

If you click on the camera game object, you can adjust how fast 
the camera moves and zooms.


======================================
============= PARAMETERS =============
======================================
On the Level game object you'll find a script which contains all 
global variables that can be adjusted according to the assignment 
specifications.

P is measured in percent, and therefore must be a number between 
0 and 100 inclusively. It is the percent chance that a zombie will 
despawn upon reaching a lane corner. When a zombie despawns, another 
one will respawn as soon as a corner is liberated so a zombie can 
spawn.

The ratio R, is how many hard zombies there are per easy zombie. So 
0.25 means there are 4 easy zombies per hard zombie.



======================================
=============== ZOMBIES ==============
======================================

+------------+--------+
|   Zombie   | Colour | 
+------------+--------+
| Classic    | Blue   | 
| Shambler 	 | Yellow |
| Modern  	 | Black  | 
| Cell Phone | Red	  |
+------------+--------+

All zombie behaviors inherit the Zombie script. All that is changed
is that the Zombie script has two abstract methods which can be
overridden: AdditionalSetup and ZombieMovement which are rather self
explanatory. The reason for this is that all zombies have identical
behaviours for everything except what happens while they move towards
their destination.

The non-modern zombies stop when there is a zombie close direcly in
front of them. This logic is implemented in the Classic zombie and
Shambler inherits it (Cell Phone inherits Shambler). This leads
to some staggering looking because it's all or nothing speed while
they stop and start as they stay behind a slower zombie. 

Another minor issue is that sometimes (very rarely) is that you'll 
have a bunch of zombies turn in unisson and see each other's backs 
and then stop while turning as they identify it as close. However 
this is always resolved within a second or two as the outer zombie
moves out, switches lanes, changes directions or stops looking at
it's phone.

One thing I feel the need to point out about the cell phone zombies
is that sometimes they'll stop in really odd places (like on a corner
where a spawn point is while in the middle of turning) that makes them
look like they've broken when in reality it's just because they're
supposed to stop for 3 seconds.



======================================
========== SURVIVOR STRATEGY =========
======================================

It takes about 20 seconds for the AI to complete the game without any
zombies at v = 20 for the zombies (the survivor has 1.5x that, as per
specs). So the timeout on the survivor is 60.

The direction of the AI is opposite the general direction of the zombies.

The strategy used for my AI is that if it can see a zombie coming
towards it or if a zombie is too close, it will go to the nearest safe 
spot (an alcove, the start or goal areas). Otherwise it will go towards 
the next collectable.

When the survivor can see a zombie, an orange circle appears below
the zombie.

When time runs out or the zombie is spotted, the survivor stops moving.

One issue with the strategy is that sometimes the survivor will hide
in an alcove and get spotted by a zombie in the inner lane when peeking
around a corner.

So, barring the above paragraph, it will almost always work out
with any configuration of two zombies. Three zombies will sometimes 
succeed but it frequely times out due to the AI being overly cowardly.
Increasing the desawn rate seems to improve its odds and when there are
modern zombies it tends to work out a little better because the AI will
peak out less. Sadly this is balanced out by the cell phone zombies
stopping which makes the AI hide for a very long time.