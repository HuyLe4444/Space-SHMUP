using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;
    [Header("Dynamic")]
    public Rigidbody rigid;
    [SerializeField]
    private eWeaponType _type;

    public eWeaponType type{
        get{return(_type);}
        set{SetType(value);}
    }
    public Hero hero;
    void Awake(){
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    public void SetHero(Hero heroRef)
    {
        hero = heroRef;
    }
    // Update is called once per frame
    void Update()
    {
        if(bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp)) {
            if (hero != null) {
                hero.IncreaseBulletCount();
            }
            Destroy(gameObject);
        }
    }

    public void SetType(eWeaponType eType) {
        _type = eType;
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION(_type);
        rend.material.color = def.projectileColor;
    }

    public Vector3 vel {
        get {return rigid.velocity;}
        set {rigid.velocity = value;}
    }
}
