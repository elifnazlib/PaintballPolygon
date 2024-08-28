using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRayCast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            Shoot();
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);
    }

    private void Shoot() {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        

        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData))
        {
            // The Ray hit something!
            Debug.Log(hitData.collider.gameObject.name);
        }
    }
}
