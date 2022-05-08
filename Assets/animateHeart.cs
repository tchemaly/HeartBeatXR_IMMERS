using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateHeart : MonoBehaviour
{
    private Animation anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
      
    }

    // double the spin speed when true

    public float bpm = 80.0f;
    void Update()
    {
        // have spin speed reverted to 1.0 second
        anim["heartbeat"].speed = (1.0f * bpm) / 60.0f;
        // leave spin or jump to complete before changing
        if (anim.isPlaying)
        {
            return;
        }
        anim.Play();


        
        
        

        
    }
}
