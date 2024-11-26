using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PowerObject : MonoBehaviour
{ 					//is just in charge of the physical object that holds the power.
	[SerializeField] float speedStart = 0.02f;
	float speed;
	bool isDark;
	PowerBase ourPower;
	Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		InitializeVariables();
		InitializeEventListeners();

		transform.position = new Vector3(0, 0, Universals.powerupHeight);
	}

	private void InitializeVariables()
	{
		rb = GetComponent<Rigidbody>();
		ourPower = GetComponent<PowerBase>();
		isDark = (ourPower.currentTeam == PowerBase.team.Dark);
	}

	private void InitializeEventListeners()
	{
		EventScript.CommenceTheGame.AddListener(ToasterBath);
		EventScript.PointScored.AddListener(ToasterBath);
		EventScript.GameWon.AddListener(ToasterBath);
		EventScript.NewRound.AddListener(ToasterBath);
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

	public void Appear(Vector3 spawnPoint)
	{
		transform.position = spawnPoint;
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

	private void ToasterBath()
	{
		this.gameObject.SetActive(false);
	}
}
