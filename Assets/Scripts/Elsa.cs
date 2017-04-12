using UnityEngine;

public class Elsa : Agent {

	private StateMachine<Elsa> stateMachine;
	public int waitedTime = 0;
	public int createdTime = 0;
	public static int WAIT_TIME = 5;
	bool alive;
	public void Awake () {
		this.agentPos=transform.position; 
		this.stateMachine = new StateMachine<Elsa>();
		
		AgentManager.AddAgent(this, (int)Agents.Elsa);	
		this.isAlive=true;
		alive=this.isAlive;
		this.agentID=(int)Agents.Elsa;
		this.stateMachine.Init(this, new ElsaWaitState(transform.position), new GlobalElsaState());
	}
	public void CreateTime () {
		this.createdTime++;
		this.waitedTime = 0;
	}
	public void IncreaseWaitedTime (int amount) {
		this.waitedTime += amount;
	}
	public bool WaitedLongEnough () {
		return this.waitedTime >= WAIT_TIME;
	}
	public void ChangeState (State<Elsa> state) {
		this.agentPos=transform.position; 
		this.agentID=(int)Agents.Elsa;
		this.isAlive=alive;
		AgentManager.AddAgent(this, (int)Agents.Elsa);
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		this.agentPos=transform.position; 
		this.agentID=(int)Agents.Elsa;
		this.isAlive=alive;
		AgentManager.AddAgent(this, (int)Agents.Elsa);
		this.stateMachine.Update();
	}
	public override void CheckSurroundings () {
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Bob));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Outlaw));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Undertaker));
		SenseEvent.Sense(this, AgentManager.GetAgent((int)Agents.Sheriff));
	}
	public override void HandleSenseEvent (Agent sensee) {

	}
}
