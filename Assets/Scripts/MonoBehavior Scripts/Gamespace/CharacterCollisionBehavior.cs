using Unity.VisualScripting;
using UnityEngine;

public class CharacterCollisionBehavior : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC")) {
            SendMessageUpwards("TriggerHit", other.gameObject);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC")) {
            SendMessageUpwards("TriggerExit", other.gameObject);
        }
    }
}
