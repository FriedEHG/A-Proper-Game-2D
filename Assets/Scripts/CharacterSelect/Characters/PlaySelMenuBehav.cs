using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySelMenuBehav : MonoBehaviour
{
	Character char0;
	Character char1;
	Character char2;
	Character char3;
	Character char4;
	Character char5;

	bool lightCharSelected; //starts false, becomes true when light player selects a character
	bool lightPlaySelected; //starts false, becomes true when light player selects the Play bttn
	bool lightOptionsOpen;  //starts false, becomes true when light player opens Options menu. Becomes false once they leave.
	bool darkOptionsOpen;
	bool darkCharSelected;
	bool darkPlaySelected;

	[SerializeField] PlayBttnBehav playBttnBehav;
	public TextMeshProUGUI lightPlayerCharDisplay;
	public TextMeshProUGUI darkPlayerCharDisplay;

	void Start()
	{
		VariableInitialize();
		EventScript.PlayUnselectColorChange.AddListener(PlayUnselect);
	}

	void VariableInitialize()
	{
		lightCharSelected = false;
		lightPlaySelected = false;
		darkCharSelected = false;
		darkPlaySelected = false;

		lightPlayerCharDisplay.text = "Lightplayer has not SSelected a Character";
		darkPlayerCharDisplay.text = "Darkplayer has not SSelected a Character";
	}



	//public void CharacterSelectLight(Character character)
	//{
	//	Universals.lightCharacter = character;
	//}

	//public void CharacterSelectDark(Character character)
	//{
	//	Universals.darkCharacter = character;
	//}

	public void CharacterSelectLight(int charnum)   //ints for now, but eventually will use Character classes
	{
		//if charnum == 0, assign character0. else...
		Universals.lightCharacterTest = charnum;
		Debug.Log($"LightCharacter: {charnum}");
		lightCharSelected = true;

		lightPlayerCharDisplay.text = $"LightPlayer has selected Character: {charnum}";

		if (lightPlaySelected)
		{//If this player was previously the one Readied, now the Play button is back to default color
			EventScript.PlayUnselectColorChange.Invoke();
		}

		lightPlaySelected = false;  //If they Were ready, now they are not because they chose a new character
	}

	public void CharacterSelectDark(int charnum)
	{
		Universals.darkCharacterTest = charnum;
		Debug.Log($"DarkCharacter: {charnum}");
		darkCharSelected = true;

		darkPlayerCharDisplay.text = $"DarkPlayer has selected Character: {charnum}";

		if (darkPlaySelected)
		{//If this player was previously the one Readied, now the Play button is back to default color
			EventScript.PlayUnselectColorChange.Invoke();
		}

		darkPlaySelected = false;   //If they Were ready, now they are not because they chose a new character
	}

	public void PlayGame(string player)
	{
		if (player == "Light")
		{
			if (lightCharSelected)
			{//Only allow the player to select Play once they have chosen a character
				lightPlaySelected = !lightPlaySelected;
				//Toggle whether or not the Light Player has selected the Play Button
			}
		}
		else if (player == "Dark")
		{
			if (darkCharSelected)
			{//Only allow the player to select Play once they have chosen a character
				darkPlaySelected = !darkPlaySelected;
				//Toggle whether or not the Dark Player has selected the Play Button
			}
		}

		if (lightPlaySelected) { playBttnBehav.LightColorSet(); }
		else if (darkPlaySelected) { playBttnBehav.DarkColorSet(); }
		else { playBttnBehav.DefaultColorSet(); }


		if (lightPlaySelected && darkPlaySelected)
		{
			SceneManager.LoadScene("GameClassic");
		}
	}

	public void Back()
	{
		SceneManager.LoadScene(0);
	}

	void PlayUnselect()
	{
		playBttnBehav.DefaultColorSet(); 
	}

	public void OpenOptions(string player)
	{
		//Open the Options Canvas of the player that pushed the button, only allowing that player to move their cursor
		//Both options Canvases look identical except for the cursor color
		//Set the corresponding Bool to true, depending on which player pushed the button
		//the Overseer will use this in order to prevent the other player from having control 
	}

	public void CloseOptions()
	{

	}
}
