using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Undertaker : Agent {

	private StateMachine<Undertaker> stateMachine;
	bool alive;

	public void Awake () {
		this.agentPos=transform.position; 
		this.stateMachine = new StateMachine<Undertaker>();		
		this.agentID=(int)Agents.Undertaker;
		this.isAlive=true;
		alive=this.isAlive;
		AgentManager.AddAgent(this, this.agentID);
		Sheriff.onKilltheOutlaw += getTheBody;
		this.stateMachine.Init(this, new HoverState(), new GlobalUndertakerState());
	}

	public void ChangeState (State<Undertaker> state) {
		this.agentPos=transform.position; 
		this.agentID=(int)Agents.Undertaker;
		this.isAlive=alive;
		AgentManager.AddAgent(this, (int)Agents.Undertaker);
		this.stateMachine.ChangeState(state);
	}
	public void getTheBody(){
		Agent deadAgent = AgentManager.GetAgent((int)Agents.Outlaw);
		Debug.Log("I had best go get the body");
		this.stateMachine.ChangeState(new UndertakerTravelState(deadAgent.transform.position));
	}
	public bool CheckPulse (Agent agent) {
		return agent.isAlive;
	}

	public override void Update () {
		this.agentPos=transform.position; 
		this.agentID=(int)Agents.Undertaker;
		this.isAlive=alive;
		AgentManager.AddAgent(this, (int)Agents.Undertaker);
		this.stateMachine.Update();
	}

	public override void CheckSurroundings () {
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Bob));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Outlaw));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Elsa));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Sheriff));
	}

	public override void HandleSenseEvent(Agent deadAgent){

		if(deadAgent.agentID == (int)Agents.Outlaw){

		}
	}
}
