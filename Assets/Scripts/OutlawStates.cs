using UnityEngine;
using System.Collections.Generic;

public class GlobalOutlawState : State<Outlaw>
{
    public  GlobalOutlawState(){}

    public override void Enter(Outlaw agent)
    {
        
    }

    public override void Execute(Outlaw agent)
    {
    	agent.CheckSurroundings(); 
    }

    public override void Exit(Outlaw agent)
    {

    }

}

public sealed class LurkState : State<Outlaw> {
	Vector2 currLocation;
	public LurkState (Vector2 currLoc) {
		currLocation=currLoc;
	}

	public override void Enter (Outlaw agent) {
		Debug.Log("Starting to Lurk...");
	}

	public override void Execute (Outlaw agent) {
		agent.IncreaseLurkedTime(1);

		if (currLocation==(BoardManager.locationsPos[(int)Locations.OutlawCamp])){
			agent.destPos=BoardManager.locationsPos[(int)Locations.Cemetary];
		}
		if (currLocation==(BoardManager.locationsPos[(int)Locations.Cemetary])){
			agent.destPos=BoardManager.locationsPos[(int)Locations.OutlawCamp];
		}
		if(agent.DecidetoRobBank()){
			agent.destPos=BoardManager.locationsPos[(int)Locations.Bank];
			agent.ChangeState(new OutlawTravelState(agent.destPos));
		}
		if (agent.LurkedLongEnough() ){
			agent.ResetLurkedTime();
			agent.ChangeState(new OutlawTravelState(agent.destPos));
		}
	}

	public override void Exit (Outlaw agent) {
		Debug.Log("... Lurked long enough!!!");
	}
}

public sealed class HeistState : State<Outlaw> {
	
	public HeistState () {}
	
	public override void Enter (Outlaw agent) {
		Debug.Log("Getting ready to Steal");
	}
	
	public override void Execute (Outlaw agent) {
		agent.RobBank();
		Debug.Log("Stole " + agent.gold + " of gold to date" + "...");
		agent.ChangeState(new OutlawTravelState (BoardManager.locationsPos[(int)Locations.OutlawCamp]));
	}
	
	public override void Exit (Outlaw agent) {
		Debug.Log("I'm out of here");
	}
}
public sealed class DeathState : State<Outlaw> {
	
	public DeathState () {}
	
	public override void Enter (Outlaw agent) {
		Debug.Log("I am dead");
		agent.agentIsDead();
	}
	
	public override void Execute (Outlaw agent) {
		if(agent.Respawn()){
		agent.transform.position=BoardManager.locationsPos[(int)Locations.OutlawCamp];
		agent.ChangeState(new LurkState(BoardManager.locationsPos[(int)Locations.OutlawCamp]));
		}
	}
	
	public override void Exit (Outlaw agent) {
		Debug.Log("I'm Back Baby");
	}
}

public sealed class OutlawTravelState : State<Outlaw> {
	public static Vector2 dest;
	List<Vector2> path;

	public OutlawTravelState (Vector2 theDest) {
		dest=theDest;
	}

	public override void Enter (Outlaw agent) {
		path = Astar.FindPath(BoardManager.getGrid(), agent.agentPos, dest);
		Debug.Log("Starting to travel...");
	}

	public override void Execute (Outlaw agent) {
		agent.changeLocations(path);
		Debug.Log("Outlaw is travelling to " + dest + " from " + agent.agentPos );
		if(!agent.isAlive){
			agent.ChangeState(new DeathState());
		}
		if (dest==agent.agentPos){
			if (dest==(BoardManager.locationsPos[(int)Locations.Bank])){
				agent.ChangeState(new HeistState());
			}
			if (dest==(BoardManager.locationsPos[(int)Locations.Cemetary])){
				agent.ChangeState(new LurkState (dest));
			}
			if (dest==(BoardManager.locationsPos[(int)Locations.OutlawCamp])){
				agent.ChangeState(new LurkState (dest));
			}
		} 
	}

	public override void Exit (Outlaw agent) {
		Debug.Log("Done Travelling");
	}
}