using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PongPower")]

public class PongPower : PowerScriptableObj
{
	public float amount;

	public override void Apply(GameObject target)
	{
		switch (currentType)
		{
			case type.Sticky:
				target.GetComponentInParent<Player>().isSticky = true;
				break;

			case type.BallSpeedScale:
				target.GetComponentInParent<Player>().moveSpd *= amount;
				break;

			case type.PaddleSpeedScale:

				break;

			case type.PaddleWidthScale:
				target.GetComponentInParent<Player>().ChangePaddleWidth(amount);
				break;


			default:
				Debug.LogError("Unchanged Power Type. Change power type within this powers Asset");
				break;
		}
	}
}
