using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TESTPowerups/TESTStatUp")]

public class TESTStatUp : TESTPowerTemplate
{

	public int amount;
	public stat currentStat;

	public override void Apply(GameObject target)
	{
		switch (currentStat)
		{
			case stat.Health:
				target.GetComponent<TESTPowerPlayer>().health += amount;
				break;

			case stat.Speed:
				target.GetComponent<TESTPowerPlayer>().speed += amount;
				break;

			default:
				break;
		}
	}


	public enum stat
	{
		None = 0,
		Health,
		Speed,
	}

}
