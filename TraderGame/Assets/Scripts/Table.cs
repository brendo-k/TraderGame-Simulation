using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
        private List<GameObject> text;
    private int[] curScore;
    public GameObject player;
    private PlayerController playrerContr;
    void Start()
    {
        //initalizing
        text = new List<GameObject>();
        foreach(Transform tra in gameObject.transform){
        	text.Add(tra.gameObject);
        }
        playrerContr = player.GetComponent<PlayerController>();
        
    }

    void Update()
    {
        //updating scoretable based on curState
        curScore = playrerContr.currentState.states;
        updateScores(curScore);

    }

    //updates the score table 
    private void updateScores(int[] scores){
    	for(int i = 0; i < text.Count; i++){
	    	Text cell = text[i].GetComponent<Text>();
	    	cell.text = scores[i].ToString();
    	}	
    }

}
