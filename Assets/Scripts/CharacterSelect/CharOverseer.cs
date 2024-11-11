using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharOverseer : MonoBehaviour
{
	public EventSystem eventSystemDark;
	public EventSystem eventSystemLight;

	public InputActionAsset inputActionAssetDark;
	public InputActionAsset inputActionAssetLight;

	InputAction MenuMoveDark;
	InputAction MenuSelectDark;

	InputAction MenuMoveLight;
	InputAction MenuSelectLight;

	PlaySelMenuBehav menuBehaviour;


	private void OnEnable()
	{
		inputActionAssetDark.Enable();
		inputActionAssetLight.Enable();
	}

	private void OnDisable()
	{
		inputActionAssetDark.Disable();
		inputActionAssetLight.Disable();
	}


	void Start()
	{
		VariableInitialize();
		ControlsInitialize();
		//Debug.Log(characterTest);
		
	}

	void VariableInitialize()
	{
		eventSystemDark.enabled = false;
		eventSystemLight.enabled = true;

		eventSystemLight.firstSelectedGameObject.GetComponent<Button>().Select();

		eventSystemDark.enabled = true;
		eventSystemLight.enabled = false;

		eventSystemDark.firstSelectedGameObject.GetComponent<Button>().Select();

		menuBehaviour = FindFirstObjectByType<PlaySelMenuBehav>();


	}


	private void ControlsInitialize()
	{
		MenuMoveDark = inputActionAssetDark.FindAction("Navigate");
		MenuSelectDark = inputActionAssetDark.FindAction("Click");

		MenuMoveLight = inputActionAssetLight.FindAction("Navigate");
		MenuSelectLight = inputActionAssetLight.FindAction("Click");
	}


	void Update()
    {
        if (MenuMoveDark.WasPressedThisFrame() || MenuSelectDark.WasPressedThisFrame())
		{
			eventSystemDark.enabled = true;
			eventSystemLight.enabled = false;
		} 
		else 
		if (MenuMoveLight.WasPressedThisFrame() || MenuSelectLight.WasPressedThisFrame())
		{
			eventSystemDark.enabled = false;
			eventSystemLight.enabled = true;
		}
	}
}
