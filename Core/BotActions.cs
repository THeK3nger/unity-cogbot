using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/**
 * This class is an action interface for the BotControl.
 * 
 * The action must be registerd in this class through the RegisterNewAction 
 * function.
 * 
 * These actions can be invoked by BotControl.
 * 
 * \author Davide Aversa
 * \version 1.9
 * \date 2013
 * \pre This class needs an instance BotControl.
 */
[RequireComponent(typeof(BotControl))]
public class BotActions : MonoBehaviour {

    private Dictionary<string,Action<string[]>> actions;
    private Action abortAction;

	private bool actionComplete = true;		/**< True if the last action is completed. */
	private bool actionSuccess = true;		/**< True if the last action is completed successfully. */ 

	private BotControl parentControl;		/**< A reference to a BotControl instance. */


	// Use this for initialization
	void Awake () {
		parentControl = gameObject.GetComponent<BotControl>();
        actions = new Dictionary<string, Action<string[]>>();
	}

    /**
     * Register an action to the BotAction component.
     * 
     * This operation is needed in order to execute the desired action.
     * 
     * \param command The command associated to the action.
     * \param action A function that execute the desired action.
     */
    public void RegisterNewAction(string command, Action<string[]> action)
    {
        Debug.Log("BotACTIONS: Registering " + command + " command");
        actions[command] = action;
    }

    /**
     * Register the action that has to be executed to block every other action
     * in progress.
     * 
     * \param The "stop" action function.
     */
    public void RegisterAbortAction(Action abortAction)
    {
        this.abortAction = abortAction;
    }

	/**
	 * Perform the given action (if exists).
	 * 
	 * \param action The action that must be executed.
	 * \retval true If the action can be executed.
	 * \retval false If the action can not be executed.
	 */
    public bool DoAction(string fullCommand)
    {
        Debug.Log("Action Received: " + fullCommand);
        string[] commands = fullCommand.Split(' ');
        if (fullCommand == "stop")
        {
            abortAction.Invoke();
            return true;
        }
        if (actionComplete && actions.ContainsKey(commands[0]))
        {
            actionComplete = false;
            actionSuccess = false;
            actions[commands[0]].Invoke(commands);
        }
        return false;
    }

	/*!
	 * Check if the last action is completed.
	 * 
	 * \retval true If the last action is completed.
	 * \retval false If the last action is still running.
	 */
	public bool LastActionComplete() {
		return actionComplete;
	}

	/**
	 * Check if the last action is completed succesfully.
	 * 
	 * \retval true If the last action is completed succesfully.
	 * \retval false If the last action is not completed or completed with faliure.
	 */
	public bool LastActionCompletedSuccessfully() {
		return actionComplete && actionSuccess;
	}

    /**
     * Notify to BotAction that the action is complete.
     */
    public void NotifyActionComplete()
    {
        actionComplete = true;
    }

    /**
     * Notify the BotAction that the action is completed succesfully.
     */
    public void NotifyActionSuccess()
    {
        actionSuccess = true;
    }

    /**
     * Notify the action to the BotController.
     * 
     * \param action The action succesfull.
     */
    public void NotifyAction(string action)
    {
        parentControl.NotifyAction(action);
    }

}
