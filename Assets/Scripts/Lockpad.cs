using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Lockpad : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform player;

    [SerializeField] private TextMeshProUGUI Ans;
    [SerializeField] private Animator Door;
    [SerializeField] private GameObject LockPanel;

    [SerializeField] private int passwordLength = 4;

    private string Answer;
    private PaperPassword Paper;

    private void Start()
    {
        Paper = GetComponentInChildren<PaperPassword>();
        GenerateRandomPassword();
    }
    private void GenerateRandomPassword()
    {
        Answer = "";

        for (int i = 0; i < passwordLength; i++)
        {
            Answer += Random.Range(1, 9).ToString();
        }

        Debug.Log("Keypad password for this run: " + Answer);

        if (Paper != null)
        {
            Paper.SetPassword(Answer);
        }

    }

    public void OnPaper(TextMeshPro paperText)
    {
        if (paperText == null)
        {
            Debug.LogError("Lockpad: Paper TextMeshPro is not assigned!");
            return;
        }

        paperText.text = "Password is: " + Answer;
    }

    public void Interact()
    {
        if (LockPanel == null) return;
        Interactor interactor = FindFirstObjectByType<Interactor>();
        
        if (LockPanel != null)
        {
            bool isActive = LockPanel.activeSelf;
            LockPanel.SetActive(!LockPanel.activeInHierarchy);

            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
            Look.instance.updatingRotation = !Look.instance.updatingRotation;

            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            playerMove.enabled = !playerMove.enabled;
            Ans.text = "";
        }
    }

    public void Number(int number)
    {
        if (Ans.text.Length >= passwordLength) return;
        Ans.text += number.ToString();
    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "Correct";

            Door.SetBool("Open", true);

            StartCoroutine(StopDoor());

            // Optional: prevent the keypad from being used again
            //LockPanel.SetActive(false);
        }
        else
        {
            Ans.text = "Invalid";

            // Wait a moment, then clear the invalid message
            StartCoroutine(ClearInvalid());
        }
    }

    private IEnumerator ClearInvalid()
    {
        yield return new WaitForSeconds(0.75f);

        Ans.text = "";
    }

    IEnumerator StopDoor()
    {
        yield return new WaitForSeconds(0.5f);
        Door.SetBool("Open", false);
        Door.enabled = false;
    }

    public string GetPassword()
    {
        return Answer;
    }
}
