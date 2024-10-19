using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S{get; private set;}
    [Header("Inscribed")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public float maxBullets = 5;

    [Header("Dynamic")] [Range(0,4)] [SerializeField]
    private float _shieldLevel = 1;
    private float currBullets;
    [Tooltip("This field holds a reference to the last triggeriing GameObject")]
    private GameObject lastTriggerGo = null;
    void Awake() {
        if(S == null) {
            S = this;
            currBullets = maxBullets;
        } else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(vAxis*pitchMult,hAxis*rollMult,0);

        if(currBullets > 5) {
            maxBullets = 5;
            currBullets = 5;
        }

        if(Input.GetKeyDown(KeyCode.Space) && currBullets > 0){
            currBullets--;
            TempFire();
        }
    }

    void TempFire() {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * projectileSpeed;

        ProjectileHero projScript = projGO.GetComponent<ProjectileHero>();
        projScript.SetHero(this);
    }

    public void IncreaseBulletCount() {
        if (currBullets < maxBullets) {
            currBullets++;
        }
    }

    void OnTriggerEnter(Collider other) {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        
        if(go == lastTriggerGo) return;
        lastTriggerGo = go;
        Enemy enemy = go.GetComponent<Enemy>();
        if(enemy != null) {
            shieldLevel--;
            Destroy(go);
        } else {
            Debug.LogWarning("Shield trigger hit by non-enemy: " + go.name);
        }
    }
    
    public float shieldLevel{
        get {return(_shieldLevel);}
        private set {
            _shieldLevel = Mathf.Min(value, 4);
            if(value < 0) {
                Destroy(this.gameObject);
                Main.HERO_DIED();
            }
        }
    }
}
