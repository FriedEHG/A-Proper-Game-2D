using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PowerObject : MonoBehaviour
{                   //This script is in charge of the Physical object that holds the Powerup Script.
	[SerializeField] float speedStart = 0.02f;
	float speed;
	bool isDark;

	public bool isCurrent;

	PowerBase ourPower;
	Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		InitializeVariables();
		InitializeEventListeners();

		transform.position = new Vector3(0, 0, Universals.powerupHeight);
		ToasterBath();
	}

	private void InitializeVariables()
	{
		rb = GetComponent<Rigidbody>();
		ourPower = GetComponent<PowerBase>();
		isDark = (ourPower.currentTeam == PowerBase.team.Dark);

		isCurrent = false;
	}

	private void InitializeEventListeners()
	{
		EventScript.CommenceTheGame.AddListener(ToasterBath);
		EventScript.PointScored.AddListener(ToasterBath);
		EventScript.GameWon.AddListener(ToasterBath);
		EventScript.NewRound.AddListener(ToasterBath);

		//EventScript.FullSpeedScaleCall.AddListener(ChangeSpeed);
	}


	// Update is called once per frame
	void FixedUpdate()
	{
		//rb.MovePosition(new Vector3(0, (isDark ? speed : -speed), 0));

		if (isDark)
		{
			rb.transform.Translate(new Vector3(0, speed * Time.timeScale, 0));
		}
		else
		{
			rb.transform.Translate(new Vector3(0, -speed * Time.timeScale, 0));
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<GoalBehav>() != null)
		{
			ToasterBath();
		}
	}


	private void ChangeSpeed(bool isDarkIncoming, float scale)
	{
		if (isDarkIncoming == isDark)
		{
			speed += (scale > 0) ? speedStart * (scale - 1) : -speedStart * (scale + 1);
			//ex.if our scale is 1.2, we want to only ADD 20% to the speed. 
			//and if scale we get is Negative, then we Add a negative number and move the scale in the opposite way
		}
	}

	public void Appear(Vector3 spawnPoint)
	{
		transform.position = spawnPoint;
		isCurrent = true;
		BeginMovement();
	}

	private void BeginMovement()
	{
		speed = speedStart;
	}

	private void HaltMovement()
	{
		speed = 0f;
	}

	public void ToasterBath()
	{
		HaltMovement();
		isCurrent = false;
		transform.position = new Vector3(1000, 1000, 1000);
	}



}
