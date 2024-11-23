using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBase : MonoBehaviour
{
	public PowerScriptableObj powerTemplate;
	public team currentTeam;	//designated in the Inspector

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log($"Power Triggered: {other.gameObject.name}");
		if (other.GetComponentInParent<Player>() != null
			&& other.GetComponentInParent<Player>().currentTeam.ToString() == currentTeam.ToString())
		{
			//Debug.Log("Power ActivatedAAAAAAAAAAAaaa");
			this.gameObject.SetActive(false);       //Deactivate
			powerTemplate.Apply(other.gameObject);  //Apply effect
		}
	}

	public enum team
	{
		CHANGETHIS = 0,
		Dark,
		Light
	}
}
