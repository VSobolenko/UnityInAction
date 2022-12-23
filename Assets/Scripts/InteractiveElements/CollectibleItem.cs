using System;
using System.Collections;
using System.Collections.Generic;
using InteractiveElements.NativeMVC;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectibleItem : MonoBehaviour
{
    [FormerlySerializedAs("name")] [SerializeField] private string itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (Managers.Inventory == null)
            return;

        Managers.Inventory.AddItem(itemName);
        Destroy(gameObject);
    }
}
