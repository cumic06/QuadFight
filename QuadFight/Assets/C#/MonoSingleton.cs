using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                GameObject container = new GameObject();
                instance = container.AddComponent<T>();
            }
            return instance;
        }
    }
}