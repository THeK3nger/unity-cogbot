using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A Smart Object component is used to handle stateful objects in the BotPerception
/// component.
/// </summary>
/// \author Davide Aversa
/// \version 2.0
/// \date 2013
public class SmartObjects : MonoBehaviour {
	
	/// <summary>
	/// The object type.
	/// </summary>
	/// Deprecated.
	public string type = " ";
	
	/// <summary>
	/// Check if the object is static or not.
	/// </summary>
	/// Deprecate.
	public bool isStatic = true;
	
	/// <summary>
	/// The list of observers.
	/// </summary>
    private List<BotPerception> observers;

	// Use this for initialization
	void Awake () {
        observers = new List<BotPerception>();
	}

	/// <summary>
	/// Add an observer to the object.
	/// </summary>
	/// <param name='obs'>
	/// The observer perception component.
	/// </param>
    public void AddObserver(BotPerception obs)
    {
        observers.Add(obs);
    }

	/// <summary>
	/// Remove an observer from the object.
	/// </summary>
	/// <param name='obs'>
	/// The observer perception component.
	/// </param>
    public void RemoveObserver(BotPerception obs)
    {
        observers.Remove(obs);
    }

	/// <summary>
	/// Notifies the state change.
	/// </summary>
    public void NotifyStateChange()
    {
        foreach (BotPerception bp in observers)
        {
            bp.NotifyObjectChange(gameObject, type[0]);
        }
    }
}
