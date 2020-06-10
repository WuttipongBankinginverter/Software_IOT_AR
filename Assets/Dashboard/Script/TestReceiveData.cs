using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using Newtonsoft.Json.Linq;
using System;

public class TestReceiveData : MonoBehaviour
{
    HCI_LAB_ITEM_PUBLISH PUB;
    bool state = true;
    Temp temperature;
    float a;
    public string Message;

    private void Start()
    {
        temperature = GetComponent<Temp>();
        PUB = GetComponent<HCI_LAB_ITEM_PUBLISH>();
    }

    void Update()
    {
        StartCoroutine(GetDynamicValue("http://localhost:8080/api/v1/downlight/rpc"));
        //StartCoroutine(GetStaticValue("http://localhost:8080/api/v1/lamp1/rpc"));
        if (a > 50f && state == true)
        {
            PUB.MyPublish(Message);
            state = false;
        }
        if (a < 50f && state == false)
        {
            PUB.MyPublish("0");
            state = true;
        }
    }

    IEnumerator GetStaticValue(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            var msg = webRequest.downloadHandler.text;
            var useMsg = JObject.Parse(msg);
            //var useGuid = Convert.ToString(useMsg["client"]["lamp"]);
            var useGuid = Convert.ToString(useMsg["params"]);
            if (useGuid == "True")
            {
                PUB.MyPublish(Message);
            }
            if (useGuid == "False")
            {
                PUB.MyPublish("0");
            }
            //device = JsonUtility.FromJson<MyDevice>(webRequest.downloadHandler.text);
            //msg = JsonUtility.ToJson(device);

            Debug.Log(useGuid);
        }
    }

    IEnumerator GetDynamicValue(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            var msg = webRequest.downloadHandler.text;
            var useMsg = JObject.Parse(msg);
            var useGuid = Convert.ToString(useMsg["params"]);
            a = float.Parse(useGuid);
            //temperature.UpdateTemp(a);

            Debug.Log(useGuid);
        }
    }

}

