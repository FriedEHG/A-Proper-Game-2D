using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehav : MonoBehaviour
{
    public GoalType currentGoalType;
	public enum GoalType
    {
        Light,
        Dark,
    }
}
