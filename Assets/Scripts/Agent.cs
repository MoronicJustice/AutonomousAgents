using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random=UnityEngine.Random;
abstract public class Agent : MonoBehaviour {
	 public Vector2 agentPos; 
	 public Vector2 destPos;
	 public bool isAlive;
	 public int gold; 
	 public int agentID;
	 
	abstract public void Update ();
	abstract public void HandleSenseEvent (Agent Sensee);
	abstract public void CheckSurroundings ();

	public void changeLocations(List<Vector2> path){
		CheckSurroundings();
		if(path.Count != 0){
			transform.position = path[0]; 	
			path.RemoveAt(0);
		}

	}
	//functions to change sprrites colour on death
	public void agentIsDead(){
		Color deathColor= Color.grey;
		if (!isAlive){
			var spriteRenderer = GetComponent<SpriteRenderer> ();
			spriteRenderer.color += deathColor;
		}

	}
	//functions to change sprrites colour on being alive again
	public void agentIsAlive(){
		Color deathColor= Color.grey;
		if (isAlive){
			var spriteRenderer = GetComponent<SpriteRenderer> ();
			spriteRenderer.color -= deathColor;
		}
		
	}
	//puts whatever money the agent has and puts it in the bank
	//used by bob to store earnings and sheriff to return stolen property
	public void putMoneyInBank () {
		Debug.Log("<color=red>My Gold was </color>" + gold);
		Bank.goldInBank += gold;
		Debug.Log("<color=red>Money In Bank is </color>" + Bank.goldInBank);
		gold=0;
		Debug.Log("<color=red>My Gold is </color>" + gold);
	}

}
    