using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float pauseTime;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {   
        
        //+ is pressed when shift and = are pressed together. Shift has to be held before pressing =
        if(Input.GetKeyDown(KeyCode.Equals) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))){
        	//increases speed by a factor of 2 
            Time.timeScale*=2;
        }
        //decreases speed by a factor of 2 when -
        if(Input.GetKeyDown(KeyCode.Minus)){
        	Time.timeScale/=2;
        }
        //pauses the game when spacebar is pressed 
        if(Input.GetKeyDown(KeyCode.Space)){
        	if(pauseTime == 0){
        		pauseTime = Time.timeScale;
        		Time.timeScale = 0;
        	}else{
        		Time.timeScale = this.pauseTime;
        		pauseTime = 0;
        	}
        }

    }
}
