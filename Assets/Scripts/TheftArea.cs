using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that aggregates TheftObjects on level
/// </summary>
public class TheftArea : MonoBehaviour
{
    public int CurrentObjectCount => _theftObjects.Count;

    private readonly HashSet<TheftObject> _theftObjects = new();
    
    public void AddTheftObjectToArea(TheftObject theftObject)
    {
        _theftObjects.Add(theftObject);
    }
    
    public void RemoveTheftObjectToArea(TheftObject theftObject)
    {
        _theftObjects.Remove(theftObject);
    }
}