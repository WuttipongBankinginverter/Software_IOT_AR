using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCI_LAB_ITEM_PUBLISH : MonoBehaviour
{

    [HideInInspector]
    public HCI_LAB_CORE CORE;

    public string PublishTopic;

    private void Awake()
    {
        CORE = GameObject.FindObjectOfType<HCI_LAB_CORE>();
    }

    public void MyPublish(string msg)
    {

        CORE.MyPublish(PublishTopic, msg);

    }
    

}
