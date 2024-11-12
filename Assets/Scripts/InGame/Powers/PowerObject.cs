using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerObject : MonoBehaviour
{
	[SerializeField] float speedStart = 0.02f;
	float speed;
	[SerializeField] bool isDark = true;
	Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		InitializeVariables();
		InitializeEventListeners();

		transform.position = new Vector3(0, 0, -Universals.powerupHeight);
	}

	private void InitializeVariables()
	{
		rb = GetComponent<Rigidbody>();
	}
	private void InitializeEventListeners()
	{
		EventScript.BeginGame.AddListener(BeginMovement);
		EventScript.NewRound.AddListener(BeginMovement);
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

	private void RandomPowerSelection()
	{
		int random = Mathf.RoundToInt(Random.Range(0.51f, 3.49f));

		if (isDark)
		{
			//switch type of power that this is too DarkPlayer.PlayerPower[random]
		}
		else
		{
			//switch type of power that this is too LightPlayer.PlayerPower[random]
		}
	}
}
