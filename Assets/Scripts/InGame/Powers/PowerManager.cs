using System.Collections;
using System.Collections.Generic;
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


	void Start()
	{
		EventScript.BrickBreak.AddListener(PowerCheck);
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
			//Debug.Log($"Power Added: {pow.name}");
		}

		//Get the character power lists

		//powersCharDark = new List<PowerBase>();
		//powersCharLight = new List<PowerBase>();    //NEEDS TO BE UPDATED
	}

	private void InitializePowersInPlay()
	{
		//Dark
		foreach (PowerBase powAll in powersAllInScene)  //look through all of the Power objects in the scene and rab the ones that 
		{                                               //match our Team AND are in the list of CharPowers that we have
			foreach (PowerBase powChar in powersCharDark)
			{
				if (powAll.powerTemplate.currentType == powChar.powerTemplate.currentType
					&& powAll.currentTeam == PowerBase.team.Dark
					&& !powersInPlayDark.Contains(powChar))
				{
					powersInPlayDark.Add(powChar);
				}
			}

			powAll.gameObject.SetActive(false);
		}

		//Light
		foreach (PowerBase powAll in powersAllInScene)
		{
			foreach (PowerBase powChar in powersCharLight)
			{
				if (powAll.powerTemplate.currentType == powChar.powerTemplate.currentType
					&& powAll.currentTeam == PowerBase.team.Light
					&& !powersInPlayLight.Contains(powChar))
				{
					powersInPlayLight.Add(powChar);
				}
			}
		}
	}

	public void PowerCheck(BrickBehav.Team brickTeam, Vector3 blockPos)      //SENDS OUT A DEBUG WHEN A POWER IS SUPPOSED TO SPAWN
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
				case BrickBehav.Team.Dark:	//if a Dark brick broke, it means playerLight broke it. So we need to summon a Light power
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

	void PowerSummonDark(int powNum, Vector3 pos)
	{
		//Debug.Log($"DarkPower {powNum-1} Summoned");
		//Debug.Log($"PowerInPlayDark:{powersInPlayDark.Count}, powNum:{powNum}");	
		PowerBase power = powersInPlayDark[powNum-1];


		if (!power.gameObject.activeSelf)
		{
			power.gameObject.SetActive(true);
			power.gameObject.transform.SetPositionAndRotation(pos, Quaternion.identity);
		}
	}

	public void PowerSummonLight(int powNum, Vector3 pos)
	{
		Debug.Log($"LightPower {powNum} Summoned");
		PowerBase power = powersInPlayLight[powNum-1];

		if (!power.gameObject.activeSelf)
		{
			power.gameObject.SetActive(true);
			power.gameObject.transform.SetPositionAndRotation(pos, Quaternion.identity);
		}
	}


}