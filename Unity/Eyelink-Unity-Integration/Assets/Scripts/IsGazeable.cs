using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGazeable : MonoBehaviour
{
    public Material MyMaterial;
    private void Awake()
    {
        MyMaterial = this.GetComponent<Renderer>().material;
    }
    private void LateUpdate()
    {
       // MyMaterial.color = Color.white;
    }
}
