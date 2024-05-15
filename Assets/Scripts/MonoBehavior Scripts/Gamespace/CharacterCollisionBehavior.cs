using Unity.VisualScripting;
using UnityEngine;

public class CharacterCollisionBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ignore in Collision Calculations")) {
            SendMessageUpwards("OnCollisionHit", true);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Ignore in Collision Calculations")) {
            SendMessageUpwards("OnCollisionHit", false);
        }
    }
}
