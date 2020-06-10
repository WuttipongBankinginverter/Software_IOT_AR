using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;

public class HCI_LAB_CORE_V2 : MonoBehaviour
{
    [SerializeField] string url;

    IEnumerator GetStaticValue(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            var msg = webRequest.downloadHandler.text;
            var useMsg = JObject.Parse(msg);
            var useGuid = Convert.ToString(useMsg["client"]["lamp"]);
            Debug.Log(useGuid);
        }
    }

    IEnumerator GetDynamicValue(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            var msg = webRequest.downloadHandler.text;
            var useMsg = JObject.Parse(msg);
            var useGuid = Convert.ToString(useMsg["params"]);
            Debug.Log(useGuid);
        }
    }
}
