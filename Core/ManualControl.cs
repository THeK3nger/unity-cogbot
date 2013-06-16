using UnityEngine;
using System.Collections;

/**
 * Keyboard Control for a Bot
 * 
 * Implement basic keyboard commands for a Bot.
 */
public class ManualControl : MonoBehaviour
{

    BotActions botActions;

    // Use this for initialization
    void Start()
    {
        botActions = gameObject.GetComponent<BotActions>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            botActions.DoAction("grab");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            botActions.DoAction("drop");
        }

    }
}
