using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
public class Sheriff : Agent {

	private StateMachine<Sheriff> stateMachine;
	public static int WORK_TIME = 5;
	public int workedTime = 0;
	bool alive;
	public void IncreaseWorkedTime (int amount) {
		this.workedTime += amount;
	}
	public bool WorkedLongEnough () {
		return this.workedTime >= WORK_TIME;
	}
	public delegate void killtheOutlaw();
	public static event killtheOutlaw onKilltheOutlaw;


	public void Awake () {
		this.agentPos=transform.position; 
		this.stateMachine = new StateMachine<Sheriff>();
		AgentManager.AddAgent(this, (int)Agents.Sheriff);
		this.agentID=(int)Agents.Sheriff;
		this.isAlive=true;
		alive=this.isAlive;
		this.stateMachine.Init(this, new WorkState(), new GlobalSheriffState());
	}

	public void ChangeState (State<Sheriff> state) {
		this.agentPos=transform.position; 
		this.agentID=(int)Agents.Sheriff;
		this.isAlive=alive;
		AgentManager.AddAgent(this, (int)Agents.Sheriff);
		this.stateMachine.ChangeState(state);
	}

	public void takeGold(Agent deadAgent){
		this.gold=deadAgent.gold;
		deadAgent.gold=0;
	}
	public void KillOutlaw(Agent deadAgent){
		deadAgent.isAlive=false;
		onKilltheOutlaw();
	}

	public int getNextPatrol(){
	int nextTarget=Random.Range(0,8);
	//avoids going to outlawcamp
	if (nextTarget==((int)Locations.OutlawCamp)){
		nextTarget=(int)Locations.SheriffsOffice;
	}
	return  nextTarget;
	}

	public override void Update () {
		this.agentPos=transform.position; 
		this.agentID=(int)Agents.Sheriff;
		this.isAlive=alive;
		AgentManager.AddAgent(this, (int)Agents.Sheriff);
		this.stateMachine.Update();
	}
	public override void CheckSurroundings () {
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Bob));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Outlaw));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Elsa));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Undertaker));
	}
	public override void HandleSenseEvent (Agent sensee) {
		if(sensee.agentID==((int)Agents.Outlaw)){
			Debug.Log("<color=red>You're dead Bandit</color>");
			KillOutlaw(sensee);
			takeGold(sensee);
		}else{
			Debug.Log("<color=yellow>Howdy Neighbour </color>");
		}
	}
}
