using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BlinkColorOnHit : MonoBehaviour {
    [Header("Dynamic")]
    public bool ignoreOnCollisionEnter = false;

    private Material[] materials;
    private Color[] originalColors;
    public bool showingColor;
    private static float blinkDuration = 0.1f;
    private static Color blinkColor = Color.red;
    public float blinkCompleteTime;
    private BoundsCheck bndCheck;

    void Awake() {
        bndCheck = GetComponentInParent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i=0; i <materials.Length; i++) {
            originalColors[i] = materials[i].color;
        }
    }

    void Update() {
        if (showingColor && Time.time > blinkCompleteTime) RevertColors();
    }

    void OnCollisionEnter(Collision coll) {
        // Check for collisions with ProjectileHero
        ProjectileHero p = coll.gameObject.GetComponent<ProjectileHero>();
        if (p != null) {
            if (bndCheck != null && !bndCheck.isOnScreen) {
                return; // Don't show damage if this is off screen
            }
            SetColors();
        }
    }

    /// <summary>
    /// Sets the Albedo color (i.e., the main color) of all materials in the
    /// materials array to blinkColor, sets showingColor to true, and sets the
    /// time that the colors should be reverted.
    /// </summary>
    void SetColors() {
        foreach (Material m in materials) {
            m.color = blinkColor;
        }
        showingColor = true;
        blinkCompleteTime = Time.time + blinkDuration;
    }

    /// <summary>
    /// Reverts all materials in the materials array back to their original color
    /// and sets showingColor to false.
    /// </summary>
    void RevertColors() {
        for (int i=0; i <materials.Length; i++) {
            materials[i].color = originalColors[i];
        }
        showingColor = false;
    }
}