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

}

