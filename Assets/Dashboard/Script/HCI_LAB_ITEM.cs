using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCI_LAB_ITEM : MonoBehaviour
{

    public delegate void MyEventStr(string msg);
    public event MyEventStr OnReceive;

    [HideInInspector]
    public HCI_LAB_CORE CORE;

    public string SubscriptionTopic;
    public string PublishTopic;

    private void Awake()
    {

        CORE = GameObject.FindObjectOfType<HCI_LAB_CORE>();

    }

    public void MyReceive(string msg)
    {

        OnReceive(msg);

    }

    public void MyPublish(string msg)
    {

        CORE.MyPublish(PublishTopic, msg);

    }

}
