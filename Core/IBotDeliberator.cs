using UnityEngine;
using System.Collections;

/**
 * A public interface for the bot higher level.
 * 
 * IBotDeliberator offers a communication interface between BotControl
 * and the higher decisional level.
 *
 * \author Davide Aversa
 * \version 1.0
 * \date 2013
 */
public interface IBotDeliberator {

	/**
	 * Return the next bot action.
	 *
	 * The action is computed by the deliberator according to the information
	 * available in BotController.
	 *
	 * \return The next valid (?) bot action.
	 */
	string GetNextAction();

    void NotifyObjectChange(GameObject obj, char type);

    /**
     * Return the interesting object types for the deliberator.
     */
    string interestType { get; }

// TODO: Define a complete IBotDeliberator interface. GetNextAction is enought?

}
