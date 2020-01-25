using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Priority_Queue;

public class Planner : MonoBehaviour
{
    // Start is called before the first frame update
	
	public List<State> plan;
	public Dictionary<State, Action> actions;
	private List<Action> actionsNodes;
    
    void Awake()
    {
        actionsNodes = new List<Action>();
        makeActions();
        int[] goal = new int[14] {2,2,2,2,2,2,2,0,0,0,0,0,0,0};
        State goalState = new State(goal);
		Dictionary<State, State> prev = new Dictionary<State, State>(new StateComparerSet());
		Dictionary<State, Action> actionList = new Dictionary<State, Action>(new StateComparerSet());
		actions = new Dictionary<State, Action>(new StateComparerSet());
		State startState = new State();
		findGoal(goalState, startState, prev, actionList);
		plan = buildPath(prev);
		buildActions(plan, actionList);
    }

   	public void buildActions(List<State> plan, Dictionary<State, Action> actionList){
		foreach(State st in plan){
			actions.Add(st, actionList[st]);
		}
	}
    
	public void updatePlan(State newStart, State goal){
		Dictionary<State, State> prev = new Dictionary<State, State>(new StateComparerSet());
		Dictionary<State, Action> actionList = new Dictionary<State, Action>(new StateComparerSet());
		findGoal(goal, newStart, prev, actionList);
		plan.Clear();
		actions.Clear();
		plan = buildPath(prev);
		buildActions(plan, actionList);
	}
	//making all the actions in the game
    public void makeActions(){
    	//moving items to the caravan or taking items out
    	string[] spices = new string[7]{"Tu", "Sa", "Ca", "Ci", "Cl", "Pe", "Su"};
    	for(int i = 0; i < spices.Length; i++){
    		for(int j = 1; j < 5; j++){
    			State preCondition = State.intitalize();
    			preCondition.setStates(spices[i], j, true);
    			actionsNodes.Add(new Action(preCondition, j + " " + spices[i] + " to caravan"));
    			preCondition = State.intitalize();
    			preCondition.setStates(spices[i], j, false);
    			actionsNodes.Add(new Action(preCondition, j + " " + spices[i] + " to inventory"));
    		}
    	}
		
		//different trader actions
		State pre = State.intitalize();
		State post = new State();
		post.setStates("Tu", 2, true);
		actionsNodes.Add(new Action(pre, post, "Trader1"));
		
		pre = State.intitalize();
		post = new State();
		takeFromInv(pre, post, "Tu", 2);
		post.setStates("Sa", 1, true);
		actionsNodes.Add(new Action(pre, post, "Trader2"));
		
		pre = State.intitalize();
		post = new State();
		takeFromInv(pre, post, "Sa", 2);
		post.setStates("Ca", 1, true);
		actionsNodes.Add(new Action(pre, post, "Trader3"));

		pre = State.intitalize();
		post = new State();
		takeFromInv(pre, post, "Tu", 4);
		post.setStates("Ci", 1, true);
		actionsNodes.Add(new Action(pre, post, "Trader4"));
		
		pre = State.intitalize();
		post = new State();
		takeFromInv(pre, post, "Ca", 1);
		takeFromInv(pre, post, "Tu", 1);
		post.setStates("Cl", 1, true);
		actionsNodes.Add(new Action(pre, post, "Trader5"));

		pre = State.intitalize();
		post = new State();
		takeFromInv(pre, post, "Tu", 2);
		takeFromInv(pre, post, "Sa", 1);
		takeFromInv(pre, post, "Ci", 1);
		post.setStates("Pe", 1, true);
		actionsNodes.Add(new Action(pre, post, "Trader6"));

		pre = State.intitalize();
		post = new State();
		takeFromInv(pre, post, "Ca", 4);
		post.setStates("Su", 1, true);
		actionsNodes.Add(new Action(pre, post, "Trader7"));
		
		pre = State.intitalize();
		post = new State();
		takeFromInv(pre, post, "Sa", 1);
		takeFromInv(pre, post, "Ci", 1);
		takeFromInv(pre, post, "Cl", 1);
		post.setStates("Su", 1, true);
		actionsNodes.Add(new Action(pre, post, "Trader8"));
    }

	//sets pre and post conditions for giving spices to traders
	public void takeFromInv(State pre, State post, string spice, int amount){
		pre.setStates(spice, amount, true);
		post.setStates(spice, -amount, true);
	}

	//heuristic for A*. Based number of changes needed in the caravan to get to the goal state
    public int heuristic(State goal, State newState){
    	int hScore = 0;
		for(int i = 0; i < 7; i++){
			hScore += 20*(int)Mathf.Pow((newState.states[i]-2),2);
		}
		return hScore;
    }

	public List<State> buildPath(Dictionary<State,State> prev){
		State cur = new State(new int[14]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1});
		cur = prev[cur];
		State start = new State();
		List<State> path = new List<State>();
		while(cur != null){
			path.Insert(0, cur);
			cur = prev[cur];
		}
		return path;
	}


    public void findGoal(State goal, State startState, Dictionary<State, State> prev, Dictionary<State, Action> actionList){
		

    	SimplePriorityQueue<State, int> queue = new SimplePriorityQueue<State, int>(new StateComparerSet());
		Dictionary<State, int> dis = new Dictionary<State, int>(new StateComparerSet());
		HashSet<State> visted = new HashSet<State>(new StateComparerSet());

		dis[startState] = 0;
		queue.Enqueue(startState, 0);
		prev[startState] = null;
		actionList[startState] = new Action(null, null, "start");
		visted.Add(startState);
		int counter = 0;
		while(queue.Count() != 0){
			counter++;
			State cur = queue.Dequeue();
			visted.Add(cur);

			List<Action> possibleActions = findPossibleActions(cur);

			if(goal.finished(cur)){
				State end = new State(new int[14]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1});
				prev[end] = cur;
				return;
			}

			foreach(Action act in possibleActions){
				State newState = act.setCursState(cur);
				if(visted.Contains(newState)){
					continue;
				}
				if (dis.ContainsKey(newState)){
					if(dis[newState] > dis[cur] + 1){
						dis[newState] = dis[cur] + 1;
					}
				}else{
					dis[newState] = dis[cur] + 1;
				}
				int fScore = dis[newState] + heuristic(goal, newState);
				if(queue.Contains(newState)){
					if(fScore < queue.GetPriority(newState)){
						queue.UpdatePriority(newState, fScore);
					}else{
						continue;
					}
				}else{
					queue.Enqueue(newState, fScore);
				}
				prev[newState] = cur;
				actionList[newState] = act;
			}
		}
		return;
    }
	public List<Action> findPossibleActions(State cur){
		List<Action> possibleAct = new List<Action>();
		foreach(Action act in actionsNodes){
			if(act.canDoAction(cur)){
				possibleAct.Add(act);
			}
		}
		return possibleAct;
	}
    public class Action{
    	private State preCondition;
    	private State postCondition;
    	public string name;

    	public Action(State preCondition, string name){
    		this.preCondition = preCondition;
    		this.postCondition = preCondition.invert();
    		this.name = name;
    	}
		public Action(State preCondition, State postCodition, string name){
    		this.preCondition = preCondition;
    		this.postCondition = postCodition;
    		this.name = name;
    	}

    	public bool canDoAction(State curState){
    		return(curState.isGreaterThanEqual(preCondition) && curState.inventorySize() + postCondition.inventorySize() <=4);
    	}

    	public State setCursState(State curState){
			return State.add(postCondition, curState);
    	}

    	public string toString(){
    		return preCondition.toString() + " // " + postCondition.toString() + " ++ " + name;
    	}
    }
    public class State{
    	public int[] states;

    	public State(){
    		states = new int[14];
    	}

    	public State(int[] state){
    		this.states = state;
    	}

    	public static State intitalize(){
    		int[] states = new int[14];
    		for(int i = 0; i < states.Length; i++){
    			states[i] = -1;
    		}
    		return new State(states);
    	}
    	
    	public bool isGreaterThanEqual(State newState){
    		for(int i = 0; i < newState.Length(); i++){
    			if(newState.states[i] > this.states[i]){
    				return false;
    			}
    		}
    		return true;
    	}

    	public void setStates(string spice, int newInt, bool isInventory){
    		int offset = 0;
    		if(isInventory){
    			offset = 7;
    		}
    		switch(spice){
    			case "Tu":
    				states[offset + 0] = newInt;
    				break;
    			case "Sa":
    				states[offset + 1] = newInt;
    				break;
    			case "Ca":
    				states[offset + 2] = newInt;
    				break;
    			case "Ci":
    				states[offset + 3] = newInt;
    				break;
    			case "Cl":
    				states[offset + 4] = newInt;
    				break;
    			case "Pe":
    				states[offset + 5] = newInt;
    				break;
    			case "Su":
    				states[offset + 6] = newInt;
    				break;
    		}
    	}

    	public int Length(){
    		return this.states.Length;
    	}

    	public State invert(){
    		int[] newState = new int[states.Length];

    		for(int i = 0; i < states.Length; i++){
    			if(states[i] == -1){
    				continue;
    			}else{
    				newState[(i + 7)%14] = states[i];    				
    				newState[i] = states[i] * -1;
    			}
    		}
    		return new State(newState);
    	}

    	public static State add(State firstState, State secondState ){
    		State newState = new State();
			for(int i = 0; i < newState.Length(); i++){
    			newState.states[i] = firstState.states[i] + secondState.states[i];
    		}
			return newState;
    	}

    	public string toString(){
    		return string.Join(",", states);
    	}

    	public int inventorySize(){
    		int size = 0;
    		for(int i = 7; i < states.Length; i++){
    			size += states[i];
    		}
    		return size;
    	}

    	public int caravanSize(){
    		int size = 0;
    		for(int i = 0; i < 7; i++){
    			size += states[i];
    		}
    		return size;
    	}

		public bool finished(State cur){
			for(int i = 0; i < 7; i++){
				if(states[i] > cur.states[i]){
					return false;
				}
			}
			return true;
		}

		public bool equals(State compare){
			for(int i = 0; i < compare.Length(); i++){
				if(compare.states[i] != states[i]){
					return false;
				}
			}
			return true;
		}
		
	}
	class StateComparerSet : IEqualityComparer<State>{
			public bool Equals(State x, State y)
			{
				if (x.equals(y)){
					return true;
				}
				return false;
			}

			public int GetHashCode(State obj){
				int hc=obj.Length();
				for(int i=0;i<obj.Length();++i)
				{
					hc=unchecked(hc*314159 + obj.states[i]);
				}
				return hc;
			}
	} 
}