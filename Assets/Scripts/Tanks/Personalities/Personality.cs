using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Personality
{
    Chase, //Chases towards the player

    Flee, //Flees from the player

    Patrol, //Patrols a set region defined by waypoints. 
            //If it can hear the player, will stop and rotate towards him
            //If it can see the player, it will rotate towards him
            //If the player is directly within the enemy's line of sight, it will start shooting at the player

    Navigate, //Patrols a set region defined by waypoints
              //Unlike patrol, this mode ignores the player and will simply shoot continuously forward

    Strategic //Will chase the player when the tank is at full health, but will flee from the player if health drops below half

}

