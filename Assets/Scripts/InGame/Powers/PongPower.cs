using System;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "PongPower")]

public class PongPower : PowerScriptableObj
{
	public float amount;
	public float powerDurationSeconds = 5;

	DateTime targetTime;

	Player player;

	public override Tuple<PongPower, float> ApplyForTime(GameObject target)
	{
		player = target.GetComponentInParent<Player>();


		if (player != false)
		{
			/////////////this could maybe be a thing that players are able to change in a setting, later in development
			player.UnstickBalls();
			/////////////

			switch (currentType)
			{
				case type.Sticky:
					player.isSticky = true;
					break;

				case type.BallSpeedScale:

					break;

				case type.PaddleSpeedScale:
					player.moveSpeedCurrent *= amount;
					break;

				case type.PaddleWidthScale:
					player.ChangePaddleWidth(amount);
					break;

				case type.Multiball:
					player.Multiball(amount);
					break;

				case type.FullSpeedScale:

					player.FullSpeedScale(amount);
					break;

				//case type.GunRapid:
				//	target.GetComponentInParent<Player>().GunRapid();
				//	break;

				//case type.GunSquare:
				//	target.GetComponentInParent<Player>().GunSquare();
				//	break;

				//case type.GunShotgun:
				//	target.GetComponentInParent<Player>().GunShotgun();
				//	break;

				default:
					Debug.LogError("Unchanged Power Type. Change power type within this powers Asset");
					break;
			}
		}

		var returnItem = new Tuple<PongPower, float>(this, powerDurationSeconds);

		return returnItem;
	}

	public override void ApplyInverse()
	{
		Debug.Log("Apply Inverse: " + name);

		if (player != false)
		{
			/////////////
			//player.UnstickBalls();
			/////////////

			switch (currentType)
			{
				case type.Sticky:
					player.isSticky = false;
					break;

				case type.BallSpeedScale:
					//Something, if we actually use this power
					break;

				case type.PaddleSpeedScale:
					player.moveSpeedCurrent *= (1 / amount);
					break;

				case type.PaddleWidthScale:
					player.ChangePaddleWidth(1 / amount);
					break;

				case type.Multiball:
					//NOT SCIENTIFICALLY POSSIBLE
					break;

				case type.FullSpeedScale:

					player.FullSpeedScale(1 / amount);
					break;

				//case type.GunRapid:
				//	target.GetComponentInParent<Player>().GunRapid();
				//	break;

				//case type.GunSquare:
				//	target.GetComponentInParent<Player>().GunSquare();
				//	break;

				//case type.GunShotgun:
				//	target.GetComponentInParent<Player>().GunShotgun();
				//	break;

				default:
					Debug.LogError("Apply Power Inverse Error, Huh?? How did we proc an error here but not at the start???");
					break;
			}
		}
	}

	//public void TimeoutFullSpeedUp()
	//{
	//	Debug.Log("Function Called");
	//	//yield return new WaitForSeconds(powerDurationSeconds);
	//	Debug.Log("Function Enacted");
	//	player.FullSpeedScale(1 / amount);
	//}
}
