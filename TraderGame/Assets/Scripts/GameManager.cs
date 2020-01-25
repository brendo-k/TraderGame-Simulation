using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float pauseTime;
    // Start is called before the first frame update
    void Start()
    {
    	
    }

    // Update is called once per frame
    void Update()
    {   
        
        
        if(Input.GetKeyDown(KeyCode.Equals) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))){
        	Time.timeScale*=2;
        }
        if(Input.GetKeyDown(KeyCode.Minus)){
        	Time.timeScale/=2;
        }
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
