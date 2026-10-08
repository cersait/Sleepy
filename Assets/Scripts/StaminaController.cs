using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina")]
    public float playerStamina = 100f;

    [SerializeField] private float maxStamina = 100f;

    [Range(0, 50)]
    [SerializeField] private float staminaDrain = 0.5f;

    [Range(0, 50)]
    [SerializeField] private float staminaRegen = 0.5f;

    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool weAreSprinting = false;

    [Header("Run Speed")]
    [SerializeField] private int WalkSpeed = 10;
    [SerializeField] private int RunSpeed = 20;

    [Header("UI")]
    [SerializeField] private Image staminaProgressUI;
    [SerializeField] private CanvasGroup sliderCanvasGroup;

    private PlayerMove playerController;
    public bool isCrouch = false;
    [SerializeField] float crouchSpeedMultiplier = .5f;

    private void Start()
    {
        playerController = GetComponent<PlayerMove>();

        // Start with full stamina
        playerStamina = maxStamina;

        // Hide the stamina bar at the beginning
        sliderCanvasGroup.alpha = 0;

        UpdateStaminaBar();
    }

    private void Update()
    {
        if (!weAreSprinting)
        {
            RegenerateStamina();
        }
    }

    private void RegenerateStamina()
    {
        if (playerStamina < maxStamina)
        {
            // Show stamina bar while regenerating
            sliderCanvasGroup.alpha = 1;

            playerStamina += staminaRegen * Time.deltaTime;

            // Prevent stamina from going above max
            playerStamina = Mathf.Clamp(playerStamina, 0f, maxStamina);

            // Update the UI while regenerating
            UpdateStaminaBar();

            if (playerStamina >= maxStamina)
            {
                playerStamina = maxStamina;

                playerController.SetRunSpeed(RunSpeed);

                hasRegenerated = true;

                // Hide the bar once stamina is completely full
                sliderCanvasGroup.alpha = 0;
            }
        }
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            weAreSprinting = true;

            // Drain stamina
            playerStamina -= staminaDrain * Time.deltaTime;

            // Prevent stamina from going below zero
            playerStamina = Mathf.Clamp(playerStamina, 0f, maxStamina);

            // Show stamina bar
            sliderCanvasGroup.alpha = 1;

            // Update UI
            UpdateStaminaBar();

            if (playerStamina <= 0)
            {
                playerStamina = 0;

                hasRegenerated = false;

                playerController.SetRunSpeed(WalkSpeed);

                // Keep the stamina bar visible
                sliderCanvasGroup.alpha = 1;
            }
        }
    }

    public void LockSpeed()
    {
        isCrouch = true;
        playerController.SetRunSpeed(WalkSpeed); // Force revert to default when locked
        playerController.SetMovementSpeedMultiplier(crouchSpeedMultiplier);
        weAreSprinting = false;
    }

    public void UnlockSpeed()
    {
        isCrouch = false;
        playerController.SetRunSpeed(RunSpeed);
        playerController.SetMovementSpeedMultiplier(1f);
        weAreSprinting = true;
    }

    private void UpdateStaminaBar()
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
    }
}