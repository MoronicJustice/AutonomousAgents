public class StateMachine <T> {
	
	private T agent;
	//the agent's  current state
	private State<T> state;
	//a record of the last state the agent was in
	private State<T> prevState;//addition
	//this state logic is called every time the FSM is updated
	private State<T> globalState;//addition

	public void Awake () {
		this.state = null;
		this.prevState = null;//addition
		this.globalState = null;//addition
	}

	public void Init (T agent, State<T> startState) {
		this.agent = agent;
		this.state = startState;
	}

	public void Update () {
		//if a global state exists, call its execute method
		if (this.globalState != null) this.globalState.Execute(this.agent);//addition
		
		if (this.state != null) this.state.Execute(this.agent);
	}
	
	public void ChangeState (State<T> nextState) {
		if (this.state != null) this.state.Exit(this.agent);
		this.prevState = this.state; //addition
		this.state = nextState;
		if (this.state != null) this.state.Enter(this.agent);
	}
}