using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastScript : MonoBehaviour
{
    private Animator animator;
    private bool isMoving = false;
    private Vector2 zeroo;
    public Vector2[] nodes;
    public SailorScript.SailorType sailorCompatibility = SailorScript.SailorType.SAILOR_DEFAULT;

    NodeScript ns;

    // Start is called before the first frame update
    void Start()
    {
        zeroo = new Vector2(0f, 0f);
        animator = GetComponent<Animator>();
        ns = gameObject.AddComponent<NodeScript>();
        ns.sailorCompatibleType = sailorCompatibility;
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
        if (!(Equals(transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity, zeroo)) && !isMoving) {
            animator.SetBool("isMoving", true);
            isMoving = true;
        } else if (Equals(transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity, zeroo) && isMoving) {
            animator.SetBool("isMoving", false);
            isMoving = false;
        }
    }
}
