using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Implement a perception by collision system for the attached object.
/// </summary>
/// \author Davide Aversa
/// \version 1.0
/// \date 2013
/// \pre This component must be attached to a *perception mesh* attached to the bot.
/// The bot must have a BotControl instance attached to itself.
[RequireComponent (typeof(Collider))]
public class BotPerception : MonoBehaviour
{
	
	/// <summary>
	/// Eanble the built-in deep test for the entering object.
	/// </summary>
	public bool raycastTest = true;
	
	/// <summary>
	/// A reference to the IBotControl instance attache to the bot.
	/// </summary>(
	private BotControl parentControl; 
	
	/// <summary>
	/// List of the object inside the perception mesh.
	/// </summary>
	private List<GameObject> objectInMesh;

	protected void Awake ()
	{
		// Extract the controller component from the parent object.
		parentControl = gameObject.transform.parent.gameObject.GetComponent<BotControl> ();
		objectInMesh = new List<GameObject> ();
	}
	
	// Update is called once per frame
	// TODO: Remove.
	void Update ()
	{
	
	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name='other'>
	/// The object which is entering the perception mesh.
	/// </param>
	void OnTriggerEnter (Collider other)
	{
		GameObject obj = other.gameObject;	// Reference to the entering object.
		if (raycastTest) {
			GameObject bot = gameObject.transform.parent.gameObject; // Reference to the bot object.
			if (RayCastVisibility (obj, bot)) {
				parentControl.objectEnteringFOV (obj);
			}
			return;
		}
		SmartObjects so = obj.GetComponent<SmartObjects> ();
		if (so != null) {
			so.AddObserver (this);	
		}
		// Add to the object list.
		objectInMesh.Add (obj);
		// Notify ingress to the controller.
		parentControl.objectEnteringFOV (obj);
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name='other'>
	/// The object which is leaving the perception mesh.
	/// </param>
	void OnTriggerExit (Collider other)
	{
		GameObject obj = other.gameObject;
		SmartObjects so = obj.GetComponent<SmartObjects> ();
		if (so != null) {
			so.RemoveObserver (this);	
		}
		// Add to the object list.
		objectInMesh.Remove (obj);
		// Notify ingress to the controller.
		parentControl.objectLeavingFOV (obj);
	}
	
	/// <summary>
	/// Notifies the object change.
	/// </summary>
	/// <param name='go'>
	/// The changing game object.
	/// </param>
	/// <param name='type'>
	/// The object type.
	/// </param>
	public void NotifyObjectChange (GameObject go, char type)
	{
		parentControl.NotifyObjectChange(go,type);
	}

	/// <summary>
	/// Check for raycast visibility.
	/// </summary>
	/// <returns>
	/// True if the object is visible from the bot. False otherwise.
	/// </returns>
	/// <param name='obj'>
	/// The target object.
	/// </param>
	/// <param name='bot'>
	/// The bot object.
	/// </param>
	private bool RayCastVisibility (GameObject obj, GameObject bot)
	{
		RaycastHit hit = new RaycastHit ();
		Vector3 offset = new Vector3 (0, 1, 0);
		// Direction between obj and other.
		Vector3 direction = (obj.transform.position - (bot.transform.position + offset)).normalized;
		Physics.Raycast (bot.transform.position + offset, direction, out hit);
		return hit.transform.gameObject.Equals (obj);
	}
	
}
