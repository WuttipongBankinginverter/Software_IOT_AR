using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCI_LAB_ITEM_TOGGLE_BUTTON : MonoBehaviour
{

    HCI_LAB_ITEM_PUBLISH PUB;
    
    [SerializeField] string ActiveMessage;
    [SerializeField] string InactiveMessage;
    Renderer Button;
    [SerializeField] Color ActiveColor;
    [SerializeField] Color InactiveColor;
    bool STATE;

    private void Awake()
    {

        PUB = GetComponent<HCI_LAB_ITEM_PUBLISH>();
        
    }

    private void Start()
    {

        Button = transform.GetChild(2).GetComponent<Renderer>();

        MySetState(false);

    }

    #region event

    private void OnMouseDown()
    {

        Debug.Log(">OnMouseDown() : " + gameObject);

        MySetState(!STATE);

        PUB.MyPublish((STATE ? ActiveMessage : InactiveMessage));

    }

    #endregion

    #region helper

    void MySetState(bool active)
    {

        STATE = active;

        if (STATE)
        {

            Button.material.color = ActiveColor;

        }
        else
        {

            Button.material.color = InactiveColor;

        }

    }

    #endregion

}
