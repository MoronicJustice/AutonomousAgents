using UnityEngine;

public sealed class HeistState : State<Outlaw> {
	
	static readonly HeistState instance = new HeistState();
	
	public static HeistState Instance {
		get {
			return instance;
		}
	}
	
	static HeistState () {}
	private HeistState () {}
	
	public override void Enter (Outlaw agent) {
		Debug.Log("Getting ready to Steal");
	}
	
	public override void Execute (Outlaw agent) {
		agent.RobBank();
		Debug.Log("Stole " + agent.gold + " of gold to date" + "...");
		agent.ChangeState(LurkState.Instance);
	}
	
	public override void Exit (Outlaw agent) {
		Debug.Log("I'm out of here");
	}
}
