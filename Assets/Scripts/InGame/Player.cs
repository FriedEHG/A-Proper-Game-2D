using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	[SerializeField] InputActionAsset PlayerInputAsset;
	/*Right now we have 2 input action assets. If we want to consolidate those, 
	 * we could consider making 4 action maps, a player 1 Menu, player1Game, player2Menu, player2Game
	 * Or not...
	*/
	InputActionMap MenuControlsMap;
	InputActionMap GameControlsMap;
	InputAction MenuMove;
	InputAction MenuSelect;
	InputAction GameMove;
	InputAction GameAction;
	InputAction EscapeAction;
	InputAction PauseAction;

	
	public Vector2 reflectPoint;

	Mode currentMode = Mode.None;   //We start on None so that the players won't be able to move before the countdown ends

	//Powerable Stats
	public float moveSpd;
	public float paddleScale = 1;
	public bool isSticky = true; 
	//

	float moveSpdTime;
	public float moveLimitX = 3.15f;
	public float moveLimitZ = 6.75f;
	Vector2 moveVector;

	[SerializeField] GameObject? paddle;
	[SerializeField] GameObject? reflectPointObj;
	BoxCollider? boxCollider;
	public bool isNoclip;

	bool isPaused = false;
	bool isPauseHeld = false;

	Vector2 startPos;

	/// <summary>
	/// ///////////////////////////////// 
	/// </summary>
	Character character;
	int characterTest;


	private void OnEnable()
	{
		PlayerInputAsset.Enable();
	}

	private void OnDisable()
	{
		PlayerInputAsset.Disable();
	}

	void Start()
	{
		EventScript.BeginGame.AddListener(GameBegin);
		EventScript.NewRound.AddListener(GameReset);
		EventScript.GameWon.AddListener(GameReset);
		VariableInitialize();
		ControlsInitialize();
		//Debug.Log(characterTest);
	}

	void VariableInitialize()
	{
		boxCollider = paddle.GetComponent<BoxCollider>();
		characterTest = Universals.lightCharacterTest;
		startPos = transform.position;
	}

	private void ControlsInitialize()
	{
		//Might need to use these for when the game is Paused in order to switch between Game controls and Menu controls,
		//not sure how much of this is handled automatically by Unity
		MenuControlsMap = PlayerInputAsset.FindActionMap("MenuControls");
		GameControlsMap = PlayerInputAsset.FindActionMap("GameControls");

		MenuMove = PlayerInputAsset.FindAction("MenuMove");
		MenuSelect = PlayerInputAsset.FindAction("MenuSelect");
		GameMove = PlayerInputAsset.FindAction("Movement");
		GameAction = PlayerInputAsset.FindAction("Action");
		PauseAction = PlayerInputAsset.FindAction("Pause");
	}

	void Update()
	{   // Update is called once per frame
		if (currentMode == Mode.Game)
		{
			GameUpdate();
		}
		else if (currentMode == Mode.Menu)
		{
			MenuUpdate();
		}
	}

	void MenuUpdate()
	{
		//Currently unused, deletable since all Menu stuff uses built-in Unity UI functionality
	}

	void GameBegin()
	{
		currentMode = Mode.Game;
	}

	void GameReset()
	{
		currentMode = Mode.None;
		transform.position = startPos;
	}

	void GameUpdate()
	{
		//if pressing GameMove buttons, move
		moveSpdTime = moveSpd * Time.deltaTime;
		moveVector = new Vector2(GameMove.ReadValue<Vector2>().x * moveSpdTime, (isNoclip ? GameMove.ReadValue<Vector2>().y : 0) * moveSpd);
		reflectPoint = reflectPointObj.transform.position;

		if (moveVector.x != 0 || moveVector.y != 0)
		{
			GameMovement();
		}
		if (PauseAction.ReadValue<float>() > 0.1)
		{
			if (isPauseHeld == false)
			{
				PauseCheck();
				isPauseHeld = true;
			}
		}
		else { isPauseHeld = false; }
	}

	private void GameMovement()
	{
		if ((paddle.transform.position.x - moveSpdTime > -moveLimitX && moveVector.x < 0) ||    //not touching left wall && tryna move left
			(paddle.transform.position.x + moveSpdTime < moveLimitX && moveVector.x > 0))       //not touching right wall && tryna move right
		{   //allow the movement
			paddle.transform.Translate(moveVector.x, 0, 0);
		}

		if (isNoclip &&
			((paddle.transform.position.y - moveSpdTime > -moveLimitZ && moveVector.y < 0) ||    //not touching bottom wall && tryna move down
			(paddle.transform.position.y + moveSpdTime < moveLimitZ && moveVector.y > 0)))       //not touching top wall && tryna move up
		{
			paddle.transform.Translate(0, 0, moveVector.y);
		}
	}

	private void PauseCheck()
	{
		Debug.Log($"PauseCheck Player:{gameObject.name}");
		EventScript.PauseGame.Invoke();
	}

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log("ye");
	}

	private void OnCollisionEnter(Collision collision)
	{
		//Debug.Log(collision.GetContact(0).point);
	}

	public void ChangeWidth(float newScaleX)
	{
		Vector3 currentScale = paddle.transform.lossyScale;
		paddle.transform.localScale = new Vector3(currentScale.x*newScaleX, currentScale.y, currentScale.z);
	}

	enum Mode
	{
		None,
		Menu,
		Game
	}
}