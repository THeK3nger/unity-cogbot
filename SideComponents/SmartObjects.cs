using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartObjects : MonoBehaviour {

	public string type = " ";
	public bool isStatic = true;

    private List<BotControl> observers;

	// Use this for initialization
	void Awake () {
        observers = new List<BotControl>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /**
     * Add an observer to the object.
     * 
     * \param obs The observer controller.
     */
    public void AddObserver(BotControl obs)
    {
        observers.Add(obs);
    }

    /**
     * Remove an observer.
     * 
     * \param obs The observer controller
     */
    public void RemoveObserver(BotControl obs)
    {
        observers.Remove(obs);
    }

    /**
     * Notify a change in the object status that has to be sent to the
     * observers.
     */
    public void NotifyStateChange()
    {
        foreach (BotControl bc in observers)
        {
            bc.NotifyObjectChange(gameObject, type[0]);
        }
    }
}
