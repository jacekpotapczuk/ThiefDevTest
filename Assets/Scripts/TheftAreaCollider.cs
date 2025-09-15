using System;
using UnityEngine;

/// <summary>
/// Dummy component to mark Theft area colliders on level.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TheftAreaCollider : MonoBehaviour
{
    public TheftArea TheftArea { get; private set; }
    
    private void Awake()
    {
        TheftArea = GetComponentInParent<TheftArea>();

        if (TheftArea == null)
        {
            throw new Exception($"{nameof(TheftAreaCollider)} is expected to be placed as child of {nameof(TheftArea)}");
        }
    }
}
