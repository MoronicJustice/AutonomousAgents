using UnityEngine;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

public class Outlaw : Agent {

	private StateMachine<Outlaw> stateMachine;
	public static int LURK_TIME = 7;
	public int lurkedTime = 0;
	public int successfulHeists;
	public int currLocation; //=(int)Locations.OutlawCamp;
	bool alive;
	
	public delegate void bankRobbery();
	public static event bankRobbery OnBankRobbery;

	public void Awake () {
		this.stateMachine = new StateMachine<Outlaw>();
		this.agentPos=transform.position; 
		AgentManager.AddAgent(this, (int)Agents.Outlaw);

		this.agentID=(int)Agents.Outlaw;
		this.isAlive=true;
		alive=this.isAlive;
		Sheriff.onKilltheOutlaw += getDead;
		this.stateMachine.Init(this, new LurkState(this.agentPos), new GlobalOutlawState());
	}

	public void IncreaseLurkedTime (int amount) {
		this.lurkedTime += amount;
	}
	
	public void ResetLurkedTime () {
		this.lurkedTime = 0;
	}
	
	public bool LurkedLongEnough () {
		return this.lurkedTime >= LURK_TIME;
	}

	public bool DecidetoRobBank () {
		int decision=Random.Range(0,8);

		if (decision>=5){
			return true;
		}
		return  false;
	}
	// 
	//  fores him to repsawn only after the undertaker has arrived at his body
	public void getDead(){
		alive=false;
		this.isAlive=false;
		agentIsDead();
		ChangeState(new DeathState());
	}
	public bool Respawn () {
		Agent undertaker=AgentManager.GetAgent((int)Agents.Undertaker);
		Renderer rend; rend = GetComponent<Renderer>();
		/*
		if (undertaker.transform.position == transform.position){
			//Vector3 pos= new Vector3(-200, -200, 0);
			rend.enabled=false;
			agentIsDead();
			return false;
		}
		//if the undertaker is at the cemetary respawn
		if(undertaker.agentPos == BoardManager.locationsPos[(int)Locations.Cemetary]){
			rend.enabled=true;
			agentIsAlive();
			return true;
		}
		//transform.position=undertaker.transform.position;
		return  false;
		*/
		return true;
	}

	public void RobBank(){
		this.gold= Bank.goldInBank;
		Bank.goldInBank=0;
		successfulHeists++;
		Debug.Log("<color=red>Stolen Gold is </color>" + this.gold);
		Debug.Log("<color=red>Number of times I'ves stolen is now</color>" + successfulHeists);
		OnBankRobbery();
	}

	public void ChangeState (State<Outlaw> state) {
		this.agentPos=transform.position;
		this.agentID=(int)Agents.Outlaw;
		this.isAlive=alive;
		alive=AgentManager.GetAgent((int)Agents.Outlaw).isAlive;
		AgentManager.AddAgent(this, (int)Agents.Outlaw);
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		this.agentPos=transform.position;
		this.agentID=(int)Agents.Outlaw;
		alive=AgentManager.GetAgent((int)Agents.Outlaw).isAlive;
		this.isAlive=alive;
		AgentManager.AddAgent(this, (int)Agents.Outlaw);
		this.stateMachine.Update();
	}
	public override void CheckSurroundings () {
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Bob));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Undertaker));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Elsa));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Sheriff));
	}
	public override void HandleSenseEvent (Agent sensee) {

	}
}
