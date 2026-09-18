using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Locker : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject Locked;
    [SerializeField] private GameObject Locked2;
    [SerializeField] private Transform player;
    [SerializeField] private Transform Lockdoor;

    [SerializeField] private EnemyController enemyController;

    private bool insideLocker = false;

    [SerializeField] float Loseplayer = 1.5f;   
    [SerializeField] float Keepplayer = 3f;
    private void Update()
    {
        // If player is inside the locker and enemy starts attacking
        if (insideLocker && enemyController != null)
        {
            if (enemyController._isAttacking)
            {
                player.transform.position = Locked2.transform.position;
            }
        }
    }
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
            player.transform.Rotate(0, 180, 0);

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

            if (controller != null)
                controller.enabled = true;

            insideLocker = false;
        }

        // Toggle player movement
        if (playerMove != null)
            playerMove.enabled = !playerMove.enabled;

        if (enemyController != null)
        {
            if (insideLocker)
            {
                enemyController.SetLosePlayerTime(Loseplayer);
            }
            else
            {
                enemyController.SetLosePlayerTime(Keepplayer);
            }
        }
    }
}