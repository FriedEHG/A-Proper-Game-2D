using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
	public float powerChance = 0.0f;    //stated in the inspector, may be different

	public List<PowerBase> powersAllInScene;

	public List<PowerBase> powersInPlayDark;
	public List<PowerBase> powersInPlayLight;

	public List<PowerBase> powersCharDark;
	public List<PowerBase> powersCharLight;

	public List<float> powerTimeoutsDark;
	public List<float> powerTimeoutsLight;
	public float timerConstant;

	void Start()
	{
		EventScript.BrickBreak.AddListener(PowerCheck);
		//Debug.Log(characterTest);
		InitializeVariables();
		InitializePowersInPlay();
	}

	private void Update()
	{
		PowerTimeoutCheck();
	}

	private void InitializeVariables()
	{
		//list of all powers
		powersAllInScene = new List<PowerBase>();
		powerTimeoutsDark = new List<float> { 0, 0, 0 };
		powerTimeoutsLight = new List<float> { 0, 0, 0 };

		foreach (PowerBase pow in FindObjectsByType<PowerBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
		{
			powersAllInScene.Add(pow);
			//Debug.Log($"Power Added: {pow.name}");
		}

		//Get the character power lists
		//powersCharDark = new List<PowerBase>();
		//powersCharLight = new List<PowerBase>();    //NEEDS TO BE UPDATED WHEN WE WANT TO ADD CHARACTER FUNCTIONALITY

	}

	private void InitializePowersInPlay()
	{
		//Dark
		foreach (PowerBase powAll in powersAllInScene)  //look through all of the Power objects in the scene and grab the ones that 
		{                                               //match our Team AND are in the list of CharPowers that we have
			foreach (PowerBase powChar in powersCharDark)   //THIS WILL NEED TO CHANGE WHEN ADDING CHARACTER SELECT FUNCTIONALITY
			{
				if (powAll.pongPowerScriptableObj.currentType == powChar.pongPowerScriptableObj.currentType
					&& powAll.currentTeam == PowerBase.team.Dark
					&& !powersInPlayDark.Contains(powChar))
				{
					powersInPlayDark.Add(powChar);
				}
			}


		}

		//Light
		foreach (PowerBase powAll in powersAllInScene)
		{
			foreach (PowerBase powChar in powersCharLight)
			{
				if (powAll.pongPowerScriptableObj.currentType == powChar.pongPowerScriptableObj.currentType
					&& powAll.currentTeam == PowerBase.team.Light
					&& !powersInPlayLight.Contains(powChar))
				{
					powersInPlayLight.Add(powChar);
				}
			}
		}
	}

	public void PowerCheck(BrickBehav.Team brickTeam, Vector3 blockPos)      //ACTIVATES WHEN A POWER IS SUPPOSED TO SPAWN
	{
		//Debug.Log($"Power Check. BrickTeam: {brickTeam}, pos: {pos}");

		float randChance = Random.Range(0f, 1f);
		int randPow = Mathf.RoundToInt(Random.Range(0.51f, 3.49f));  //Subtract 0.49 from min and add 0.49 to max in order to have more even random distribution

		Vector3 powPos = new Vector3(blockPos.x, blockPos.y, Universals.powerupHeight);

		//Debug.Log($"RandomPower:{randChance}");
		if (randChance < powerChance)
		{
			switch (brickTeam)
			{
				case BrickBehav.Team.Dark:  //if a Dark brick broke, it means playerLight broke it. So we need to summon a Light power
					PowerSummonLight(randPow, powPos);

					break;
				case BrickBehav.Team.Light:
					PowerSummonDark(randPow, powPos);

					break;
				default:
					Debug.Log($"WAWAWEEWA");

					break;
			}
		}
	}

	private void PowerTimeoutCheck()
	{
		timerConstant += Time.deltaTime;

		for (int i = 0; i < powerTimeoutsDark.Count - 1; i++)
		{
			if (powerTimeoutsDark[i] != 0)
			{
				if (timerConstant >= powerTimeoutsDark[i])
				{
					powersInPlayDark[i].pongPowerScriptableObj.ApplyInverse();
				}
			}
		}

		for (int i = 0; i < powerTimeoutsLight.Count - 1; i++)
		{
			if (powerTimeoutsLight[i] != 0)
			{
				if (timerConstant >= powerTimeoutsLight[i])
				{
					powersInPlayLight[i].pongPowerScriptableObj.ApplyInverse();
				}
			}
		}
	}

	void PowerSummonDark(int powNum, Vector3 pos)
	{
		Debug.Log($"DarkPower {powNum} Summoned");
		PowerBase power = powersInPlayDark[powNum - 1];
		//Debug.Log($"PowerInPlayDark:{powersInPlayDark.Count}, powNum:{powNum}");

		if (power.gameObject.GetComponent<PowerObject>().isCurrent == false)
		{
			Debug.Log($"LightPower {powNum} is False, now making True");
			power.gameObject.GetComponent<PowerObject>().Appear(pos);
			//power.gameObject.SetActive(true);
			//power.gameObject.transform.SetPositionAndRotation(pos, Quaternion.identity);
		}
	}

	void PowerSummonLight(int powNum, Vector3 pos)
	{
		Debug.Log($"LightPower {powNum} Summoned");
		PowerBase power = powersInPlayLight[powNum - 1];

		if (power.gameObject.GetComponent<PowerObject>().isCurrent == false)
		{
			Debug.Log($"LightPower {powNum} is False, now making True");
			power.gameObject.GetComponent<PowerObject>().Appear(pos);
			//power.gameObject.SetActive(true);
			//power.gameObject.transform.SetPositionAndRotation(pos, Quaternion.identity);
		}
	}

	public void PowerCountdownBegin(PongPower powerIncoming, float seconds)
	{
		for (int i = 0; i < powersInPlayDark.Count - 1; i++)
		{
			if (powersInPlayDark[i].GameObject() == powerIncoming.GameObject())
			{
				powerTimeoutsDark[i] = Time.time + seconds;
				return;
			}
		}

		for (int i = 0; i < powersInPlayLight.Count - 1; i++)
		{
			if (powersInPlayLight[i].GameObject() == powerIncoming.GameObject())
			{
				powerTimeoutsLight[i] = Time.time + seconds;
				return;
			}
		}
	}

}