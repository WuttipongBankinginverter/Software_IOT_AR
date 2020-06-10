using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public void UpdateTemp(float temp)
    {
        transform.localScale = new Vector3(temp, 1f, 1f);
    }
}
