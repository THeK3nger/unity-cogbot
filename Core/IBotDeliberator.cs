using UnityEngine;
using System.Collections;

/// <summary>
/// A public interface for the bot higher level.
/// </summary>
/// 
/// IBotDeliberator offers a communication interface between BotControl
/// and the higher decisional level.
/// 
/// \author Davide Aversa
/// \version 1.0
/// \date 2013
public interface IBotDeliberator {

	/// <summary>
	/// Return the next bot action.
	/// </summary>
	/// 
	/// The action is computed by the deliberator according to the information
	/// available in BotController.
	/// 
	/// <returns>
	/// The next valid (?) bot action.
	/// </returns>
	string GetNextAction();
	
	/// <summary>
	/// Notifies the object change.
	/// </summary>
	/// <param name='obj'>
	/// The changing object.
	/// </param>
	/// <param name='type'>
	/// The object type.
	/// </param>
	/// <param name='isLeaving'>
	/// Set this to true if the notification comes from a leaving object.
	/// </param>	
	void NotifyObjectChange(GameObject obj, string type, bool isLeaving = false);

	/// <summary>
	/// List of the interesting object types for the deliberator.
	/// </summary>
	/// <value>
	/// The type of the interest for the deliberator.
	/// </value>
    string[] interestType { get; }

}
