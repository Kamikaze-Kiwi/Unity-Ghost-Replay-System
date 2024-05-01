using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording
{
    public string id;
    public float time;
    public List<TransformState> transformStates = new();
}
