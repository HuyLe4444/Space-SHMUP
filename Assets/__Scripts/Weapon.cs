using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType {
    none,
    blaster,
    spread,
    phaser,
    missile,
    laser,
    shield
}

[System.Serializable]
public class WeaponDefinition {
    public eWeaponType type = eWeaponType.none;
    public string letter; //Letter to show on the power-up                     
    public Color color = Color.white; //Color of Collar and power-up
    public GameObject weaponModelPrefab;
    public GameObject projectilePrefab; // Prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; // Amount of damage caused
    public float damagePerSec = 0;  //Damage per second (laser)
    public float delayBetweenShots = 0; 
    public float velocity = 50; //SPeed of projectiles
}

public class Weapon : MonoBehaviour {

}
