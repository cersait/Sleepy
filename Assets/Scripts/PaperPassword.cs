using TMPro;
using UnityEngine;

public class PaperPassword : MonoBehaviour
{
    [SerializeField] private TextMeshPro Papr;

    private string password;

    public void SetPassword(string newPassword)
    {
        password = newPassword;

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