using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] //데이터를 인스펙터 창에 노출

public class Data : MonoBehaviour
{
    public double coin;
    public bool[] stageUnlock = new bool[3];
}
