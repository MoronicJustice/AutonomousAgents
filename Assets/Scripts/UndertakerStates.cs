using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GlobalUndertakerState : State<Undertaker>
{
    public GlobalUndertakerState(){}

    public override void Enter(Undertaker agent)
    {
        
    }

    public override void Execute(Undertaker agent)
    {
       agent.CheckSurroundings(); 
    }

    public override void Exit(Undertaker agent)
    {

    }

}
public class HoverState : State<Undertaker>
{
    public HoverState(){}

    public override void Enter(Undertaker agent)
    {
        Debug.Log("Waiting to begin the Harvest");
    }

    public override void Execute(Undertaker agent)
    {   Agent outlaw= AgentManager.GetAgent((int)Agents.Outlaw); 
        if(!outlaw.isAlive){
            agent.ChangeState(new UndertakerTravelState(outlaw.agentPos));
        }
    }

    public override void Exit(Undertaker agent)
    {
        Debug.Log("Time to harvest");
    }
}
public class UndertakerTravelState : State<Undertaker>
{
    public static Vector2 dest;
    List<Vector2> path;

    public UndertakerTravelState (Vector2 theDest) {
        dest=theDest;
    }

    public override void Enter (Undertaker agent) {
        path = Astar.FindPath(BoardManager.getGrid(), agent.agentPos, dest);
        Debug.Log("Starting to travel...");
    }

    public override void Execute (Undertaker agent) {
        agent.changeLocations(path);
        Debug.Log("Undertaker is travelling to " + dest + " from " + agent.agentPos );
        if (dest==agent.agentPos){
            if (dest==(BoardManager.locationsPos[(int)Locations.Cemetary])){
                agent.ChangeState(new UndertakerTravelState(BoardManager.locationsPos[(int)Locations.UndertakersOffice]));
            }
            if (dest==(BoardManager.locationsPos[(int)Locations.UndertakersOffice])){
                agent.ChangeState(new HoverState());
            }
            agent.ChangeState(new SearchState());
        } 
    }

    public override void Exit (Undertaker agent) {
        Debug.Log("Done Travelling");
    }
}

public class SearchState : State<Undertaker>
{   
    public SearchState()
    {
    }

    public override void Enter(Undertaker agent)
    {
        Debug.Log("Searching for corpse at " + agent.transform.position);
    }

    public override void Execute(Undertaker agent)
    {

        Debug.Log("Victim found");
        //Agent outlaw= AgentManager.GetAgent((int)Agents.Outlaw);
        //outlaw.goToCemetary();
        agent.ChangeState(new UndertakerTravelState(BoardManager.locationsPos[(int)Locations.Cemetary]));
    }

    public override void Exit(Undertaker agent)
    {
        Debug.Log("Time to go to the Cemetary");
    }
}
