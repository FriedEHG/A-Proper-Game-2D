using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static GoalBehav;

public class BallBehav : MonoBehaviour
{
	//ALL Ball spawning and despawning is handled in the Overseer

	[SerializeField] Overseer overseer;
	[SerializeField] Vector2 startingDirection;
	public Vector2 movementDirection;
	[SerializeField] public Vector3 startingPos;
	public float speedStart = 5f; // Start speed of the ball
	public float speed = 0f; // Speed of the ball

	private Rigidbody rb; // Reference to the Rigidbody2D component
	public Team currentTeam;
	private SphereCollider sphereCollider;

	public Material darkMaterial;
	public Material lightMaterial;

	public bool isOGBall;       //Disable this bool in editor on all balls Except one of each color

	private Player ourPlayer;
	private GameObject ourSpawnPoint;


	void Start()
	{
		InitializeEventListeners();

		InitializeVariables();
		//Debug.Log($"Ball start: {name}");
	}


	void Update()
	{
		rb.velocity = movementDirection * speed;
	}


	private void InitializeEventListeners()
	{
		EventScript.CommenceTheGame.AddListener(GameBegin);
		EventScript.PointScored.AddListener(Halt);
		EventScript.NewRound.AddListener(GameReset);
		EventScript.GameWon.AddListener(Halt);
		EventScript.FullSpeedScaleCall.AddListener(ChangeSpeed);
	}

	private void ChangeSpeed(bool isDarkIncoming, float scale)
	{
		if (isDarkIncoming && currentTeam == Team.Dark || !isDarkIncoming && currentTeam == Team.Light)
		{
			speed += (scale > 0) ? speedStart * (scale - 1) : -speedStart * (scale + 1);
			//ex.if our scale is 1.2, we want to only ADD 20% to the speed. 
			//and if scale we get is Negative, then we Add a negative number and move the scale in the opposite way
		}
	}

	private void InitializeVariables()
	{
		sphereCollider = GetComponent<SphereCollider>();
		rb = GetComponent<Rigidbody>();

		//speed = 0f;
		movementDirection = startingDirection.normalized;   //potentially we could randomize this after each spawn

		foreach (Player player in FindObjectsByType<Player>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
		{
			if (currentTeam == Team.Light && player.currentTeam == Player.Team.Light
				|| currentTeam == Team.Dark && player.currentTeam == Player.Team.Dark)
			{
				ourPlayer = player;
				ourSpawnPoint = AcquireSpawnpoint();

				//Debug.Log($"Ball {name} Initialized. ourPlayer:{player}. ourTeam:{currentTeam.ToString()}");
			}
		}

		SetStartPos();
	}

	private GameObject AcquireSpawnpoint()
	{
		Transform[] childrenTransforms = ourPlayer.GetComponentsInChildren<Transform>();
		GameObject spawnPoint = this.gameObject;	//this will be changed after like 4 lines, so it's fine if it self references

		foreach (var childTransform in childrenTransforms)
		{
			if (childTransform != ourPlayer.transform && childTransform.CompareTag("BallSpawn"))
			{
				spawnPoint = childTransform.gameObject;
			}
		}

		return spawnPoint;
	}

	private void SetStartPos()
	{
		if (currentTeam == Team.Light)
		{   //set each ball to its team height
			startingPos = new Vector3(ourSpawnPoint.transform.position.x, ourSpawnPoint.transform.position.y, Universals.lightBallHeight);
			gameObject.GetComponent<Renderer>().material = lightMaterial;
		}
		else
		{
			startingPos = new Vector3(ourSpawnPoint.transform.position.x, ourSpawnPoint.transform.position.y, Universals.darkBallHeight);
			gameObject.GetComponent<Renderer>().material = darkMaterial;
		}
	}

	public void GameBegin()		//Happens after the countdown
	{
		transform.SetPositionAndRotation(startingPos, Quaternion.identity);
		movementDirection = startingDirection.normalized;
		if (!isOGBall)
		{
			gameObject.SetActive(false);
		}

		//speed = speedStart;

		StickyStick(ourPlayer);
	}

	public void Respawn()
	{
		SetStartPos();
		transform.SetPositionAndRotation(startingPos, Quaternion.identity);
		//Debug.Log(ourPlayer.name);
		movementDirection = startingDirection.normalized;
		StickyStick(ourPlayer);
	}

	
	public void MultiballSpawning()
	{
		Start();
		//Debug.Log($"Multiball SpeedStart:{speedStart}, Current Speed: {speed}");
		speed = speedStart;

		transform.SetPositionAndRotation(startingPos, Quaternion.identity);
		movementDirection = startingDirection.normalized;
	}

	void GameReset()	//Happens after every goal
	{
		SetStartPos();
		transform.position = startingPos;
		movementDirection = Vector3.zero;
	}

	void Halt()
	{
		speed = 0f;
		//Debug.Log("HALT");
	}

	void OnCollisionEnter(Collision collision)
	{
		//Debug.Log($"Before: {rb.velocity}");
		GameObject otherObj = collision.gameObject;
		// Bounce off the walls
		if (otherObj.CompareTag("Paddle"))
		{
			Player player = otherObj.GetComponent<Player>();

			// Reflect the ball's direction based on the normal of the collision
			Vector2 reflectPoint = player.reflectPoint;
			ReflectPaddle(collision, reflectPoint);

			if (player.isSticky)
			{   //Set speed to zero After we change the direction
				StickyStick(player);
			}
		}
		else if (otherObj.tag == "Brick")
		{
			if (otherObj.GetComponent<BrickBehav>().currentTeam == BrickBehav.Team.Dark && currentTeam == Team.Light ||
				otherObj.GetComponent<BrickBehav>().currentTeam == BrickBehav.Team.Light && currentTeam == Team.Dark)
			{   //IF the brick is the opposite team as the ball, should Always be the case due to how we arranged the z-pos of objects
				BrickBehav brickBehav = otherObj.GetComponent<BrickBehav>();
				StandardReflect(collision);
				brickBehav.ChangeTeam();    //send the brick to the other plane and change it's colors
			}
		}
		else if (otherObj.tag == "Wall")
		{
			GoalBehav collidedGoal = collision.gameObject.GetComponent<GoalBehav>();
			if (collidedGoal != null)
			{   //If we collided with a Goal wall...
				if (collidedGoal.currentGoalType == GoalType.Light && currentTeam == Team.Dark)
				{
					overseer.ScoreFor("Dark");
					rb.velocity = Vector3.zero;     //Set velocity to 0 cause the we are starting a new bout
				}
				else if (collidedGoal.currentGoalType == GoalType.Dark && currentTeam == Team.Light)
				{
					overseer.ScoreFor("Light");
					rb.velocity = Vector3.zero;
				}
				else    //Occurs when the ball hits their own goal
				{
					overseer.SelfGoalDissapear(GetComponent<BallBehav>(), currentTeam);
				}
			}
			else
			{   //else if it is just a Regular wall...
				StandardReflect(collision);
			}
		}
	}

	public void StickyStick(Player player)
	{
		Halt();
		player.stuckBalls.Add(this);
		transform.parent = player.GetComponentInChildren<MovingContainer>().gameObject.transform;
	}

	private void ReflectPaddle(Collision collision, Vector2 reflectPoint)
	{
		Vector2 poc = collision.contacts[0].point;
		movementDirection = (poc - reflectPoint).normalized;
		rb.velocity = movementDirection * speed;
		//Debug.Log($"{poc} - {reflectPoint} = {poc - reflectPoint}");
	}

	private void StandardReflect(Collision collision)
	{
		Vector2 normal = collision.contacts[0].normal;
		movementDirection = Vector2.Reflect(movementDirection, normal);
		rb.velocity = movementDirection * speed;
	}


	public enum Team
	{
		None = 0,
		Dark = 1,
		Light = 2,
	}
}
