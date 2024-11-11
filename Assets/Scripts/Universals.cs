using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Universals
{
	public static float darkBallHeight = 10f;
	public static float lightBallHeight = -10f;
	//ball vs brick values ,must be opposite so a ball my glide above blocks of same color and collide against opposites
	//paddles don't get a height, they just have a tall collider
	public static float darkBrickHeight = -11f;
	public static float lightBrickHeight = 9f;

	public static float powerupHeight = 20f;	//placed on top of everything else

	//During character seletion, the charSelectBttns will assign their Serialized Character to one of these two Variables
	public static Character lightCharacter = null;
	public static Character darkCharacter = null;

	//public static Character[] characters = { null, null, null, null, null, null };

	public static int lightCharacterTest = 999;
	public static int darkCharacterTest = 888;

	//public static int[] charactersTest = { null, null, null, null, null, null };
	public static int requiredScore = 2;

	public static string LightTeam = "Light";	//These may be used to syncronize all "Team" mentions if we feel like it later
	public static string DarkTeam = "Dark";	
}