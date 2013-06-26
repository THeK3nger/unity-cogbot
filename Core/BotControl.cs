using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * The main brain of a Bot.
 *
 * The Bot Controller class is the core of the Bot AI. It is the nexus between all the AI elements
 * like perception, action and planning/behavior components.
 * 
 * \author Davide Aversa
 * \version 1.0
 * \date 2013
 */
public class BotControl : MonoBehaviour
{

	// CONTROL INSPECTOR PARAMETERS
	public bool onDemand = false;			//If true the think loop must be executed on demand.
	public float thinkTick = 1;				//Time interval between a think cicle.
	public string deliberatorName;			//Name of the IBotDeliberator implementation.

	private BotActions botActions;  		//Reference to the BotAction component.
	private IBotDeliberator deliberator;	//Reference to a IBotDeliberator interface.
	
	private List<GameObject> objectInFov; 	// Contains the list of object in the FOV.

	// STATE
	private enum Status { IDLE, EXECUTING };
	private Status controlStatus;			// Controller Status.

    //public StateBook internalKnowledge;

	// Use this for initialization
    protected void Awake()
    {
        controlStatus = Status.IDLE;
        objectInFov = new List<GameObject>();
        botActions = gameObject.GetComponent<BotActions>();
        deliberator = gameObject.GetComponent(deliberatorName) as IBotDeliberator;
        // Run Thread Function Every `n` second
		if (!onDemand) {
        	InvokeRepeating("ThinkLoop", 0, thinkTick);
		}
    }

	/**
	 * CheckCondition parse a condition formula and return a single boolean value.
	 *
	 * TODO: Define formula syntax.
	 * 
	 * \param condition The input condition.
	 * \return The thruth value for the condition formula.
	 */
	public bool CheckCondition(string condition) {
		// PARSE AND
		string[] andConditions = condition.Split('&');
		if (andConditions.Length > 1) {
			foreach (string c in andConditions) {
				if (!CheckCondition(c)) return false;
			}
			return true;
		}
		// PARSE OR
		string[] orConditions = condition.Split('|');
		if (orConditions.Length > 1) {
			foreach (string c in orConditions) {
				if (CheckCondition(c)) return true;
			}
			return false;
		}
		// PARSE CONDITION
		bool not = condition.StartsWith("!");
		if (not) condition = condition.Substring(1);
		switch (condition) {
		default :
			return false; //TODO: Default true or default false?
		}
	}

	// TODO: ThinkLoop 
	public void ThinkLoop() {
		if (controlStatus == Status.IDLE) {
			string nextaction = deliberator.GetNextAction();
			Debug.Log("NEXT ACTION: " + nextaction);
            if (nextaction != "")
            {
                controlStatus = Status.EXECUTING;
                botActions.DoAction(nextaction);
            }
		}
	}

    public void DoAction(string command)
    {
        botActions.DoAction(command);
    }

	/**
	 * Used by BotAction to notify the controller about the success of the given action.
	 * 
	 * \param action The action notification string (TODO: to be defined).
	 */
	public void NotifyAction(string action) {
		controlStatus = Status.IDLE;
        switch (action)
        {
            case "grab":
                Debug.Log("Grab Completed");
                break;
            default:
                break;
        }
	}

    /**
     * Notification callback for an object in the FOV which are changing its state.
     * 
     * \param The changing object.
     */
    public void NotifyObjectChange(GameObject obj)
    {
        if (System.Array.IndexOf(deliberator.interestType,obj.tag) != -1)
        {
            deliberator.NotifyObjectChange(obj);
        }
    }
	
	public void NotifyObjectChange(GameObject obj, bool isLeaving)
    {
        if (System.Array.IndexOf(deliberator.interestType,obj.tag) != -1)
        {
            deliberator.NotifyObjectChange(obj, isLeaving);
        }
    }

}
