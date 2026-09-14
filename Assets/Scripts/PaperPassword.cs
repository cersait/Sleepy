using TMPro;
using UnityEngine;

public class PaperPassword : MonoBehaviour
{
    [SerializeField] private TextMeshPro Papr;
    private static string password;

    private void Awake()
    {
        SetPassword();
    }

    public void SetPassword(string newPassword)
    {
        password = newPassword;

        if (Papr != null)
        {
            Papr.text = "Password is: " + password;
        }
    }
    public void SetPassword()
    {
        if (Papr != null)
        {
            Papr.text = "Password is: " + password;
        }
    }

    public string GetPassword()
    {
        return password;
    }
}