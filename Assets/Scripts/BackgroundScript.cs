using UnityEngine;
public class BackgroundScript : MonoBehaviour {
    MeshRenderer mr;

    private void Start() {
        //transform.localScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, 0);
        mr = GetComponent<MeshRenderer>();
        //mr.sortingLayerName
        mr.sortingOrder = 0;
    }
    private void Update() {
        
  
        Material mat = mr.material;
        Vector2 offset = mat.mainTextureOffset;
        offset.y += Time.deltaTime / 2;
        mat.mainTextureOffset = offset;

    }

}
