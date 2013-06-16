using UnityEngine;
using System.Collections;

public class BotAttributes : GridWorldBehaviour {

    public int goldCarrying { get; set; }
    public int goldStored { get; set; }
    public bool speedBoost;
    public int keys { get; set; }
    public int spawnArea { get; set; }
    public int life { get; set; }

	// Use this for initialization
	protected override void Awake () {
        base.Awake();
        Vector3 current = transform.position;
        spawnArea = mapWorld.GetArea(current.x, current.z);
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("GOLD CARRYING: " + goldCarrying);
	}
}
