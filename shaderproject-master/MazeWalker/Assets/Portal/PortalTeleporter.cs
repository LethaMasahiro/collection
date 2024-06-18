using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{

    public Transform target;

    public bool canTeleport = true;
    public PortalTeleporter otherTele;

    void OnTriggerEnter(Collider other) {
        if (canTeleport && other.CompareTag("Player")) {
            otherTele.canTeleport = false;

            var player = other.transform.parent;
            var mat = target.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0)) * transform.worldToLocalMatrix;

            var pos = player.position;
            var forward = player.forward;
            var up = player.up;

            pos = mat * new Vector4(pos.x, pos.y, pos.z, 1.0f);
            forward = mat * new Vector4(forward.x, forward.y, forward.z, 0);
            up = mat * new Vector4(up.x, up.y, up.z, 0);

            player.position = pos;
            player.rotation = Quaternion.LookRotation(forward, up);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            canTeleport = true;
        }
    }
}
