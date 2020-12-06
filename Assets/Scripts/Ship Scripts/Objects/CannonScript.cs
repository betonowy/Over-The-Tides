using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CannonScript : MonoBehaviour {

    public GameObject ballTemplate;

    public float ballSpawnDistance = 0.2f;
    public float ballSpeed = 2;
    public float cooldownTime = 3;
    public float power = 20;
    public CannonAnimation spriteSubobject;

    public Vector2[] nodes;

    private Vector2 transformedSpawnPoint = new Vector2();
    private Vector2 spawnVelocityVector = new Vector2();
    private float cooldown = 0;

    public Sprite readyToFire;
    public GameObject cannonIndicator;
    public Sprite redIndi;
    public Sprite greenIndi;
    private SpriteRenderer indiRenderer;
    private GameObject cnnIndi;

    private NodeScript ns;
    private Rigidbody2D cannonRB;
    private bool reloadPlaying = false;
    private bool reloadHesitate = false;

    private AnimationV1 shootingAnimation;

    // Start is called before the first frame update
    void Start() {
        cannonRB = GetComponent<Rigidbody2D>();
        ns = gameObject.AddComponent<NodeScript>();
        ns.SetParent(gameObject);
        for (int i = 0; i < nodes.Length; i++) {
            ns.CreateRelativeNode(nodes[i]);
        }
        cooldown = cooldownTime;
        GetComponent<SpriteRenderer>().sprite = null; // to change
        indiRenderer = GetComponent<SpriteRenderer>();
        shootingAnimation = GetComponent<AnimationV1>(); // to change
    }

    // Update is called once per frame
    void Update() {
        cooldown -= Time.deltaTime * (float)ns.ReadyCrewCount() / (float)nodes.Length;
        reloadHesitate = ns.ReadyCrewCount() == 0;
        if (reloadHesitate && cooldown > 0) {
            GetComponents<AudioSource>()[1].Stop();
        }
        Animate(); // to change
        if (cooldown > 0 && cooldown < 0.843 * ns.ReadyCrewCount() / nodes.Length && !reloadPlaying) {
            reloadPlaying = true;
            GetComponents<AudioSource>()[1].Play();
        }
        if (cooldown < 0) {
            ChangeIfReady(); // to change
            ChangeIndicatorGreen();
        }
        if (!GetComponents<AudioSource>()[1].isPlaying) {
            reloadPlaying = false;
        }
    }

    void Animate() {
        
    }

    void updateSpawnPointAndVelocity() {
        float rotation = cannonRB.rotation * Mathf.Deg2Rad;

        transformedSpawnPoint.x = -Mathf.Sin(rotation);
        transformedSpawnPoint.y = Mathf.Cos(rotation);

        spawnVelocityVector = transformedSpawnPoint * ballSpeed + cannonRB.velocity;
        transformedSpawnPoint *= ballSpawnDistance;
    }

    public void shot() {
        if (cooldown <= 0) {
            spriteSubobject.AnimateShoot();
            updateSpawnPointAndVelocity();
            GameObject spawnedBall = Instantiate(ballTemplate, gameObject.transform.position, gameObject.transform.rotation);
            Rigidbody2D ballRigidbody = spawnedBall.GetComponent<Rigidbody2D>();

            ballRigidbody.position += transformedSpawnPoint;
            ballRigidbody.velocity = spawnVelocityVector;

            spawnedBall.SendMessage("SetInitialSpeed", spawnVelocityVector);
            gameObject.GetComponents<AudioSource>()[0].Play();
            cooldown = cooldownTime;
            ChangeIndicatorRed();
        }
    }

    public void ChangeIfReady() {
        spriteSubobject.AnimateReady();
    }

    public void CreateIndicator(bool pos)
    {
        cnnIndi = Instantiate(cannonIndicator);
        cnnIndi.transform.SetParent(gameObject.transform);
        cnnIndi.transform.rotation = gameObject.transform.rotation;
        if (pos) {
            cnnIndi.transform.position = gameObject.transform.position + new Vector3((float)-0.2, (float)0.25, 0);
        } else {
            cnnIndi.transform.position = gameObject.transform.position + new Vector3((float)-0.2, (float)-0.25, 0);
        }
    }

    public void ChangeIndicatorRed() {
        cnnIndi.GetComponent<SpriteRenderer>().sprite = redIndi;
    }

    public void ChangeIndicatorGreen()
    {
        cnnIndi.GetComponent<SpriteRenderer>().sprite = greenIndi;
    }
}
