using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyData
{
    public KeyCode Gool = KeyCode.LeftShift;
    public KeyCode guardKey = KeyCode.Mouse1;
    public KeyCode Fire = KeyCode.Mouse0;
}
public static class DataManager
{
    public static KeyData keyData = new KeyData();
}
