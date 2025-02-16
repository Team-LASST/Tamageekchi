using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpertiseSelect : MonoBehaviour
{
    public Button buttonSoftware;
    public Button buttonHardware;
    public Button buttonDesigner;
    [SerializeField]
    private ButtonLoader submitButton;  // Assuming ButtonLoader contains a Button

    private Dictionary<string, int> expertiseSelection = new Dictionary<string, int>();
    private Dictionary<Button, string> buttonExpertiseMap = new Dictionary<Button, string>();
    private Dictionary<Button, Color> buttonDefaultColors = new Dictionary<Button, Color>();
    private Color selectedColor = new Color(0.5f, 0f, 0.5f, 0.3f); // Purple
    private Button selectedButton = null;

    void Start()
    {
        // Assuming ButtonLoader contains a reference to a Button component
        if (submitButton != null)
        {
            // Assuming ButtonLoader has a "button" field or property that is a Button
            submitButton.button.interactable = false; // Disable the button initially
        }

        buttonExpertiseMap[buttonSoftware] = "Software Engineer";
        buttonExpertiseMap[buttonHardware] = "Hardware Engineer";
        buttonExpertiseMap[buttonDesigner] = "Designer";

        // Initializing expertise selection
        expertiseSelection["Software Engineer"] = 0;
        expertiseSelection["Hardware Engineer"] = 0;
        expertiseSelection["Designer"] = 0;

        // Storing default button colors
        buttonDefaultColors[buttonSoftware] = buttonSoftware.image.color;
        buttonDefaultColors[buttonHardware] = buttonHardware.image.color;
        buttonDefaultColors[buttonDesigner] = buttonDesigner.image.color;

        // Adding listeners to button clicks
        buttonSoftware.onClick.AddListener(() => SelectExpertise(buttonSoftware));
        buttonHardware.onClick.AddListener(() => SelectExpertise(buttonHardware));
        buttonDesigner.onClick.AddListener(() => SelectExpertise(buttonDesigner));

        if (submitButton != null)
        {
            submitButton.button.onClick.AddListener(SubmitSelection);
        }
    }

    void SelectExpertise(Button clickedButton)
    {
        // If the same button is clicked again, toggle the selection off
        if (selectedButton == clickedButton)
        {
            // Reset the color and selection
            clickedButton.image.color = buttonDefaultColors[clickedButton];
            expertiseSelection[buttonExpertiseMap[clickedButton]] = 0;
            selectedButton = null;

            // Disable the submit button if no selection is made
            if (submitButton != null)
            {
                submitButton.button.interactable = false;
            }
        }
        else
        {
            // Reset the previously selected button
            if (selectedButton != null)
            {
                selectedButton.image.color = buttonDefaultColors[selectedButton];
                expertiseSelection[buttonExpertiseMap[selectedButton]] = 0;
            }

            // Set the new selected button's color to purple
            selectedButton = clickedButton;
            selectedButton.image.color = selectedColor;
            expertiseSelection[buttonExpertiseMap[selectedButton]] = 1;

            // Enable the submit button once a selection is made
            if (submitButton != null)
            {
                submitButton.button.interactable = true;
            }
        }
    }

    public void SubmitSelection()
    {
        string selectedExpertise = "";
        foreach (var expertise in expertiseSelection)
        {
            if (expertise.Value == 1)
            {
                selectedExpertise = expertise.Key;
                break;
            }
        }

        if (string.IsNullOrEmpty(selectedExpertise))
        {
            Debug.Log("No expertise selected.");
            return;
        }

        Debug.Log("Selected Expertise: " + selectedExpertise);
        CloudScriptManager.Instance.ExecSubmitExpertise(selectedExpertise, _ => OnSubmissionComplete(true), _ => OnSubmissionComplete(false));
    }

    private void OnSubmissionComplete(bool success)
    {
        if (submitButton != null)
        {
            submitButton.SetLoading(false);
        }
    }
}
