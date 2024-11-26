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
		EventScript.CommenceTheGame.AddListener(BeginGame);
		EventScript.NewRound.AddListener(BeginGame);
		BeginGame();
    }

    void Update()
    {
        
    }

    public void BeginGame()
    {
        currentTeam = startingTeam;
		Vector3 newHeight;
		if (currentTeam == Team.Light)
		{
			gameObject.GetComponent<Renderer>().material = lightMaterial;
			newHeight = new Vector3(transform.position.x, transform.position.y, Universals.lightBrickHeight);
		}
		else
		{
			gameObject.GetComponent<Renderer>().material = darkMaterial;
			newHeight = new Vector3(transform.position.x, transform.position.y, Universals.darkBrickHeight);
		}
		transform.SetPositionAndRotation(newHeight, Quaternion.identity);
	}

	public void ChangeTeam()
    {
		//Debug.Log("Ball.ChangeTeam");
		EventScript.BrickBreak.Invoke(currentTeam, transform.position);

		Vector3 newHeight;
		if (currentTeam == Team.Dark)
        {
            currentTeam = Team.Light;
            gameObject.GetComponent<Renderer>().material = lightMaterial;
			newHeight = new Vector3(transform.position.x, transform.position.y, Universals.lightBrickHeight);
		}
        else
        {
			currentTeam = Team.Dark;
			gameObject.GetComponent<Renderer>().material = darkMaterial;
			newHeight = new Vector3(transform.position.x, transform.position.y, Universals.darkBrickHeight);
		}
		transform.SetPositionAndRotation(newHeight, Quaternion.identity);

	}

	public enum Team
	{
		None = 0,
		Dark = 1,
		Light = 2,
	}
}
