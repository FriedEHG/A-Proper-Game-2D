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

	static public UnityEvent<bool, Vector3> MultiballCall = new UnityEvent<bool, Vector3>();
}