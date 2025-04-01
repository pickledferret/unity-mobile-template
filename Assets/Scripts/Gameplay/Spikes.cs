using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.DisableMovement();
            GameManager.Instance.FailLevel();
        }
    }
}
