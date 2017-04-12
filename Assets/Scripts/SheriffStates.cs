using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GlobalSheriffState : State<Sheriff>
{
    public GlobalSheriffState(){}

    public override void Enter(Sheriff agent)
    {
        
    }

    public override void Execute(Sheriff agent)
    {
    	agent.CheckSurroundings();
    	
    }

    public override void Exit(Sheriff agent)
    {

    }

}


public class WorkState : State<Sheriff>
{
    public WorkState(){}

    public override void Enter(Sheriff agent)
    {
        Debug.Log("Back in the office working again");
    }

    public override void Execute(Sheriff agent)
    {
        agent.IncreaseWorkedTime(1);
		Debug.Log("Working for so long in the office is boring");
		if (agent.WorkedLongEnough()) agent.ChangeState(new PatrolState());
    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log(" Time to Patrol");
    }
}

public class PatrolState : State<Sheriff>
{
	Vector2 dest;
	Vector2 currLocation;
    public PatrolState(){}

    public override void Enter(Sheriff agent)
    {
        Debug.Log("On patrol");
    }

    public override void Execute(Sheriff agent)
    {
    	dest=BoardManager.locationsPos[agent.getNextPatrol()];
    	currLocation= agent.agentPos;
    	if (currLocation==dest){
    		dest=BoardManager.locationsPos[agent.getNextPatrol()];
    	}
    	agent.ChangeState(new SheriffTravelState (dest));
    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Checking Out New Area");
    }

}

public class SheriffTravelState : State<Sheriff>
{
	public static Vector2 dest;
	List<Vector2> path;

	public SheriffTravelState (Vector2 theDest) {
		dest=theDest;
	}

	public override void Enter (Sheriff agent) {
		path = Astar.FindPath(BoardManager.getGrid(), agent.agentPos, dest);
		Debug.Log("Starting to travel...");
	}

	public override void Execute (Sheriff agent) {
		agent.changeLocations(path);
		Debug.Log("Sheriff is travelling to " + dest + " from " + agent.agentPos );
		if (dest==agent.agentPos){
			if (dest==(BoardManager.locationsPos[(int)Locations.Bank])){
				agent.ChangeState(new ReturnGoldState());
			}
			if (dest==(BoardManager.locationsPos[(int)Locations.Saloon])){
				agent.ChangeState(new DrinkState());
			}
			if (dest==(BoardManager.locationsPos[(int)Locations.SheriffsOffice])){
				agent.ChangeState(new WorkState());
			}
			agent.ChangeState(new PatrolState ());
		} 
	}

	public override void Exit (Sheriff agent) {
		Debug.Log("Done Travelling");
	}
}

public class DrinkState : State<Sheriff>
{
    public DrinkState(){}

    public override void Enter(Sheriff agent)
    {
        Debug.Log("Time to relax after a hard days work");
    }

    public override void Execute(Sheriff agent)
    {
        Debug.Log("I'd like another drink of whiskey please.");
        agent.ChangeState(new SheriffTravelState (BoardManager.locationsPos[(int)Locations.SheriffsOffice]));

    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Back to the office for more work.");
    }
}

public class ReturnGoldState : State<Sheriff>
{
    public ReturnGoldState(){}

    public override void Enter(Sheriff agent)
    {
        Debug.Log("I believe this is yours");
    }

    public override void Execute(Sheriff agent)
    {
        Debug.Log("Here you go");
        agent.putMoneyInBank();
        agent.ChangeState(new PatrolState());
    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Back to the Patrol");
    }
}




