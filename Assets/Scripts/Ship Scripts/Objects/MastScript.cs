using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastScript : MonoBehaviour
{
    public Vector2[] nodes;

    NodeScript ns;

    // Start is called before the first frame update
    void Start()
    {
        ns = gameObject.AddComponent<NodeScript>();
        ns.SetParent(gameObject);
        for (int i = 0; i < nodes.Length; i++) {
            ns.CreateRelativeNode(nodes[i]);
        }
    }

    public NodeScript getNodes() {
        return ns;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
