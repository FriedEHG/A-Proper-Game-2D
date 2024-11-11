using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehav : MonoBehaviour
{

    public Team currentTeam;
    public Team startingTeam = Team.Dark;

    public Material darkMaterial;
    public Material lightMaterial;

    // Start is called before the first frame update
    void Start()
	{
		EventScript.BeginGame.AddListener(BeginGame);
		EventScript.NewRound.AddListener(BeginGame);
		BeginGame();
    }

    void Update()
    {
        
    }

    public void BeginGame()
    {
        currentTeam = startingTeam;

		if (currentTeam == Team.Light)
		{
			gameObject.GetComponent<Renderer>().material = lightMaterial;
			Vector3 newHeight = new Vector3(transform.position.x, transform.position.y, Universals.lightBrickHeight);
			transform.SetPositionAndRotation(newHeight, Quaternion.identity);
		}
		else
		{
			gameObject.GetComponent<Renderer>().material = darkMaterial;
			Vector3 newHeight = new Vector3(transform.position.x, transform.position.y, Universals.darkBrickHeight);
			transform.SetPositionAndRotation(newHeight, Quaternion.identity);
		}
	}

    public void ChangeTeam()
    {
        if (currentTeam == Team.Dark)
        {
            currentTeam = Team.Light;
            gameObject.GetComponent<Renderer>().material = lightMaterial;
			Vector3 newHeight = new Vector3(transform.position.x, transform.position.y, Universals.lightBrickHeight);
			transform.SetPositionAndRotation(newHeight, Quaternion.identity);
		}
        else
        {
			currentTeam = Team.Dark;
			gameObject.GetComponent<Renderer>().material = darkMaterial;
			Vector3 newHeight = new Vector3(transform.position.x, transform.position.y, Universals.darkBrickHeight);
			transform.SetPositionAndRotation(newHeight, Quaternion.identity);
		}
    }

	public enum Team
	{
		None = 0,
		Dark = 1,
		Light = 2,
	}
}
