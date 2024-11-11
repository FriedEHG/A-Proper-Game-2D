using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(CharacterController))] //Automatically adds CharaccterController component to any object we attach this script to
[RequireComponent(typeof(PlayerInput))]
public class TESTPlayer : MonoBehaviour
{   //THIS SCRIPT CCAN BE FOUND AT (https://www.youtube.com/watch?v=g_s0y5yFxYg)
	// OR AT https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

	private float playerSpeed = 2.0f;
	[SerializeField]
	private float jumpHeight = 1.0f;
	[SerializeField] private float gravityValue = -9.81f;



	private CharacterController controller;
	private Vector3 playerVelocity;
	private bool groundedPlayer;

	private Vector2 movementInput = Vector2.zero;
	private bool jumped = false;


	private void Start()
	{
		controller = gameObject.GetComponent<CharacterController>();
		//changed AddComponent to GetComponent since the controller is Auto Added up top
	}


	public void OnMove(InputAction.CallbackContext context)
	{
		movementInput = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		jumped = context.action.triggered;
	}


	void Update()
	{
		groundedPlayer = controller.isGrounded;
		if (groundedPlayer && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
		}

		Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
		controller.Move(move * Time.deltaTime * playerSpeed);

		if (move != Vector3.zero)
		{	//rotates the player to the direction that they are Moving
			gameObject.transform.forward = move;
		}

		// Changes the height position of the player..
		if (jumped && groundedPlayer)
		{
			playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
		}

		playerVelocity.y += gravityValue * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);
	}
}