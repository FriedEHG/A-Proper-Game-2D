using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Powers/StatUp")]

public class PowerStatChange : PowerScriptableObj
{
	public float amount;
	public stat currentStat;

	public override void Apply(GameObject target)
	{
		switch (currentStat)
		{
			case stat.Sticky:
				target.GetComponentInParent<Player>().isSticky = true;
				break;

			case stat.SpeedScale:
				target.GetComponentInParent<Player>().moveSpd *= amount;
				break;

			case stat.WidthScale:
				target.GetComponentInParent<Player>().ChangePaddleWidth(amount);
				break;


			default:
				break;
		}
	}


	public enum stat
	{
		None = 0,
		Sticky,
		SpeedScale,
		WidthScale,

	}
}
