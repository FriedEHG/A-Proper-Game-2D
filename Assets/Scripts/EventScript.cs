using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

static public class EventScript
{
	static public UnityEvent PauseGame = new UnityEvent();	
	static public UnityEvent ScoreReset = new UnityEvent();
	static public UnityEvent ResumeGame = new UnityEvent();

	static public UnityEvent CommenceTheGame = new UnityEvent();
	static public UnityEvent PointScored = new UnityEvent();
	static public UnityEvent NewRound = new UnityEvent();
	//static public UnityEvent<GameObject> SelfGoal = new UnityEvent<GameObject>(); unused?

	static public UnityEvent GameWon = new UnityEvent();

	static public UnityEvent PlayUnselectColorChange = new UnityEvent();

	static public UnityEvent<BrickBehav.Team, Vector3> BrickBreak = new UnityEvent<BrickBehav.Team, Vector3>();

	static public UnityEvent<bool, Vector3> MultiballCall = new UnityEvent<bool, Vector3>();    //bool is "isDark", Vector3 is Position
	static public UnityEvent<bool, float> FullSpeedScaleCall = new UnityEvent<bool, float>();   //bool "isDark", float = Scale we multiply speed by
	//^^ FullSpeedScaleCall is called by the Player on a FullSpeedUp Power, and the event is Listened to by all of the active Balls and Powerups



}