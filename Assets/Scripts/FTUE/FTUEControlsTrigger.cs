using UnityEngine;

public class FTUEControlsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            GameplayEvents.OnPlayerInsideFTUETrigger(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            GameplayEvents.OnPlayerInsideFTUETrigger(false);
        }
    }
}
