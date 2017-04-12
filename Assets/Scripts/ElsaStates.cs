using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GlobalElsaState : State<Elsa>
{
    public GlobalElsaState(){}

    public override void Enter(Elsa agent)
    {
        
    }

    public override void Execute(Elsa agent)
    {
    	agent.CheckSurroundings(); 
    }

    public override void Exit(Elsa agent)
    {

    }

}
public sealed class CreateState : State<Elsa> {
	
	public CreateState () {}
	
	public override void Enter (Elsa agent) {
		Debug.Log("Gathering creative energies...");
	}
	
	public override void Execute (Elsa agent) {
		agent.CreateTime();
		Debug.Log("...creating more time, for a total of " + agent.createdTime + " unit" + (agent.createdTime > 1 ? "s" : "") + "...");
		agent.ChangeState(new ElsaWaitState(agent.agentPos));
	}
	
	public override void Exit (Elsa agent) {
		Debug.Log("...creativity spent!");
	}
}


public sealed class ElsaWaitState : State<Elsa> {
	Vector2 currLocation;
	public ElsaWaitState (Vector2 currLoc) {

		currLocation=currLoc;
	}

	public override void Enter (Elsa agent) {
		Debug.Log("Starting to wait...");
	}

	public override void Execute (Elsa agent) {
		
		if (currLocation==(BoardManager.locationsPos[(int)Locations.Shack])){
			agent.destPos=BoardManager.locationsPos[(int)Locations.Saloon];
		}

		if (currLocation==(BoardManager.locationsPos[(int)Locations.Saloon])){
			 agent.destPos=BoardManager.locationsPos[(int)Locations.Shack];
		}
		
		agent.IncreaseWaitedTime(1);
		Debug.Log("...waiting for " + agent.waitedTime + " cycle" + (agent.waitedTime > 1 ? "s" : "") + " so far...");
		if (agent.WaitedLongEnough()) agent.ChangeState(new ElsaTravelState(agent.destPos));
	}

	public override void Exit (Elsa agent) {
		Debug.Log("...waited long enough!");
	}
}


public sealed class ElsaTravelState : State<Elsa> {

	public static Vector2 dest;
	List<Vector2> path;

	public ElsaTravelState (Vector2 theDest) {
		dest=theDest;
	}

	public override void Enter (Elsa agent) {
		path = Astar.FindPath(BoardManager.getGrid(), agent.agentPos, dest);
		Debug.Log("Starting to travel...");
	}

	public override void Execute (Elsa agent) {
		agent.changeLocations(path);
		Debug.Log("Elsa is travelling to " + dest + " from " + agent.agentPos );
		if (dest==agent.agentPos) agent.ChangeState(new ElsaWaitState (dest));
	}

	public override void Exit (Elsa agent) {
		Debug.Log("Done Travelling");
	}
}
