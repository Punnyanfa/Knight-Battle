using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallaraxEffects : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // Starting position for the pallax game object
    Vector2 startingPosition;

    // Start Z value of the pallax game object
    float startingZ;

    // Distance that the camera has moved form starting position of the pallax object

    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zDistanceFormTarget => transform.position.z - followTarget.transform.position.z;

    // if  object is in front of target, use near clip plane. If behind target use farClipPlane
    float clippingPlane => (cam.transform.position.z + (zDistanceFormTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // the futher the object from the player, the faster the object ParallaxEffect object will move. Drag it's z value closer to the target to make it move slower
    float parallaxFactor => Mathf.Abs(zDistanceFormTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // when the target moves, move the parallax object the same distance times a multiplier
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        // the X/Y position changes based on target travel speed times the parallax factor, but z stays consistent 
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
