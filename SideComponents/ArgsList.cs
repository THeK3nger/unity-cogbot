using UnityEngine;
using System.Collections;

/*!
 * ArgsList stores an ordered list of arguments in an efficient way.
 */
public class ArgsList {

    private string[] args;

    // INDEXER. See http://msdn.microsoft.com/en-us/library/6x16t2tx.aspx for
    // more information.
    /*!
     * Indexer for the ArgsList class.
     */
    public string this[int i]
    {
        get
        {
            return args[i];
        }

        set
        {
            throw new System.InvalidOperationException("ArgsList are immutable!");
        }
    }

    /*!
     * Return the number of arguments in the list.
     */
    public int Length
    {
        get { return args.Length; }
    }

    /*!
     * Constructor. 
     * 
     * Build an ArgsList from a list of strings.
     */
    public ArgsList(params string[] args)
    {
        this.args = args;
    }

    /*!
     * Constructor.
     * 
     * Build an ArgsList from a single string.
     * 
     * @param args The input string.
     * @param separator The separator character between the arguments.
     */
    public ArgsList(string args, char separator = ' ')
    {
        this.args = args.Split(separator);
    }

    public string[] ToStringArray()
    {
        return args.Clone() as string[];
    }

    public override bool Equals(object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to ArgsList return false.
        ArgsList otherList = obj as ArgsList;
        if ((System.Object)otherList == null)
        {
            return false;
        }

        // Return true if other have the same elements in the same order.
        if (otherList.Length != this.Length) return false;
        for (int i = 0; i < this.Length; ++i)
        {
            if (otherList[i] != this[i]) return false;
        }
        return true;
    }

    public override int GetHashCode()
    {
        int prime = 31;
        int result = 1;
        foreach (string s in args)
        {
            result = prime * result + s.GetHashCode();
        }
        return result;
    }


}
