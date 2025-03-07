using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 2;
    public int score = 100;
    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;   
    void Awake(){
        bndCheck = GetComponent<BoundsCheck>();
    }
    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        if(bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown)){
            Destroy(gameObject);
        }
    }

    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed*Time.deltaTime;
        pos = tempPos;
    }

    // void OnCollisionEnter(Collision coll) {
    //     GameObject otherGO = coll.gameObject;
    //     if(otherGO.GetComponent<ProjectileHero>() != null) {
    //         Destroy(otherGO);
    //         Destroy(gameObject);
    //     } else {
    //         Debug.Log("Enemy hit by non ProjectileHero: " + otherGO.name);
    //     }
    // }

    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if(p != null) {
            health -= 2;
            if(health <= 0) {
                if(!calledShipDestroyed) {
                    calledShipDestroyed = true;
                    // Main.SHIP_DESTROYED(this);
                }
                Destroy(this.gameObject); // Destroy the enemy
            }
            if (p.hero != null) {
                p.hero.IncreaseBulletCount();
            }
            Destroy(otherGO); // Destroy the projectile
        } else {
            Debug.Log("Enemy hit by non ProjectileHero: " + otherGO.name);
        }
    }
}
