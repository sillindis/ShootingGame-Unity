using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] //�����͸� �ν����� â�� ����
public class Data
{
    public double coin;
    public int level;
    public int damage;

    public bool[] stageUnlock = new bool[3];
    public double[] stageScore = new double[3];

    public float musicVolume;
    public bool musicOn;

}
