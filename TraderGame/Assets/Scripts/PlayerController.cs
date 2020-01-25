using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Planner.State currentState;
    public GameObject traders;
	public GameObject caravan;
    public Planner.Action[] showPlan;
    private Planner planner;
    private Vector3[] traderLocations;
    private NavMeshAgent agent;
    private List<Planner.State> plan;
    private Vector3 caravanLoation;
    private Dictionary<Planner.State, Planner.Action> actions;
    private float time;
    private bool waiting;
    private RandomizeTraders trade;
    // Start is called before the first frame update
    void Awake()
    {
        //initalizing components needed to move the player
        trade = traders.GetComponent<RandomizeTraders>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        planner = gameObject.GetComponent<Planner>();
        showPlan = new Planner.Action[5];
    }

    void Start(){
        //starting values
        caravanLoation = caravan.transform.position;
        currentState = new Planner.State();
        traderLocations = trade.getPositions();
        plan = planner.plan;
        actions = planner.actions;
        waiting = false;
        
    }
    

    // Update is called once per frame
    void Update()
    {
        //run if couroutine isn't running and if at the destenation.
        //sets new destination once prev destination is reached.
        //removes old destination from list.
        if(atDest() && !waiting){
            if(plan.Count != 0){
                currentState = plan[0];
                if(plan.Count > 1){
                    StartCoroutine(doAction(actions[plan[1]]));
                }
                plan.RemoveAt(0);
            }
        }
        createPlan();
        
    }

    //method to show the first 5 actions to be done
    public void createPlan(){
        for(int i = 0; i < plan.Count; i++){
            if(i < 5){
                showPlan[i] = actions[plan[i]];
            }
        }
    }

    //moving player agent to traderNum
    public void moveToTrader(int traderNum){
        agent.SetDestination(traderLocations[traderNum-1]);
        agent.Move(Vector3.zero);
    }

    //moving player agent to caravan
    public void moveToCaravan(){
        agent.SetDestination(caravanLoation);
        agent.Move(Vector3.zero);
    }

    //checkiing if player agent is at it's dest
    public bool atDest(){
        return agent.remainingDistance <= 0.5;
    }

    //callback method to update plan
    public void stolen(){
        int[] goal = new int[14] {2,2,2,2,2,2,2,0,0,0,0,0,0,0};
        Planner.State goalState = new Planner.State(goal);
        planner.updatePlan(currentState, goalState);
        plan = planner.plan;
        actions = planner.actions;
    }

    //coroutine method to perfrom an action
    //waits 0.5s before computing
    IEnumerator doAction(Planner.Action act){
        waiting = true;
        yield return new WaitForSeconds(0.5f);
        waiting = false;
        string actionName = act.name;
        for(int i = 1; i <= 8; i++){
            if(string.Equals(actionName, "Trader" + i)){
                moveToTrader(i);
            }
        }
        for(int i = 1; i < 28; i++){
            if(actionName.Contains(" to caravan")){
                moveToCaravan();
            }
        }
    }


}
