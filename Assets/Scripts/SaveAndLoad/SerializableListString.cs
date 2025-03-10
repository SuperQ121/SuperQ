using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SerializableListString<T> : List<T>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<string> _list = new List<string>();

    public void OnBeforeSerialize()
    {
        _list.Clear();
        _list.AddRange(this);
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        this.AddRange(_list);
    }
}