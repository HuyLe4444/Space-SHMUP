using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4 : Enemy
{
    public float duration = 4;
    private Vector3 p0, p1;
    private float timeStart;

    void Start() {
        health = 20;
        p0 = p1 = pos;
        InitMovement();
    }

    void InitMovement() {
        p0 = p1; // Set p0 to the old p1

        // Assign a new on-screen location to p1
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;

        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        // Make sure that it moves to a different quadrant of the screen
        if (Mathf.Abs(p0.x) > Mathf.Abs(p0.y)) {
            p1.x *= -1;
        } else {
            p1.y *= -1;
        }

        // Reset the time
        timeStart = Time.time;
    }
    public override void Move() {
        // This completely overrides Enemy.Move() with linear interpolation
        float u = (Time.time - timeStart) / duration;

        if (u >= 1) {
            InitMovement();
            u = 0;
        }

        // Easing: Sine -0.15f
        u = u - 0.15f * Mathf.Sin(u * 2 * Mathf.PI); // Easing: Sine -0.15
        pos = (1 - u) * p0 + u * p1; // Simple linear interpolation
    }
}
