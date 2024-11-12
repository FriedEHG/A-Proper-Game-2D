using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBase : MonoBehaviour
{

	public PowerScriptableObj powerTemplate;

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log($"Power Triggered: {other.gameObject.name}");
		if (other.GetComponentInParent<Player>() != null)
		{
			//Debug.Log("Power ActivatedAAAAAAAAAAAaaa");
			Destroy(gameObject);        //Self destruct
			powerTemplate.Apply(other.gameObject);  //Apply effect
		}
	}
}
