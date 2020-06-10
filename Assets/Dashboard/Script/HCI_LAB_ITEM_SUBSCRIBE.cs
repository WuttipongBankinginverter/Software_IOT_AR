using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCI_LAB_ITEM_SUBSCRIBE : MonoBehaviour
{

    public delegate void MyEventStr(string msg);
    public event MyEventStr OnReceive;

    public string SubscriptionTopic;
    
    public void MyReceive(string msg)
    {

        OnReceive(msg);

    }

}
