using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour, Interactable
{
    public void Interact()
    {
        Destroy(this.gameObject);
    }
}
