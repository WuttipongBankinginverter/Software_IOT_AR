using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCI_LAB_ITEM_TEXT : MonoBehaviour
{

    HCI_LAB_ITEM_SUBSCRIBE SUB;

    Renderer BG;

    [SerializeField] string ActiveMessage;
    [SerializeField] Color ActiveColor;
    [SerializeField] Color NormalColor;

    UnityEngine.TextMesh TEXT;

    private void Awake()
    {

        SUB = GetComponent<HCI_LAB_ITEM_SUBSCRIBE>();
        BG = transform.GetChild(2).GetComponent<Renderer>();

        SUB.OnReceive += ITEM_OnReceive;

        MySetBackground(false);

    }

    private void Start()
    {

        var tmps = GetComponentsInChildren<TextMesh>();

        foreach (var item in tmps)
        {

            if (item.gameObject.name == "text")
            {

                TEXT = item;
                break;

            }

        }

        TEXT.text = "";

    }

    #region event

    private void ITEM_OnReceive(string msg)
    {

        TEXT.text = msg;

        if (ActiveMessage.Trim().Length > 0 && ActiveMessage == msg)
        {

            MySetBackground(true);

        }
        else
        {

            MySetBackground(false);

        }

    }

    #endregion

    #region helper

    void MySetBackground(bool active)
    {

        if (active)
        {

            BG.material.color = ActiveColor;

        }
        else
        {

            BG.material.color = NormalColor;

        }

    }

    #endregion

}
