using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Text.RegularExpressions;

public class SignUpManager : MonoBehaviour
{
    [SerializeField] PopupManager popupManager;
    [SerializeField] SplashManager splashManager;
    [Header("Fields")]
    [SerializeField] TMP_InputField field_firstname;
    [SerializeField] TMP_InputField field_lastname, field_email, field_pw, field_confirmpw;
    [Header("Errors")]
    [SerializeField] TMP_Text error_name;
    [SerializeField] TMP_Text error_email, error_pw, error_confirmpw;
    [Header("Buttons")]
    [SerializeField] ButtonLoader button_register;
    [SerializeField] Button button_showpw, button_showconfirmpw;

    private void OnDisable()
    {
        if (field_firstname == null)
            return;

        // Reset fields on disable
        field_firstname.text = field_lastname.text = field_email.text = field_pw.text = field_confirmpw.text = "";

        // Hide errors
        error_name.gameObject.SetActive(false);
        error_email.gameObject.SetActive(false);
        error_pw.gameObject.SetActive(false);
        error_confirmpw.gameObject.SetActive(false);
    }

    private bool ValidateFields()
    {
        error_name.gameObject.SetActive(false);
        error_email.gameObject.SetActive(false);
        error_pw.gameObject.SetActive(false);
        error_confirmpw.gameObject.SetActive(false);

        if (field_firstname.text.Trim().Length == 0 || field_lastname.text.Trim().Length == 0)
        {
            error_name.gameObject.SetActive(true);
            return false;
        }

        if (!Regex.IsMatch(field_email.text, splashManager.EmailRegex))
        {
            error_email.gameObject.SetActive(true);
            return false;
        }

        if (field_pw.text.Length < 6)
        {
            error_pw.gameObject.SetActive(true);
            return false;
        }

        if (field_pw.text != field_confirmpw.text)
        {
            error_confirmpw.gameObject.SetActive(true);
            return false;
        }

        return true;
    }

    /*
     * Button Events
     */
    public void OnButtonRegUser()
    {
        button_register.SetLoading(true);

        // Validation
        if (!ValidateFields())
        {
            button_register.SetLoading(false);
            return;
        }

        var regReq = new RegisterPlayFabUserRequest // for button click
        {
            Email = field_email.text,
            Password = field_pw.text,
            DisplayName = $"{field_firstname.text} {field_lastname.text}",
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(regReq, OnRegSucc, e => OnError("Error registering user!", e));
    }

    public void OnButtonShowPassword()
    {
        splashManager.OnButtonShowPassword(button_showpw, field_pw);
        splashManager.OnButtonShowPassword(button_showconfirmpw, field_confirmpw);
    }

    /*
     * Responses
     */
    void OnRegSucc(RegisterPlayFabUserResult r)
    {
        popupManager.ShowMessage("Success!", "Please login with your new account.", "OK", false, () => splashManager.OnSwitchPanel("Login"), () => splashManager.OnSwitchPanel("Login"));
        CloudScriptManager.Instance.ExecBasicCoudScriptFunction(CloudScriptType.OnUserRegister, null, e => OnError("Error assigning default Geeklings", e));
    }

    void OnError(string errorTitle, PlayFabError e)
    {
        if (e.Error.ToString() == "UsernameNotAvailable")
        {
            popupManager.ShowMessage("Oops!", "You already have an account under this email.", "OK", false);
        }
        else
        {
            string errorMessage = e.ErrorMessage;

            if (e.ErrorDetails != null)
                foreach (var pair in e.ErrorDetails)
                    foreach (var msg in pair.Value)
                        errorMessage += "\n" + msg;

            popupManager.ShowMessage(errorTitle, errorMessage, "OK", false);
        }

        Debug.LogError(e.GenerateErrorReport());
        Debug.LogError("Error Type: " + e.Error.ToString());
        Debug.LogError("Error Code: " + e.Error.GetTypeCode().ToString());

        button_register.SetLoading(false);
    }
}
