using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCI_LAB_ITEM_BUTTON : MonoBehaviour
{

    HCI_LAB_ITEM_PUBLISH PUB;

    [SerializeField] string Message;
    Renderer Button;
    [SerializeField] Color ActiveColor;
    [SerializeField] Color NormalColor;
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
        PUB.MyPublish(Message);
        MySetState(true);
    }

    private void OnMouseUp()
    {
        Debug.Log(">OnMouseUp() : " + gameObject);
        MySetState(false);
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

            Button.material.color = NormalColor;

        }

    }

    #endregion

}
