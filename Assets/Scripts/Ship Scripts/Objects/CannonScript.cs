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

    public Vector2[] nodes;

    private Vector2 transformedSpawnPoint = new Vector2();
    private Vector2 spawnVelocityVector = new Vector2();
    private float cooldown = 0;

    public Sprite[] animation;
    public float[] animationTiming;
    private float frameTime = 0;
    private int frameIndex = 0;
    private int lastIndex = 0;
    private bool triggerAnimation = false;

    private NodeScript ns;
    private Rigidbody2D cannonRB;
    private SpriteRenderer mySprite;
    private bool reloadPlaying = false;
    private bool reloadHesitate = false;

    // Start is called before the first frame update
    void Start() {
        cannonRB = GetComponent<Rigidbody2D>();
        ns = gameObject.AddComponent<NodeScript>();
        ns.SetParent(gameObject);
        for (int i = 0; i < nodes.Length; i++) {
            ns.CreateRelativeNode(nodes[i]);
        }
        cooldown = cooldownTime;
        mySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        cooldown -= Time.deltaTime * (float)ns.ReadyCrewCount() / (float)nodes.Length;
        reloadHesitate = ns.ReadyCrewCount() == 0;
        if (reloadHesitate && cooldown > 0) {
            GetComponents<AudioSource>()[1].Stop();
        }
        Animate();
        if (cooldown > 0 && cooldown < 0.843 * ns.ReadyCrewCount() / nodes.Length && !reloadPlaying) {
            reloadPlaying = true;
            GetComponents<AudioSource>()[1].Play();
        }
        if (!GetComponents<AudioSource>()[1].isPlaying) {
            reloadPlaying = false;
        }
    }

    void Animate() {
        if (frameIndex != 0 || triggerAnimation) {
            triggerAnimation = false;
            frameTime -= Time.deltaTime;
            if (frameTime < 0) {
                if (++frameIndex >= animation.Length) {
                    frameIndex = 0;
                }
                frameTime = animationTiming[frameIndex];
            }
        }
        if (lastIndex != frameIndex) {
            mySprite.sprite = animation[frameIndex];
            lastIndex = frameIndex;
        }
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
            updateSpawnPointAndVelocity();
            triggerAnimation = true;
            GameObject spawnedBall = Instantiate(ballTemplate, gameObject.transform.position, gameObject.transform.rotation);
            Rigidbody2D ballRigidbody = spawnedBall.GetComponent<Rigidbody2D>();

            ballRigidbody.position += transformedSpawnPoint;
            ballRigidbody.velocity = spawnVelocityVector;

            spawnedBall.SendMessage("SetInitialSpeed", spawnVelocityVector);
            gameObject.GetComponents<AudioSource>()[0].Play();
            cooldown = cooldownTime;
        }
    }
}
