using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
	public float powerChance = 0.1f;	//stated in the inspector, may be different

	List<PowerBase> powersAllInScene;
	
	List<PowerBase> powersInPlayDark;
	List<PowerBase> powersInPlayLight;

	List<PowerBase> powersCharDark;
	List<PowerBase> powersCharLight;


	void Start()
	{
		EventScript.BrickBreakLight.AddListener(PowerCheckLight);
		EventScript.BrickBreakDark.AddListener(PowerCheckDark);
		//Debug.Log(characterTest);
		InitializeVariables();
		InitializePowersInPlay();
	}

	private void InitializeVariables()
	{
		//list of all powers
		powersAllInScene = new List<PowerBase>();

		foreach (PowerBase pow in FindObjectsByType<PowerBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
		{
			powersAllInScene.Add(pow);
			Debug.Log($"Power Added: {pow.name}");
		}

		//Get the character power lists

		powersCharDark = new List<PowerBase>();
		powersCharLight = new List<PowerBase>();	//NEEDS TO BE UPDATED
	}

	private void InitializePowersInPlay()
	{
		//Dark
		foreach (PowerBase powAll in powersAllInScene)
		{
			foreach(PowerBase powChar in powersCharDark)
			{
				if (powAll.powerTemplate.currentType == powChar.powerTemplate.currentType 
					&& powAll.currentTeam == PowerBase.team.Dark)
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
				if (powAll.powerTemplate.currentType == powChar.powerTemplate.currentType
					&& powAll.currentTeam == PowerBase.team.Light)
				{
					powersInPlayLight.Add(powChar);
				}
			}
		}
	}

	public void PowerCheckLight()		//SENDS OUT A DEBUG WHEN A POWER IS SUPPOSED TO SPAWN
	{
		float randChance = Random.Range(0f, 1f);
		int randPow = Mathf.RoundToInt(Random.Range(0.51f, 3.49f));  //Subtract 0.49 from min and add 0.49 to max in order to have more even random distribution

		if (randChance < powerChance)
		{
			//PowerSummonLight(randPow);
			Debug.Log($"LightPower {randPow} Summoned");
		}
	}

	public void PowerCheckDark()        //SENDS OUT A DEBUG WHEN A POWER IS SUPPOSED TO SPAWN
	{
		float randChance = Random.Range(0f, 1f);
		int randPow = Mathf.RoundToInt(Random.Range(0.51f, 3.49f));  //Subtract 0.49 from min and add 0.49 to max in order to have more even random distribution

		if (randChance < powerChance)
		{
			//PowerSummonDark(randPow);
			Debug.Log($"DarkPower {randPow} Summoned");
		}
	}
}