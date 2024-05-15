using UnityEngine;

public class CharacterCollisionBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("NPC")) {
            SendMessageUpwards("OnCollisionHit", true);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("NPC")) {
            SendMessageUpwards("OnCollisionHit", false);
        }
    }
}
