<h1>Minesweeper Solver (README under construction)</h1>
<p>I quite like minesweeper and difficult programming challenges so I am doing my best to make an auto solver.<br>
Sometimes when I'm bored I add to it.</p>
<h2>Current Functionality:</h2>

<h3>Basic Logic</h3>
<p>if tile value is n and n unknown tiles are ajacent then all must be mines.
<br>This is trivial.


	Example: solve (Y, B).

      Z Y X
    A|0 1 1
    B|1 3 ?
    C|1 ? ?
	
 	Solution:

 	  Z Y X
    A|0 1 1
    B|1 3 F
    C|1 F F

</p>	

<h3>Orthoginal reasoning</h3>
<p>if the only difference in value between 2 orthoginally ajacent numbers is equal to the amount of hidden tiles and all other variables are known, then the rest must be mines (or none).
<br>This can be used to gain information; it is an uncommon scenario for this to solve in a case where the first method can't.</p>

	Example: solve (Y, B).

	  Z Y X
    A|1 F ?
    B|1 2 ?
    C|? ? ?
	
 	Solution:<br>

	  Z Y X
    A|1 F ?
    B|1 2 ?
    C|0 1 ?

</p>


<h3>Simulated Positioning (in development)</h3>
<p>if enough is known about the area, it can be possible to simulate each arrangement of mines and evaluate which arrangements are possible.
<br> My method will not be subject to risk; exec if there is only one fitting solution or if all solutions have a common mine then flag the position(s).</p>

	Example: solve (Y-X, B).

	  Z Y X W
    A|? ? ? ?
    B|? 2 5 ?
    C|? ? ? ?

 	Possible solution(s):
  
	  Z Y X W	|     Z Y X W 
    A|1 F 3 F	|   A|1 F 3 F
    B|2 2 5 F	|   B|1 2 5 F
    C|1 F 3 F	|   C|0 1 F F ... 4 total

	Solution:
 
 	  Z Y X W
    A|1 ? ? F
    B|2 2 5 F
    C|1 ? ? F
 	
