using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    
    public enum SenseType
    {
        Sight,
        Hearing,
        Smell
    };


    public static class SenseEvent
    {
    	
        static float SENSE_RANGE = 2.0f;

        public static void Sense(Agent sensor, Agent sensee)
        {	//commented out to prevent failure due to null object
        	List<Vector2> path;
        	//path = Astar.FindPath(BoardManager.getSenseGrid(), sensor.agentPos, sensee.agentPos);
      
            if (Vector2.Distance(sensor.agentPos, sensee.agentPos) < SENSE_RANGE)
            {
				sensor.HandleSenseEvent(sensee);
            }                         
        }
    }

