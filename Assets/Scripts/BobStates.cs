using UnityEngine;
using System.Collections.Generic;

public class GlobalBobState : State<Bob>
{
    public GlobalBobState(){}

    public override void Enter(Bob agent)
    {
        
    }

    public override void Execute(Bob agent)
    {
    	agent.CheckSurroundings(); 
    }

    public override void Exit(Bob agent)
    {

    }

}
public sealed class WaitState : State<Bob> {
	Vector2 currLocation;
	public WaitState (Vector2 currLoc) {

		currLocation=currLoc;
	}

	public override void Enter (Bob agent) {
		Debug.Log("Starting to wait...");
	}

	public override void Execute (Bob agent) {
		if (currLocation==(BoardManager.locationsPos[(int)Locations.Shack])){
			agent.destPos=BoardManager.locationsPos[(int)Locations.Mine];
			//Debug.Log("<color=blue>////The Destination is </color>" + agent.destPos);
		}
		if (currLocation==(BoardManager.locationsPos[(int)Locations.Mine])){
			agent.destPos=BoardManager.locationsPos[(int)Locations.Bank];
			agent.goMine();
			//Debug.Log("<color=blue>////The Destination is </color>" + agent.destPos);
		}
		if (currLocation==(BoardManager.locationsPos[(int)Locations.Bank])){
			 agent.destPos=BoardManager.locationsPos[(int)Locations.Saloon];
			 agent.putMoneyInBank();
			 //Debug.Log("<color=blue>////The Destination is </color>" + agent.destPos);
		}
		if (currLocation==(BoardManager.locationsPos[(int)Locations.Saloon])){
			 agent.destPos=BoardManager.locationsPos[(int)Locations.Shack];
			 //Debug.Log("<color=blue>////The Destination is </color>" + agent.destPos);
		}
		
		agent.IncreaseWaitedTime(1);
		Debug.Log("...waiting for " + agent.waitedTime + " cycle" + (agent.waitedTime > 1 ? "s" : "") + " so far...");
		if (agent.WaitedLongEnough()) agent.ChangeState(new BobTravelState(agent.destPos));
	}

	public override void Exit (Bob agent) {
		Debug.Log("...waited long enough!");
	}
}


public sealed class BobTravelState : State<Bob> {

	public static Vector2 dest;
	List<Vector2> path;

	public BobTravelState (Vector2 theDest) {
		dest=theDest;
	}

	public override void Enter (Bob agent) {
		path = Astar.FindPath(BoardManager.getGrid(), agent.agentPos, dest);
		Debug.Log("Starting to travel...");
	}

	public override void Execute (Bob agent) {
		agent.changeLocations(path);
		Debug.Log("Bob is travelling to " + dest + " from " + agent.agentPos );
		if (dest==agent.agentPos) agent.ChangeState(new WaitState (dest));
	}

	public override void Exit (Bob agent) {
		Debug.Log("Done Travelling");
	}
}
