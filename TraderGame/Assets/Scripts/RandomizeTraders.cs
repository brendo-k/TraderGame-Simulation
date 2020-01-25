using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeTraders : MonoBehaviour
{
	private Vector3[] permute;

    //randomizes the positions of the traderss
	void Start()
    {
    	permute = new Vector3[8];
	   	int counter = 0;
	   	foreach (Transform child in gameObject.transform)
		{
			permute[counter] = (child.position);
			counter++;
		}	
		ranomizeArray();
		for(int i = 0; i < permute.Length; i++){
			gameObject.transform.GetChild(i).position = permute[i];
		}
    }

	//get method for permute 
	public Vector3[] getPositions(){
		return permute;
	}

	//randomizes an array
    private void ranomizeArray(){
    	int n = permute.Length;
    	while (n > 1){
    		n--;
    		int k = (int)Random.Range(0, n+1);
    		Vector3 value = permute[k];
    		permute[k] = permute[n];
    		permute[n] = value;
    	}
    }
}
