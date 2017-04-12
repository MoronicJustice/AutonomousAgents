using UnityEngine;

public class Outlaw : Agent {

	private StateMachine<Outlaw> stateMachine;
	public static int LURK_TIME = 7;
	public int lurkedTime = 0;
	public int gold=0;
	public int successfulHeists=0;
	public int currLocation =(int)Locations.OutlawCamp;

	public delegate void bankRobbery();
	public static event bankRobbery OnBankRobbery;

	public void Awake () {
		this.stateMachine = new StateMachine<Outlaw>();
		this.stateMachine.Init(this, LurkState.Instance);
	}

	public void IncreaseLurkedTime (int amount) {
		this.lurkedTime += amount;
	}
	
	public bool LurkedLongEnough () {
		return this.lurkedTime >= LURK_TIME;
	}

	public void ChangeLocations(){
		if (currLocation==((int)Locations.OutlawCamp)){
			currLocation=(int)Locations.Cemeterary;
		}else{
		currLocation=(int)Locations.OutlawCamp;
		}
	}

	public void RobBank(){
		gold++;
		successfulHeists++;
		if(OnBankRobbery!=null){
			OnBankRobbery();
		}
	}

	public void ChangeState (State<Outlaw> state) {
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		this.stateMachine.Update();
	}
}
