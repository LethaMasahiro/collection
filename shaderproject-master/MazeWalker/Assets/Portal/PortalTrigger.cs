using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{

    private Dictionary<GameObject, int> layers = new Dictionary<GameObject, int>();

    void OnTriggerEnter(Collider other) {
        layers[other.gameObject] = other.gameObject.layer;
        other.gameObject.layer = LayerMask.NameToLayer("PortalFloor");
    }

    void OnTriggerExit(Collider other) {
        other.gameObject.layer = layers[other.gameObject];
        layers.Remove(other.gameObject);
    }
}
