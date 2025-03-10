using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 定义了一个名为 SerializableDictionary 的泛型类，它继承自 Unity 的 Dictionary<TKey, TValue> 类，
/// 并实现了 ISerializationCallbackReceiver 接口，使得该字典可以在 Unity 的序列化过程中被正确地保存和加载。
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>

[System.Serializable]
public class SerializableDictionary< TKey,TValue>:Dictionary<TKey, TValue>,ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach(KeyValuePair<TKey, TValue> pair in this )
        {
            keys.Add( pair.Key );
            values.Add( pair.Value );
        }
    }
    public void OnAfterDeserialize()
    {
        this.Clear();

        if(keys.Count!=values.Count)
        {
            Debug.Log("Keys count is not equal to values count");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i],values[i]);
        }

    }


}
