using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : MonoBehaviour
{

    public delegate void ClosedEvent();
    
    public delegate void ButtonPressedEvent(int buttonID);
    
    public ClosedEvent OnClose;

    public ButtonPressedEvent OnButtonPressed;

    public bool isClosable = true;


    public void SetButtons(string[] ButtonNames)
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
