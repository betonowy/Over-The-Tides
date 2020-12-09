using UnityEngine;

public class FogOfWarScript : MonoBehaviour {

    public GameObject mFogOfWarPlane;
    public Transform mPlayer;
    public LayerMask mFogLayer;
    public float mRadius = 10f;
    private float mRadiusSpr { get { return mRadius * mRadius;  } }

    private Mesh mMesh;
    private Vector3[] mVertices;
    private Color[] mColors;

    void Start() {
        Initialize();
    }

    void Update() {
        Ray r = new Ray(transform.position, mPlayer.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 1000, mFogLayer, QueryTriggerInteraction.Collide)) {
            for (int i = 0; i < mVertices.Length; i++) {
                Vector3 v = mFogOfWarPlane.transform.TransformPoint(mVertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < mRadiusSpr) {
                    float alpha = Mathf.Min(mColors[i].a, dist / mRadiusSpr);
                    mColors[i].a = alpha;
                }
            }
            UpdateColor();
        }
    }

    void Initialize() {
        mMesh = mFogOfWarPlane.GetComponent<MeshFilter>().mesh;
        mVertices = mMesh.vertices;
        mColors = new Color[mVertices.Length];
        for (int i = 0; i < mColors.Length; i++) {
            mColors[i] = Color.black;
        }
        UpdateColor();
    }

    void UpdateColor() {
        mMesh.colors = mColors;
    }
}
