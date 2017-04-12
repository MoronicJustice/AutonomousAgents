using UnityEngine;

public class Bob : Agent {

	private StateMachine<Bob> stateMachine;

	public static int WAIT_TIME = 5;
	public int waitedTime = 0;
	public int createdTime = 0;
	bool alive;
	public void Awake () {
		this.agentPos=transform.position; 
		this.stateMachine = new StateMachine<Bob>();
		
		AgentManager.AddAgent(this, (int)Agents.Bob);
		this.agentID=(int)Agents.Bob;
		this.isAlive=true;
		alive=this.isAlive;
		Outlaw.OnBankRobbery += notTheMoney; //subscribe
		this.stateMachine.Init(this, new WaitState(this.agentPos), new GlobalBobState());
	}

	public void IncreaseWaitedTime (int amount) {
		this.waitedTime += amount;
	}
	public void goMine () {
		this.gold++;
	}
	public void notTheMoney(){
		Debug.Log("<color=blue> Oh No that outlaw has taken all the money, somone get the sheriff. </color>");
	}
	public bool WaitedLongEnough () {
		return this.waitedTime >= WAIT_TIME;
	}
	public void ChangeState (State<Bob> state) {
		this.agentPos=transform.position;
		this.isAlive=alive; 
		this.agentID=(int)Agents.Bob;
		AgentManager.AddAgent(this, (int)Agents.Bob);
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		this.agentPos=transform.position;
		this.isAlive=alive;
		this.agentID=(int)Agents.Bob;
		AgentManager.AddAgent(this, (int)Agents.Bob);
		this.stateMachine.Update();
	}
	public override void CheckSurroundings () {
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Outlaw));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Elsa));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Sheriff));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Undertaker));	
	}

	//in initialisation code
	public override void HandleSenseEvent (Agent sensee) {

	}
}
