using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeCamColor : MonoBehaviour
{
    public GameObject parapluie;
    public Material parapluieMaterial;
    private List<ColorZone> colorZones;
    public Color color;
    private Color colorParapluie;
    private Color colorEdge;

    public PencilContour pencilContour;
    //public Material color2;
    public Color colorReset;
    public Material myColor;
    public Camera cameraMain;
    //private float t = 0f;
    public float speedColorChange;
    public AnimationCurve lerpCurve;

    void Start()
    {
        cameraMain = Camera.main;
        colorZones = GetComponentsInChildren<ColorZone>().ToList();
    }

    public void Update()
    {
        {
            color = colorReset;
            colorParapluie = colorReset;
            colorEdge = colorReset;
            foreach (var colorZone in colorZones)
            {
                float dist = Vector3.Distance(colorZone.GameObject().transform.position, parapluie.transform.position);
                float slider = 1 - (dist / colorZone.radius);
                if (slider <= 0f) slider = 0f;
                float lerpedSlider = lerpCurve.Evaluate(slider);
                color += colorZone.color * lerpedSlider;
                colorParapluie += colorZone.colorParapluie * lerpedSlider;
                colorEdge += colorZone.colorEdge * lerpedSlider;
                //Debug.Log("Distance avec le colorZone" + colorZone.gameObject + dist + ", valeur du slider : " + slider);
            }

            /*for (int i = 0; i < colorZones.Count; i++)
            {
                float dist = Vector3.Distance(colorZones[i].GameObject().transform.position, parapluie.transform.position);
                float slider = 1 - (dist / colorZones[i].radius);
                float lerpedSlider = lerpCurve.Evaluate(slider);
                color += colorZones[i].color * lerpedSlider;
            }*/

            cameraMain.gameObject.GetComponent<PencilContour>().backgroundColor = Color.Lerp(cameraMain.gameObject.GetComponent<PencilContour>().backgroundColor, color, 1f);
            myColor.color = Color.Lerp(cameraMain.gameObject.GetComponent<PencilContour>().backgroundColor, color, 1f);
            //color2.SetColor("_Emission", color);
            //color2.color = Color.Lerp(cameraMain.gameObject.GetComponent<PencilContour>().backgroundColor, color, 1f);
            pencilContour.edgeColor = colorEdge;
        
                parapluieMaterial.SetColor("_BaseColor", colorParapluie);
                parapluieMaterial.SetColor("_HighlightColor", colorParapluie);
            

            //parapluieMaterial.color = Color.Lerp(cameraMain.gameObject.GetComponent<PencilContour>().backgroundColor, color, 1f);
            //t += Time.deltaTime * speedColorChange;
        }
    }
}
