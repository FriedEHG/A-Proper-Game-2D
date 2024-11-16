using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerScriptableObj : ScriptableObject
{
	public type currentType;

	public abstract void Apply(GameObject target);

	public enum type
	{
		CHANGETHIS = 0,
		Sticky,
		BallSpeedScale,
		PaddleSpeedScale,
		PaddleWidthScale,
	}
}
