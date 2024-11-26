using System.Collections;
using System.Collections.Generic;
using DS;
using UnityEngine;

public class DsMono : MonoBehaviour
{
    // Start is called before the first frame update
    public int LENGTH = 1000_000_000;
    public int Circle = 10000;
    [Range(0, 1)]
    public float InsertIndex = 0.5f;

    void Start()
    {
        BlockLinkedList<string> blockLinkedList = new BlockLinkedList<string>((int)Mathf.Sqrt(LENGTH));
        string[] array = new string[LENGTH];
        List<string> list = new List<string>();
        LinkedList<string> linkedList = new LinkedList<string>();

        for (int i = 0; i < LENGTH; i++)
        {
            var v = Random.Range(0, 100).ToString();
            blockLinkedList.Add(v);
            array[i] = v;
            list.Add(v);
            linkedList.AddLast(v);
        }

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        var index = (int)(LENGTH * InsertIndex);
        if (index == LENGTH)
            index--;

        //插入操作
        stopwatch.Start();
        for (int i = 0; i < Circle; i++)
        {
            var value = Random.Range(0, 100).ToString();
            blockLinkedList.Insert(index, value);
        }
        stopwatch.Stop();
        Debug.Log($"BlockLinkedList Insert: {stopwatch.ElapsedMilliseconds}ms");

        stopwatch.Restart();
        for (int i = 0; i < Circle; i++)
        {
            var value = Random.Range(0, 100).ToString();
            array[index] = value;
            for (int j = LENGTH - 1; j > index; j--)
            {
                array[j] = array[j - 1];
            }
        }
        stopwatch.Stop();
        Debug.Log($"Array Insert: {stopwatch.ElapsedMilliseconds}ms");

        stopwatch.Restart();
        for (int i = 0; i < Circle; i++)
        {
            var value = Random.Range(0, 100).ToString();
            list.Insert(index, value);
        }
        stopwatch.Stop();
        Debug.Log($"List Insert: {stopwatch.ElapsedMilliseconds}ms");

        stopwatch.Restart();
        for (int i = 0; i < Circle; i++)
        {
            var value = Random.Range(0, 100).ToString();

            var node = linkedList.First;
            for (int j = 0; j < index; j++)
            {
                node = node.Next;
            }
            linkedList.AddAfter(node, value);
        }
        stopwatch.Stop();
        Debug.Log($"LinkedList Insert: {stopwatch.ElapsedMilliseconds}ms");
    }
}
