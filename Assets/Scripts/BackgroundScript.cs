using UnityEngine;
public class BackgroundScript : MonoBehaviour {
    MeshRenderer mr;
    public GameObject cam;

    private void Start() {
        //transform.localScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, 0);
        mr = GetComponent<MeshRenderer>();
        //mr.sortingLayerName
        mr.sortingOrder = 0;
        cam = GameObject.Find("Main Camera");
    }
    private void Update() {
        Material mat = mr.material;
        Vector2 offset = mat.mainTextureOffset;
        offset = cam.transform.position;
        mat.mainTextureOffset = offset;

    }

}
