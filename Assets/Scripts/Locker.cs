using UnityEngine;

public class Locker : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject Locked;
    [SerializeField] private GameObject Locked2;
    [SerializeField] private Transform player;
    [SerializeField] private Transform Lockdoor;

    private bool insideLocker = false;

    public void Interact()
    {
        if (Locked == null || Locked2 == null || player == null)
            return;

        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        CharacterController controller = player.GetComponent<CharacterController>();

        // Toggle where the player goes
        if (!insideLocker)
        {
            // Go inside
            if (controller != null)
                controller.enabled = false;

            player.transform.position = Locked.transform.position;
            player.transform.LookAt(Lockdoor);

            if (controller != null)
                controller.enabled = true;

            insideLocker = true;
        }
        else
        {
            // Go outside
            if (controller != null)
                controller.enabled = false;

            player.transform.position = Locked2.transform.position;
            player.transform.LookAt(Lockdoor);

            if (controller != null)
                controller.enabled = true;

            insideLocker = false;
        }

        // Toggle player movement
        if (playerMove != null)
            playerMove.enabled = !playerMove.enabled;
    }
}