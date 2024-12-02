using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerScriptableObj : ScriptableObject
{
	public type currentType;

	public abstract Tuple<PongPower, float> ApplyForTime(GameObject target);

	public abstract void ApplyInverse();

	public enum type
	{
		CHANGETHIS = 0,
		Sticky,
		BallSpeedScale,
		PaddleSpeedScale,
		PaddleWidthScale,
		Multiball,
		FullSpeedScale,
		GunRapid,
		GunSquare,
		GunShotgun
	}
}
