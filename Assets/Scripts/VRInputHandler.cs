// VRInputHandler.cs
// PlanetGenVR
// Ryan Vitacion
// Description: Implements VR control scheme for PlanetGenVR rendering program.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class VRInputHandler : MonoBehaviour {

  //movement speed variable
  public float speed = 10.0f;

  //gameobject references
  public GameObject controls;
  private Rigidbody rb;
  private Transform cam;
  private int currentScene;

  //input axis variables
  public SteamVR_Action_Vector2 trackpad;
  public SteamVR_Action_Single trigger;
  Vector2 trackpadValueR;
  Vector2 trackpadValueL;
  float triggerValueR;
  float triggerValueL;

  //get current scene index and camera reference on startup
  void Start() {
    currentScene = SceneManager.GetActiveScene().buildIndex;
    rb = GetComponent<Rigidbody>();
  }
	
  //method to check input at every fixed frame
	void FixedUpdate () {

    //get camera position and orientation
    cam = Camera.main.transform;

    //assign trackpad and trigger axis variables
    trackpadValueR = trackpad.GetAxis(SteamVR_Input_Sources.RightHand);
    trackpadValueL = trackpad.GetAxis(SteamVR_Input_Sources.LeftHand);
    triggerValueR = trigger.GetAxis(SteamVR_Input_Sources.RightHand);
    triggerValueL = trigger.GetAxis(SteamVR_Input_Sources.LeftHand);

    //quit application is escape key is pressed
    if (Input.GetKeyDown("escape")) Application.Quit();

    //switch to next scene if right menu button is pressed
    if (SteamVR_Input._default.inActions.menuButton.GetStateDown(SteamVR_Input_Sources.RightHand)) {

      if (currentScene == 4) SceneManager.LoadScene(0);
      else SceneManager.LoadScene(currentScene+1);

    }

    //switch to previous scene if left menu button is pressed
    if (SteamVR_Input._default.inActions.menuButton.GetStateDown(SteamVR_Input_Sources.LeftHand)) {

      if (currentScene == 0) SceneManager.LoadScene(4);
      else SceneManager.LoadScene(currentScene-1);

    }

    //toggle controls UI if left trackpad button is pressed
    if (SteamVR_Input._default.inActions.trackpadButton.GetStateDown(SteamVR_Input_Sources.LeftHand)) controls.SetActive(!controls.activeSelf);

    //increase speed if right trackpad button is held down
    if (SteamVR_Input._default.inActions.trackpadButton.GetStateDown(SteamVR_Input_Sources.RightHand)) speed = 50;
    if (SteamVR_Input._default.inActions.trackpadButton.GetStateUp(SteamVR_Input_Sources.RightHand)) speed = 10;

    //move camera based on left and right trackpad inputs
    rb.MovePosition(transform.position + (cam.forward * trackpadValueR.y * speed) + (cam.right * trackpadValueR.x * speed) + (cam.up * trackpadValueL.y * speed));

    //rotate camera left/right if left/right trigger is pulled
    if (triggerValueR > 0) transform.RotateAround(cam.position, cam.forward, -0.1f);
    if (triggerValueL > 0) transform.RotateAround(cam.position, cam.forward, 0.1f);

    //rotate camera up/down if right/left grip button is pressed
    if (SteamVR_Input._default.inActions.gripButton.GetState(SteamVR_Input_Sources.RightHand)) transform.RotateAround(cam.position, cam.right, -0.1f);
    if (SteamVR_Input._default.inActions.gripButton.GetState(SteamVR_Input_Sources.LeftHand)) transform.RotateAround(cam.position, cam.right, 0.1f);
	}

}
