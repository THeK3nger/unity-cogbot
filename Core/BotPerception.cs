using UnityEngine;
using System.Collections;

/**
 * Implement a perception by collision system for the attached object.
 * 
 * \author Davide Aversa
 * \version 1.0
 * \date 2013
 * \pre This component must be attached to a *perception mesh* attached to the bot.
 * The bot must have a BotControl instance attached to itself.
 */
[RequireComponent (typeof (Collider))]
public class BotPerception : GridWorldBehaviour {

	public bool raycastTest = false;
    public bool rasterTest = true;

	private BotControl parentControl;	/**< A reference to the IBotControl instance attache to the bot. */

	// Use this for initialization
	protected override void Awake () {
        base.Awake();
		parentControl = gameObject.transform.parent.gameObject.GetComponent<BotControl>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**
	 * Collision callback for entering objects.
	 */
    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;	// Reference to the entering object.
        if (raycastTest)
        {
            GameObject bot = gameObject.transform.parent.gameObject; // Reference to the bot object.
            if (RayCastVisibility(obj, bot))
            {
                parentControl.objectEnteringFOV(obj);
            }
            return;
        }
        if (rasterTest)
        {
            GameObject bot = gameObject.transform.parent.gameObject; // Reference to the bot object.
            if (RasterVisibility(obj, bot))
            {
                parentControl.objectEnteringFOV(obj);
            }
            return;
        }
        parentControl.objectEnteringFOV(obj);
    }

	/**
	 * Collistion callback fot exiting objects.
	 */
	void OnTriggerExit(Collider other) {
		GameObject obj = other.gameObject;
		parentControl.objectLeavingFOV(obj);
    }

    /**
     * Check for raycast visibility.
     * 
     * \param obj The target object.
     * \param bot The bot object.
     * \return True if the object is visible from the bot. Flase otherwise.
     */
    private bool RayCastVisibility(GameObject obj, GameObject bot)
    {
        RaycastHit hit = new RaycastHit();
        Vector3 offset = new Vector3(0, 1, 0);
        // Direction between obj and other.
        Vector3 direction = (obj.transform.position - (bot.transform.position + offset)).normalized;
        Physics.Raycast(bot.transform.position + offset, direction, out hit);
        return hit.transform.gameObject.Equals(obj);
    }

    /**
     * Check for raster visibility.
     * 
     * This algorithm is a variation of a supercover algorithm based on 
     * Bresenham rastering algorithm.
     * 
     * It use the global map to check if there are obstacles in the line
     * between the bot and the object.
     * 
     * More reference at http://lifc.univ-fcomte.fr/~dedu/projects/bresenham/index.html
     * 
     * \param obj The target object.
     * \param bot The bot object.
     * \return True if the object is visible from the bot. Flase otherwise.
     */
    private bool RasterVisibility(GameObject obj, GameObject bot)
    {
        Vector3 botPos = bot.transform.position;
        Vector3 objPos = obj.transform.position;
        int[] botIJ = mapWorld.GetIndexesFromWorld(botPos.x, botPos.z);
        int[] objIJ = mapWorld.GetIndexesFromWorld(objPos.x, objPos.z);
        int y = botIJ[0];
        int x = botIJ[1];
        int dx = objIJ[1] - botIJ[1];
        int dy = objIJ[0] - botIJ[0];
        int xstep, ystep;
        if (IsOpaque(y,x)) return false;
        if (dy < 0) { ystep = -1; dy = -dy; } else ystep = 1;
        if (dx < 0) { xstep = -1; dx = -dx; } else xstep = 1;
        int ddx = 2 * dx;
        int ddy = 2 * dy;
        if (ddx >= ddy)
        {
            int errorprev = dx;
            int error = dx;
            for (int c = 0; c < dx-1; c++)
            {
                x += xstep;
                error += ddy;
                if (error > ddx)
                {
                    y += ystep;
                    error -= ddx;
                    if (error + errorprev <= ddx) { if (IsOpaque(y - ystep, x)) return false; }
                    if (error + errorprev >= ddx) { if (IsOpaque(y, x - xstep)) return false; }
                }
                if (IsOpaque(y, x)) return false;
                errorprev = error;
            }
        }
        else
        {
            int errorprev = dy;
            int error = dy;
            for (int c = 0; c < dy-1; c++)
            {
                y += ystep;
                error += ddx;
                if (error > ddy)
                {
                    x += xstep;
                    error -= ddy;
                    if (error + errorprev >= ddy) { if (IsOpaque(y - ystep, x)) return false; }
                    if (error + errorprev <= ddy) { if (IsOpaque(y, x - xstep)) return false; }
                }
                if (IsOpaque(y, x)) return false;
                errorprev = error;
            }
        }
        return true;
    }

    private bool IsOpaque(int i, int j)
    {
        return mapWorld.ElementIs("opaque", i, j);
    }
	
}
