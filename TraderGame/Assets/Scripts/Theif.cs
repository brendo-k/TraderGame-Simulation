using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Theif : MonoBehaviour
{
    public GameObject caravan;
    public GameObject player;
    private Vector3 playerLocation;
    private Vector3 caravanLocation;
    private NavMeshAgent agent;
    private bool running;
    private bool isWandering;
    private bool isSteal;
    private bool isWander;
    private bool stealInventory;
    private float probl;
    private float thefts;
    public int numberThefts;
    // Start is called before the first frame update
    void Start()
    {
        //initalizing variables
        agent = gameObject.GetComponent<NavMeshAgent>();
        running = false;
        isWandering = true;
        isSteal = false;
        isWander = false;
        caravanLocation = caravan.transform.position;
        playerLocation = player.transform.position;
        thefts = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if coroutine makeMove isn't running, run makemove
        if(!running){
            StartCoroutine("makeMove");
        }
        //if wandering is true or thefts have == 2 then theif wander 
        if(isWandering || numberThefts == 2){
            if(!isWander){
                StartCoroutine(wander());
            }
        }else{
            //sets the destination to steal if hasn't been set yet
            if(!isSteal){
                StopCoroutine("makeMove");
                isSteal = true;
                probl = Random.Range(0f,1f);
                //steal from caravan or player;
                if(probl < 0.5){
                    interceptCaravan();
                    stealInventory = false;
                }else{
                    interceptPlayer();
                    stealInventory = true;
                }
            }else{  
                //once reached update curInventory
                if(atDest()){
                    if(stealInventory)
                        steal(true);
                    else
                        steal(false);
                    running = false;
                    isWandering = true; 
                    isSteal = false;
                
                //if thief hasn't reached, continue pathing towards destination
                }else{
                    if(probl < 0.5){
                        interceptCaravan();
                    }else{
                        interceptPlayer();
                    }
                }
            }
        }
    }

    //Every .1s change direction randomly
    IEnumerator wander(){
        isWander = true;
        yield return new WaitForSeconds(0.1f);
        agent.SetDestination(gameObject.transform.position + 7*Random.insideUnitSphere);
        agent.Move(Vector3.zero);
        isWander = false;
    }

    //every 5 seconds change isWandering to true or flase 
    //if true, then thief steals
    IEnumerator makeMove(){
        running = true;
        yield return new WaitForSeconds(5f);
        float prob = Random.Range(0f,1f);
        if(prob < 1.0/3.0){
            isWandering = false;
        }else{
            isWandering = true;
        }
        running = false;
    }

    //moves thief agent to caravan
    public void interceptCaravan(){
        agent.destination = caravanLocation;
        agent.Move(Vector3.zero);
    }
    
    //moves thief agent to player
    public void interceptPlayer(){
        agent.destination = playerLocation;
    }

    //updates current inventory by randomly taking one item out of inventory or caravan 
    //takes out of inventory if isInventory == true
    public void steal(bool isInventory){
        int Offset = 0;
        if(isInventory){
            Offset = 7;
        }
        PlayerController playerController = player.GetComponent<PlayerController>();
        Planner.State curState = playerController.currentState;
        int sum = 0; 
        for(int i = Offset; i < 7+Offset; i++){
            sum += curState.states[i];
        }
        
        if(sum >= 1){
            int taken = 1;
            while(taken != 0){
                int index = (int)Random.Range(0f,7f);
                if(curState.states[index+Offset] > 0){
                    curState.states[index+Offset]--;
                    taken--;
                }
            }
        }
        thefts++;
        playerController.stolen();
    }


    //cheks to see if the thief agent is at it's destination    
    public bool atDest(){
        return agent.remainingDistance <= 0.5;
    }
}
