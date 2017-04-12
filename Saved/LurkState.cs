using UnityEngine;

public sealed class LurkState : State<Outlaw> {

	static readonly LurkState instance = new LurkState();

	public static LurkState Instance {
		get {
			return instance;
		}
	}

	static LurkState () {}
	private LurkState () {}

	public override void Enter (Outlaw agent) {
		Debug.Log("Starting to Lurk...");
	}

	public override void Execute (Outlaw agent) {
		agent.IncreaseLurkedTime(1);
		Debug.Log("...Lurking for " + agent.lurkedTime + " cycle" + (agent.lurkedTime > 1 ? "s" : "") + " so far...");
		if (agent.LurkedLongEnough()) agent.ChangeState(HeistState.Instance);
	}

	public override void Exit (Outlaw agent) {
		Debug.Log("... Lurked long enough!!!");
	}
}
