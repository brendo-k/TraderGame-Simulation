using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlan : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private PlayerController playerController;
    private Planner.Action[] actions;
    private List<Text> text;
    //class to show the next 5 actions
    
    void Start()
    {
        //initializing
        playerController = player.GetComponent<PlayerController>();
        actions = playerController.showPlan;
        text = new List<Text>();
        
        //getting each textbox
        foreach(Transform tra in gameObject.transform){
        	text.Add(tra.gameObject.GetComponent<Text>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //each textbox is one action from the action array of showPlan
        if(actions[0] != null){
        int counter = 0;
            foreach(Text texts in text){
                texts.text = actions[counter].name;
                counter++;
            }
        }
    }
}
