using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GoalBehav;

public class BallBehav : MonoBehaviour
{

	//ALL Ball spawning and despawning is handled in the Overseer



	[SerializeField] Overseer overseer;
	[SerializeField] Vector2 startingDirection;
	public Vector2 movementDirection;
	[SerializeField] public Vector3 startingPos;
	public float speedStart = 5f; // Start speed of the ball
	private float speed = 0f; // Speed of the ball

	private Rigidbody rb; // Reference to the Rigidbody2D component
	public Team currentTeam;
	private SphereCollider sphereCollider;

	public Material darkMaterial;
	public Material lightMaterial;

	public bool isTrue = false;		//Disable this bool in editor on all balls Except one

	void Start()
	{
		EventScript.BeginGame.AddListener(GameBegin);
		EventScript.PointScored.AddListener(Halt);
		EventScript.NewRound.AddListener(GameReset);
		EventScript.GameWon.AddListener(Halt);

		InitializeVariables();

	}

	void Update()
	{
		rb.velocity = movementDirection * speed;
	}

	private void InitializeVariables()
	{
		sphereCollider = GetComponent<SphereCollider>();
		rb = GetComponent<Rigidbody>();

		if (currentTeam == Team.Light)
		{   //set each ball to its team height
			startingPos = new Vector3(transform.position.x, transform.position.y, Universals.lightBallHeight);
			gameObject.GetComponent<Renderer>().material = lightMaterial;
		}
		else
		{
			startingPos = new Vector3(transform.position.x, transform.position.y, Universals.darkBallHeight);
			gameObject.GetComponent<Renderer>().material = darkMaterial;
		}

		speed = 0f;
		movementDirection = startingDirection.normalized;   //potentially we could randomize this after each spawn
	}

	public void GameBegin()
	{
		transform.SetPositionAndRotation(startingPos, Quaternion.identity);
		speed = speedStart;
		movementDirection = startingDirection.normalized;
		rb.velocity = movementDirection * speed;
	}

	void GameReset()
	{
		transform.position = startingPos;
		movementDirection = Vector3.zero;
	}

	void Halt()
	{
		speed = 0f;
		movementDirection = Vector3.zero;
	}

	void OnCollisionEnter(Collision collision)
	{
		//Debug.Log($"Before: {rb.velocity}");
		GameObject otherObj = collision.gameObject;
		// Bounce off the walls
		if (otherObj.CompareTag("Paddle"))
		{
			// Reflect the ball's direction based on the normal of the collision
			Vector2 reflectPoint = otherObj.GetComponent<Player>().reflectPoint;
			ReflectPaddle(collision, reflectPoint);
		}
		else if (otherObj.tag == "Brick")
		{
			if (otherObj.GetComponent<BrickBehav>().currentTeam == BrickBehav.Team.Dark && currentTeam == Team.Light ||
				otherObj.GetComponent<BrickBehav>().currentTeam == BrickBehav.Team.Light && currentTeam == Team.Dark)
			{	//IF the brick is the opposite team as the ball
				BrickBehav brickBehav = otherObj.GetComponent<BrickBehav>();
				StandardReflect(collision);
				brickBehav.ChangeTeam();    //send the brick to the other plane and change it's colors
			}
		}
		else if (otherObj.tag == "Wall")
		{
			GoalBehav collidedGoal = collision.gameObject.GetComponent<GoalBehav>();
			if (collidedGoal != null)
			{	//If we collided with a Goal wall...
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
			{	//else if it is just a Regular wall...
				StandardReflect(collision);
			}
		}
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

	//public void ReflectLR() //called by the LRTrigger
	//{
	//	Debug.Log("ReflectLR");
	//	rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
	//}

	//public void ReflectUD() //called by the UDTrigger
	//{
	//	Debug.Log("ReflectUD");
	//	rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
	//}


	public enum Team
	{
		None = 0,
		Dark = 1,
		Light = 2,
	}
}
