using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class HCI_LAB_CORE : MonoBehaviour
{

    [SerializeField] string MqttBrokerAddress = "localhost";
    [SerializeField] int MqttBrokerPort = 1883;
    [SerializeField] string AccessToken;

    MqttClient CLIENT;
    Queue<MyMqttMsg> LIST_MQTT_MSG_QUEUE;
    HCI_LAB_ITEM_SUBSCRIBE[] LIST_ITEM_SUBSCRIBE;


    private void Awake()
    {

        try
        {
            CLIENT = new MqttClient(MqttBrokerAddress);
        }
        catch (System.Exception)
        {

            CLIENT = new MqttClient(IPAddress.Parse(MqttBrokerAddress), MqttBrokerPort, false, null);

        }

        LIST_MQTT_MSG_QUEUE = new Queue<MyMqttMsg>();

    }

    void Start()
    {

        LIST_ITEM_SUBSCRIBE = GameObject.FindObjectsOfType<HCI_LAB_ITEM_SUBSCRIBE>();

        StartCoroutine(MyStartMqtt());

    }

    private void CLIENT_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {

        var topic = e.Topic;
        var msg = System.Text.Encoding.UTF8.GetString(e.Message);

        Debug.Log(">Receive : topic=" + topic + ", message=" + msg);

        LIST_MQTT_MSG_QUEUE.Enqueue(new MyMqttMsg(topic, msg));

    }

    // Update is called once per frame
    void Update()
    {

        if (LIST_MQTT_MSG_QUEUE != null && LIST_MQTT_MSG_QUEUE.Count > 0)
        {

            var data = LIST_MQTT_MSG_QUEUE.Dequeue();

            foreach (var item in LIST_ITEM_SUBSCRIBE)
            {

                if (data.TOPIC == item.SubscriptionTopic)
                {

                    item.MyReceive(data.MESSAGE);

                }

            }

        }

    }

    #region event

    IEnumerator MyStartMqtt()
    {

        yield return new WaitForEndOfFrame();

        MyLog("MQTT Connecting ...");
        CLIENT.Connect("AR-Dashboard-",AccessToken,"",true,60);

        if (CLIENT.IsConnected)
        {

            MyLog("MQTT Connected");

            CLIENT.MqttMsgPublishReceived += CLIENT_MqttMsgPublishReceived;
    
            foreach (var item in LIST_ITEM_SUBSCRIBE)
            {

                if (item.SubscriptionTopic.Trim().Length > 0)
                {

                    Debug.Log(">MyStartMqtt() : Subscribe " + item.SubscriptionTopic);
                    CLIENT.Subscribe(new string[] { item.SubscriptionTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }

            }

        }
        else
        {

            MyLog("MQTT Connection fail !");

        }

    }

    #endregion

    #region helper

    public void MyPublish(string topic, string msg)
    {

        Debug.Log(">MyPublish(" + topic + ", " + msg + ")");
        CLIENT.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg));

    }

    void MyLog(string msg)
    {

        Debug.Log(msg);

    }

    #endregion

}

public class MyMqttMsg
{

    public string TOPIC { get; set; }
    public string MESSAGE { get; set; }

    public MyMqttMsg(string topic, string msg)
    {

        TOPIC = topic;
        MESSAGE = msg;

    }

}