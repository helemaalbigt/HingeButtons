using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public InteractionDepth depth;
}

public enum InteractionDepth
{
    proximity,
    action,
}
