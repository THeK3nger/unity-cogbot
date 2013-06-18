using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*!
 * StateBook stores informations about predicative conditions.
 * 
 * For example StateBook can store predicates like `Location(2,3,bot1)` or
 * `Hold(gun)` usefull in deliberative algorithm and tecniques like planning
 * or FSM.
 * 
 * Statebook class is easy to use. To add a conditions you just have to
 * 
 *     statebook["name:arg1 arg2 arg3"] = true;
 *     
 * In the same way we can check the validity of a given conditions with
 * 
 *     statebook["name:arg1 arg2 arg3"];
 *     
 * Statebook use the Closed-World Assumption.
 */
public class StateBook : MonoBehaviour {

    private Dictionary<string, HashSet<ArgsList>> conditionsDB;
    private Dictionary<string, int> predicatesArity;
    
	// Use this for initialization
	void Start () {
        conditionsDB = new Dictionary<string, HashSet<ArgsList>>();
        predicatesArity = new Dictionary<string, int>();

        /* TEST */
        this["bob:1 2 cane"] = true;
        this["bob:1 3 pollo"] = true;
        this["bob:3 5 cane"] = true;
        foreach (string[] ss in this.GetEnumerator("bob", "$1 $2 cane"))
        {
            Debug.Log(ss[0] + " " + ss[1]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    /*!
     * Query the database for the validity of a given expression.
     * 
     * \param name The predicate name.
     * \param args The list of arguments of the predicate.
     * \retval true If the query is valid in the DB.
     * \retvale false Otherwise.
     */
    public bool Query(string name, string[] args) 
    {
        if (!conditionsDB.ContainsKey(name)) return false;
        return conditionsDB[name].Contains(new ArgsList(args)); 
    }

    /*!
     * Query the database for the validity of a given expression.
     * 
     * \param name The predicate name.
     * \param args The list of arguments of the predicate separated by spaces.7
     * \retval true If the query is valid in the DB.
     * \retvale false Otherwise.
     */
    public bool Query(string name, string args)
    {
        if (!conditionsDB.ContainsKey(name)) return false;
        return conditionsDB[name].Contains(new ArgsList(args));
    }

    /*!
     * Query the database for the validity of a given expression.
     * 
     * \param query The query string. The format is "name:arg1 arg2 ...".
     * \retval true If the query is valid in the DB.
     * \retvale false Otherwise.
     */
    public bool Query(string query)
    {
        string[] splitted = query.Split(':');
        return this.Query(splitted[0], splitted[1]);
    }

    /*!
     * Set the given query with the given value.
     * 
     * \param query The input query string.
     * \param value The desired value.
     */
    public void SetValue(string query, bool value)
    {
        Debug.Log(query);
        string[] splitted = query.Split(':');
        string theName = splitted[0];
        string args = splitted[1];
        int argsNum = args.Split(' ').Length;
        // If true add the arglist to the list. If false remove it.
        if (value == true)
        {
            // If the condition do not exist, create it.
            if (!conditionsDB.ContainsKey(theName))
            {
                conditionsDB.Add(theName, new HashSet<ArgsList>());
                predicatesArity.Add(theName, argsNum);
            }
            // You cannot add an arglist with different number of arguments for
            // an existent condition.
            if (argsNum != predicatesArity[theName])
            {
                throw new System.InvalidOperationException("Invalid arguments number!");
            }
            conditionsDB[theName].Add(new ArgsList(args));
        }
        else
        {
            // If you set a false condition you are removing a positive condition
            // in the DB (if any).
            if (!conditionsDB.ContainsKey(theName))
                return;
            conditionsDB[theName].Remove(new ArgsList(args));
        }
    }

    // ENUMERATORS
    /*!
     * Enumerator for the properties name in the database.
     */
    public System.Collections.IEnumerable GetConditionsEnumerator()
    {
        foreach (KeyValuePair<string, HashSet<ArgsList>> entry in conditionsDB)
        {
            yield return entry.Key;
        }
    }

    /*!
     * Enumerator for a given property.
     * 
     * This enumerator return all the argument tuple associated to a given
     * condition.
     * 
     * /param name The condition name.
     */
    public System.Collections.IEnumerable GetEnumerator(string name)
    {
        foreach (ArgsList al in conditionsDB[name])
        {
            yield return al.ToStringArray();
        }
    }

    /*
     * Enumerator for selective query.
     * 
     * This enumerator return all the argument tuple associated to a given 
     * condition that match the given template.
     * 
     * A template is a string of the form "$var1 fixed1 fixed2 $var2 ..." where
     * the element starting with the $ symbol are variable and the fixed
     * ones must match the database argument.
     * 
     * \param name The property name.
     * \param template The template string.     * 
     */
    public System.Collections.IEnumerable GetEnumerator(string name, string template)
    {
        if (conditionsDB.ContainsKey(name))
        {
            int unknow = 0;
            string[] splitted = template.Split(' ');
            foreach (string s in splitted)
            {
                if (s.StartsWith("$"))
                {
                    unknow++;
                }
            }
            foreach (ArgsList al in conditionsDB[name])
            {
                string[] result = new string[unknow];
                int idx = 0;
                bool iPickThis = true;
                for (int i = 0; i < al.Length; ++i)
                {
                    if (splitted[i].StartsWith("$"))
                    {
                        result[idx] = al[i];
                        idx++;
                    }
                    else
                    {
                        // If one item is 
                        iPickThis = iPickThis && (splitted[i] == al[i]);
                        if (!iPickThis) break;
                    }
                }
                if (iPickThis) yield return result;
            }
        }
    }

    // INDEXERS
    // Query string is in the form: "condition_name:arg1 arg2 arg3 ..."
    public bool this[string query]
    {
        get
        {
            return Query(query);
        }

        set
        {
            SetValue(query, value);
        }
    }
}
