using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpertiseSelect : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    
    private Button selectedButton;
    private Color defaultColor;
    private Color selectedColor = new Color(0.5f, 0f, 0.5f); // Purple

    void Start()
    {
        defaultColor = button1.image.color;
        
        button1.onClick.AddListener(() => SelectButton(button1));
        button2.onClick.AddListener(() => SelectButton(button2));
        button3.onClick.AddListener(() => SelectButton(button3));
    }

    void SelectButton(Button clickedButton)
    {
        if (selectedButton != null)
        {
            selectedButton.image.color = defaultColor;
        }

        selectedButton = clickedButton;
        selectedButton.image.color = selectedColor;
    }
}
