using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    Transform[] childNodes;
    //Created a dynamic array to store the nodes//
    public List<Transform> childNodeList = new List<Transform>();

    void Start()
    {
        FillNodes();

    }
    //will draw out a path between the nodes//
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        FillNodes();

        for(int i=0; i < childNodeList.Count; i++)
        {
            Vector3 pos = childNodeList[i].position;

            if (i > 0)
            {
                Vector3 prev = childNodeList[i-1].position;
                Gizmos.DrawLine(prev, pos);
            }
        }
    }
     
    void FillNodes()
    {
        //Empties the list//
        childNodeList.Clear();

        childNodes = GetComponentsInChildren<Transform>();

        foreach (Transform child in childNodes)
        {
            if(child != this.transform)
            {
                childNodeList.Add(child);
            }
        }
    }

    //for the stone script//
    public int RequestPosition(Transform nodeTransform)
    {
        return childNodeList.IndexOf(nodeTransform);
    }
}
