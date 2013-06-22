using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// This class is an action interface for the BotControl.
/// </summary>
/// 
/// The action must be registerd in this class through the RegisterNewAction 
/// function.
/// 
/// These actions can be invoked by BotControl.
/// 
/// \author Davide Aversa
/// \version 2.0
/// \date 2013
/// \pre This class needs an instance BotControl.
[RequireComponent(typeof(BotControl))]
public class BotActions : MonoBehaviour
{
	
	/// <summary>
	/// Dictionary that map commands strings to actions.
	/// </summary>
	private Dictionary<string,Action<string[]>> actions;
	
	/// <summary>
	/// The abort action.
	/// </summary>
	private Action abortAction;
	
	/// <summary>
	/// True if the last action is completed.
	/// </summary>
	private bool actionComplete = true;
	
	/// <summary>
	/// True if the last action is completed successfully.
	/// </summary>
	private bool actionSuccess = true;
	
	/// <summary>
	/// A reference to a BotControl instance.
	/// </summary>
	private BotControl parentControl;

	void Awake ()
	{
		parentControl = gameObject.GetComponent<BotControl> ();
		actions = new Dictionary<string, Action<string[]>> ();
	}

	/// <summary>
	/// Registers an action to the BotActions component.
	/// </summary>
	/// 
	/// This operation is needed in order to execute the desired action.
	/// 
	/// <param name='command'>
	/// The command associated to the action.
	/// </param>
	/// <param name='action'>
	/// A function that execute the desired action.
	/// </param>
	public void RegisterNewAction (string command, Action<string[]> action)
	{
		Debug.Log ("BotACTIONS: Registering " + command + " command");
		actions [command] = action;
	}

	/// <summary>
	/// Register the action that has to be executed to block every other action 
	/// in progress.
	/// </summary>
	/// <param name='abortAction'>
	/// The "stop" action function.
	/// </param>
	public void RegisterAbortAction (Action abortAction)
	{
		this.abortAction = abortAction;
	}

	/// <summary>
	/// Perform the given action (if exists).
	/// </summary>
	/// <returns>
	/// True if the action can be executed. False otherwise.
	/// </returns>
	/// <param name='fullCommand'>
	/// The action that must be executed.
	/// </param>
	public bool DoAction (string fullCommand)
	{
		Debug.Log ("Action Received: " + fullCommand);
		string[] commands = fullCommand.Split (' ');
		if (fullCommand == "stop") {
			abortAction.Invoke ();
			return true;
		}
		if (actionComplete && actions.ContainsKey (commands [0])) {
			actionComplete = false;
			actionSuccess = false;
			actions [commands [0]].Invoke (commands);
		}
		return false;
	}

	/// <summary>
	/// Check if the lasts the action is complete.
	/// </summary>
	/// <returns>
	/// True if the last action is completed. False otherwise.
	/// </returns>
	public bool LastActionComplete ()
	{
		return actionComplete;
	}

	/// <summary>
	/// Check if the last action is completed succesfully.
	/// </summary>
	/// <returns>
	/// True if the last action is completed succesfully. False otherwise.
	/// </returns>
	public bool LastActionCompletedSuccessfully ()
	{
		return actionComplete && actionSuccess;
	}

	/// <summary>
	/// Notify to BotAction that the action is complete.
	/// </summary>
	public void NotifyActionComplete (string action)
	{
		actionComplete = true;
		NotifyAction(action);
	}

	/// <summary>
	/// Notify to BotAction that the action is completed succesfully.
	/// </summary>
	public void NotifyActionSuccess ()
	{
		actionSuccess = true;
	}

	/// <summary>
	/// Notify the action to the BotController.
	/// </summary>
	/// <param name='action'>
	/// The name of action succesfully completed.
	/// </param>(
	private void NotifyAction (string action)
	{
		parentControl.NotifyAction (action);
	}

}
