using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCI_LAB_ITEM_GAUGE : MonoBehaviour
{

    [SerializeField] int MinValue;
    [SerializeField] int MaxValue;

    HCI_LAB_ITEM_SUBSCRIBE SUB;

    UnityEngine.TextMesh TEXT;
    LineRenderer[] LINE;

    private void Awake()
    {

        SUB = GetComponent<HCI_LAB_ITEM_SUBSCRIBE>();
        LINE = GetComponentsInChildren<LineRenderer>();

        SUB.OnReceive += ITEM_OnReceive;

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

        MySetGauge("");
        TEXT.text = "";

    }

    #region event

    private void ITEM_OnReceive(string msg)
    {

        MySetGauge(msg);

    }

    #endregion

    #region helper

    void MySetGauge(string value)
    {

        float radius = 0.025f - 0.005f;
        int section = 32;

        TEXT.text = value.ToString();

        #region draw BG

        LINE[0].positionCount = section + 1;

        for (int i = 0; i < LINE[0].positionCount; i++)
        {

            float deg = ((float)i / section) * 360f;
            deg = -deg - 90;
            float px = radius * Mathf.Cos(deg * Mathf.Deg2Rad);
            float py = radius * Mathf.Sin(deg * Mathf.Deg2Rad);
            LINE[0].SetPosition(i, new Vector3(px, py, -0.0051f));

        }

        #endregion

        #region draw DATA

        int ivalue;
        float fvalue;

        if (int.TryParse(value, out ivalue))
        {

        }
        else if (float.TryParse(value, out fvalue))
        {

            ivalue = Mathf.RoundToInt(fvalue);

        }
        else
        {

            return;

        }

        var normal = Mathf.InverseLerp(MinValue, MaxValue, ivalue);
        var res = Mathf.RoundToInt(Mathf.Lerp(0, section, normal));
        LINE[1].positionCount = res + 1;

        for (int i = 0; i < LINE[1].positionCount; i++)
        {

            float deg = ((float)i / section) * 360f;
            deg = -deg - 90;
            float px = radius * Mathf.Cos(deg * Mathf.Deg2Rad);
            float py = radius * Mathf.Sin(deg * Mathf.Deg2Rad);
            LINE[1].SetPosition(i, new Vector3(px, py, -0.0052f));

        }

        #endregion

    }

    #endregion

}
