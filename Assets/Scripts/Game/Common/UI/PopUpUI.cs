using TMPro;
using UnityEngine;

public class PopUpUI : MonoBehaviour
{

    public delegate void ClosedEvent();
    
    public delegate void ButtonPressedEvent(int buttonID);
    
    public ClosedEvent OnClose;

    public ButtonPressedEvent OnButtonPressed;

    public GameObject ButtonPrefabs;
    
    public TextMeshProUGUI title, description;

    public GameObject closeButton;

    public GameObject[] buttons;
    public bool IsClosable 
    {
        set
        {
            if (!value)
            {
                if (buttons == null || buttons.Length == 0)
                {
                    Debug.LogError("Can not set IsClosable on pop up if no other buttons were made to close it");
                    return;
                }
                closeButton.SetActive(false);
            }
            else
            {
                closeButton.SetActive(true);
            }
        }
    }
    public void SetTitle(string title)
    {
        this.title.text = title;
    }

    public void SetDescription(string description)
    {
        this.description.text = description;
    }

    public void SetButtons(string[] ButtonNames)
    {
        //Create all the required buttons
    }

    public void Close()
    {
        
        OnClose = null;
        OnButtonPressed = null;
        Destroy(this.gameObject);
    }
}
