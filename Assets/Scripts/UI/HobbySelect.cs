using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class HobbySelect : MonoBehaviour
{
    [SerializeField]
    private List<Button> buttons = new List<Button>();
    //[SerializeField]
    //private Button backButton;
    [SerializeField]
    private ButtonLoader submitButton;
    [SerializeField]
    private UnityEvent goNextStep;
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color toggledColor = Color.green;

    private Dictionary<string, string> buttonToHobbyMap = new Dictionary<string, string>
    {
        { "GamingButton", "Gaming" },
        { "ReadingButton", "Reading" },
        { "HackingButton", "Hacking" },
        { "CookingButton", "Cooking" },
        { "CosplayButton", "Cosplaying" },
        { "ArtsCraftButton", "Arts & Craft" },
        { "PhotographyButton", "Photography" },
        { "BoardGamesButton", "Board Games" },
        { "RubicksCubeButton", "Rubicks Cube" },
        { "GardeningButton", "Gardening" },
        { "MuseumButton", "Visiting Museums" }
    };

    //Hobby Selections
    private Dictionary<string, int> hobbies = new Dictionary<string, int>();

    void Start()
    {
        foreach (var hobby in buttonToHobbyMap.Values)
        {
            hobbies[hobby] = 0;
        }

        InitializeButtons();

        if (submitButton != null)
        {
            submitButton.button.onClick.AddListener(SubmitSelection);
        }
    }

    void InitializeButtons()
    {
        foreach (Button button in buttons)
        {
            if (buttonToHobbyMap.TryGetValue(button.name, out string hobbyName))
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = hobbyName;
                }

            button.TryGetComponent<Image>(out Image img);
            img.color = normalColor;
            button.onClick.AddListener(() => ToggleButton(button, img));
        }
    }

    void ToggleButton(Button button, Image img)
    {
        bool isToggled = !img.color.Equals(toggledColor);
        img.color = isToggled ? toggledColor : normalColor;

        if (buttonToHobbyMap.TryGetValue(button.name, out string hobby))
        {
            hobbies[hobby] = isToggled ? 1 : 0;
        }
    }
    }

    private void OnSubmissionComplete(bool success)
    {
        //backButton.interactable = true;
        submitButton.SetLoading(false);

        //if (success)
            //goNextStep?.Invoke();
    }

    public void SubmitSelection()
    {
        List<string> selectedHobbies = new List<string>();

        foreach (var hobby in hobbies)
        {
            if (hobby.Value == 1)
            {
                selectedHobbies.Add(hobby.Key);
            }
        }

        Debug.Log("Selected Hobbies: " + string.Join(", ", selectedHobbies));
        CloudScriptManager.Instance.ExecSubmitApplication(string.Join(", ", selectedHobbies), _ => OnSubmissionComplete(true), _ => OnSubmissionComplete(false));
    }
}