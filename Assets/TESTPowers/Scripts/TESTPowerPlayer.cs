using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

#if UNITY_EDITOR
using System.Reflection;
#endif


public class TESTPowerPlayer : MonoBehaviour
{

	[SerializeField] InputActionAsset PlayerInputAsset;
	InputActionMap GameControlsMap;

	public int health = 50;
	int maxHealth = 100;
	public float speed = 5f;

	Vector2 moveVector;

	InputAction GameMove;

	private void OnEnable()
	{
		PlayerInputAsset.Enable();
	}
	private void OnDisable()
	{
		PlayerInputAsset.Disable();
	}

	// Start is called before the first frame update
	void Start()
	{
		ControlsInitialize();
	}

	private void ControlsInitialize()
	{
		GameControlsMap = PlayerInputAsset.FindActionMap("GameControls");
		GameMove = PlayerInputAsset.FindAction("Movement");
	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR
		ClearLog();
		Debug.Log($"{health}/{maxHealth}");
#endif


		moveVector = new Vector3(GameMove.ReadValue<Vector2>().x, 0, GameMove.ReadValue<Vector2>().y) * speed;

		transform.Translate(moveVector);
	}

	private void ClearLog()
	{
		var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
		var type = assembly.GetType("UnityEditor.LogEntries");
		var method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
	}
}
