using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerBase : MonoBehaviour
{
	public PowerScriptableObj pongPowerScriptableObj;
	public team currentTeam;    //designated in the Inspector
	public PowerObject ourPowerObject;

	PowerManager powerManager;


	private void Start()
	{
		ourPowerObject = gameObject.GetComponent<PowerObject>();
		this.GameObject().GetComponentInParent<PowerManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log($"Power Triggered: {other.gameObject.name}");
		if (other.GetComponentInParent<Player>() != null
			&& other.GetComponentInParent<Player>().currentTeam.ToString() == currentTeam.ToString())
		{
			//Debug.Log("Power ActivatedAAAAAAAAAAAaaa");
			ourPowerObject.ToasterBath();           //Deactivate
			Tuple<PongPower, float> returnedTuple = pongPowerScriptableObj.ApplyForTime(other.gameObject);
			powerManager.PowerCountdownBegin(returnedTuple.Item1, returnedTuple.Item2);
			//^^ Apply effect 

			
		}
	}

	public enum team
	{
		CHANGETHIS = 0,
		Dark,
		Light
	}
}
