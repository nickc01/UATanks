using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PatrolLoopMode
{
    End, //Will stop patrolling when it has reached the last point
    Loop, //Will loop back to the start of the point list
    PingPong //When it has reached the end of the list, it will start working backwards. It will go back and forth in the list
}
