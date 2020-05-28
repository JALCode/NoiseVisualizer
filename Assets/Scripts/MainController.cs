using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization; 

public class MainController : MonoBehaviour
{
    public NoiseGenerator noiseGenerator;
    public NoiseDisplay noiseDisplay;
    public Camera noiseView;

    public Slider horizontalSlider;
    public Slider verticalSlider;

    public TMP_Dropdown modeDropdown;

    public ValueButton subScale;
    public TMP_InputField scale;
    public ValueButton addScale;

    public ValueButton subOctaves;
    public TMP_InputField octaves;
    public ValueButton addOctaves;

    public ValueButton subPersistance;
    public TMP_InputField persistance;
    public ValueButton addPersistance;

    public ValueButton subLacunarity;
    public TMP_InputField lacunarity;
    public ValueButton addLacunarity;

    public ValueButton subSeed;
    public TMP_InputField seed;
    public ValueButton addSeed;

    public ValueButton subOffset;
    public TMP_InputField offset;
    public ValueButton addOffset;

    public ValueButton subLOD;
    public TMP_InputField lod;
    public ValueButton addLOD;

    public Toggle flatshading;

    public Toggle falloff;

    public AudioSource clickDown;
    public AudioSource clickUp;
    public AudioSource toggleOff;
    public AudioSource toggleOn;
    public AudioSource change;

    Vector3 oldRotation = new Vector3(0, 0, 30);

    Vector3 planeRotation = new Vector3(0,-90,-45);
    Vector3 lineRotation = new Vector3(0, -90, 45);

    public Material backgroundMaterial;
   
    public Button lowerColor;
    public Button upperColor;
    public Button backgroundColor;

    public FlexibleColorPicker colorPicker;


    // Start is called before the first frame update
    void Start()
    {
        horizontalSlider.onValueChanged.AddListener(delegate { HorizontalRotation(); });
        verticalSlider.onValueChanged.AddListener(delegate { VerticalRotation(); });

        modeDropdown.onValueChanged.AddListener(delegate { SelectMode(); });
        //SelectMode();

        scale.onValueChanged.AddListener(delegate { ScaleChange(); });
        scale.onEndEdit.AddListener(delegate { ScaleChange(); });
        scale.text = noiseGenerator.noiseScale.ToString(CultureInfo.InvariantCulture);

        octaves.onValueChanged.AddListener(delegate { OctavesChange(); });
        octaves.onEndEdit.AddListener(delegate { OctavesChange(); });
        octaves.text = noiseGenerator.octaves.ToString(CultureInfo.InvariantCulture);

        persistance.onValueChanged.AddListener(delegate { PersistanceChange(); });
        persistance.onEndEdit.AddListener(delegate { PersistanceChange(); });
        persistance.text = noiseGenerator.persistance.ToString(CultureInfo.InvariantCulture);

        lacunarity.onValueChanged.AddListener(delegate { LacunarityChange(); });
        lacunarity.onEndEdit.AddListener(delegate { LacunarityChange(); });
        lacunarity.text = noiseGenerator.lacunarity.ToString(CultureInfo.InvariantCulture);

        seed.onValueChanged.AddListener(delegate { SeedChange(); });
        seed.onEndEdit.AddListener(delegate { SeedChange(); });
        seed.text = noiseGenerator.seed.ToString(CultureInfo.InvariantCulture);

        offset.onValueChanged.AddListener(delegate { OffsetChange(); });
        offset.onEndEdit.AddListener(delegate { OffsetChange(); });
        offset.text = noiseGenerator.offset.x.ToString(CultureInfo.InvariantCulture);

        lod.onValueChanged.AddListener(delegate { LODChange(); });
        lod.onEndEdit.AddListener(delegate { LODChange(); });
        lod.text = "7";

        flatshading.onValueChanged.AddListener(delegate { FlatshadingChange(); });
        flatshading.isOn = noiseGenerator.useFlatShading;
             
        falloff.onValueChanged.AddListener(delegate { FalloffChange(); });
        falloff.isOn = noiseGenerator.useFalloff;

        lowerColor.onClick.AddListener(delegate { SetLowerColor(); });
        upperColor.onClick.AddListener(delegate { SetUpperColor(); });
        backgroundColor.onClick.AddListener(delegate { SetBackgroundColor(); });

        noiseGenerator.DrawNoise();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Change all parses and to strings with CultureInfo.InvariantCulture

        if (subScale.triggered) {
            scale.text = (float.Parse(scale.text, CultureInfo.InvariantCulture) - subScale.value).ToString("F2", CultureInfo.InvariantCulture);
            //ScaleChange();
        } else if (addScale.triggered) {
            scale.text = (float.Parse(scale.text, CultureInfo.InvariantCulture) + addScale.value).ToString("F2", CultureInfo.InvariantCulture);
            //ScaleChange();
        } else if (subOctaves.triggered) {
            octaves.text = (int.Parse(octaves.text, CultureInfo.InvariantCulture) - subOctaves.value).ToString(CultureInfo.InvariantCulture);
            //OctavesChange();
        } else if (addOctaves.triggered) {
            octaves.text = (int.Parse(octaves.text, CultureInfo.InvariantCulture) + addOctaves.value).ToString(CultureInfo.InvariantCulture);
            //OctavesChange();
        } else if (subPersistance.triggered) {
            persistance.text = (float.Parse(persistance.text, CultureInfo.InvariantCulture) - subPersistance.value).ToString("F3", CultureInfo.InvariantCulture);
            //PersistanceChange();
        } else if (addPersistance.triggered) {
            persistance.text = (float.Parse(persistance.text, CultureInfo.InvariantCulture) + addPersistance.value).ToString("F3",CultureInfo.InvariantCulture);
            //PersistanceChange();
        } else if (subLacunarity.triggered) {
            lacunarity.text = (float.Parse(lacunarity.text, CultureInfo.InvariantCulture) - subLacunarity.value).ToString("F2", CultureInfo.InvariantCulture);
            //LacunarityChange();
        } else if (addLacunarity.triggered) {
            lacunarity.text = (float.Parse(lacunarity.text, CultureInfo.InvariantCulture) + addLacunarity.value).ToString("F2", CultureInfo.InvariantCulture);
            //LacunarityChange();
        } else if (subSeed.triggered) {
            seed.text = (int.Parse(seed.text, CultureInfo.InvariantCulture) - subSeed.value).ToString(CultureInfo.InvariantCulture);
            //SeedChange();
        } else if (addSeed.triggered) {
            seed.text = (int.Parse(seed.text, CultureInfo.InvariantCulture) + addSeed.value).ToString(CultureInfo.InvariantCulture);
            //SeedChange();
        } else if (subOffset.triggered) {
            offset.text = (float.Parse(offset.text, CultureInfo.InvariantCulture) - subOffset.value).ToString("F2", CultureInfo.InvariantCulture);
            //OffsetChange();
        } else if (addOffset.triggered) {
            offset.text = (float.Parse(offset.text, CultureInfo.InvariantCulture) + addOffset.value).ToString("F2", CultureInfo.InvariantCulture);
            //OffsetChange();
        } else if (subLOD.triggered) {
            lod.text = (int.Parse(lod.text, CultureInfo.InvariantCulture) - subLOD.value).ToString(CultureInfo.InvariantCulture);
            //LODChange();
        } else if (addLOD.triggered) {
            lod.text = (int.Parse(lod.text, CultureInfo.InvariantCulture) + addLOD.value).ToString(CultureInfo.InvariantCulture);
            //LODChange();
        }

    }

    void HorizontalRotation(){
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,Mathf.Lerp(0,360, horizontalSlider.value), transform.rotation.eulerAngles.z);
    }

    void VerticalRotation() {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(30, -120, verticalSlider.value));
    }

    void SelectMode() {
        switch (modeDropdown.captionText.text) {
            case "Line":

                noiseDisplay.lineRenderer.enabled = true;
                noiseDisplay.planeRenderer.enabled = false;
                noiseDisplay.meshRenderer.enabled = false;
                noiseDisplay.standRenderer.enabled = false;


                verticalSlider.interactable = false;
                horizontalSlider.interactable = false;

                subLOD.interactable = false;
                lod.interactable = false;
                addLOD.interactable = false;

                flatshading.interactable = false;



                if (noiseGenerator.drawMode == NoiseGenerator.DrawMode.Mesh) {
                    oldRotation = transform.rotation.eulerAngles;
                }
                transform.rotation = Quaternion.Euler(lineRotation);
                

                noiseGenerator.drawMode = NoiseGenerator.DrawMode.Line;

                break;

            case "NoiseMap":

                noiseDisplay.lineRenderer.enabled = false;
                noiseDisplay.planeRenderer.enabled = true;
                noiseDisplay.meshRenderer.enabled = false;
                noiseDisplay.standRenderer.enabled = false;


                verticalSlider.interactable = false;
                horizontalSlider.interactable = false;

                subLOD.interactable = false;
                lod.interactable = false;
                addLOD.interactable = false;

                flatshading.interactable = false;

                if (noiseGenerator.drawMode == NoiseGenerator.DrawMode.Mesh) {
                    oldRotation = transform.rotation.eulerAngles;
                }
                transform.rotation = Quaternion.Euler(planeRotation);

                noiseGenerator.drawMode = NoiseGenerator.DrawMode.Plane;

                break;

            case "Mesh":

                noiseDisplay.lineRenderer.enabled = false;
                noiseDisplay.planeRenderer.enabled = false;
                noiseDisplay.meshRenderer.enabled = true;
                noiseDisplay.standRenderer.enabled = true;


                verticalSlider.interactable = true;
                horizontalSlider.interactable = true;

                subLOD.interactable = true;
                lod.interactable = true;
                addLOD.interactable = true;

                flatshading.interactable = true;

                if (noiseGenerator.drawMode == NoiseGenerator.DrawMode.Line
                || noiseGenerator.drawMode == NoiseGenerator.DrawMode.Plane) {
                    transform.rotation = Quaternion.Euler(oldRotation);
                }

                noiseGenerator.drawMode = NoiseGenerator.DrawMode.Mesh;

                break;
        }

        change.Stop();
        change.Play();
        noiseGenerator.DrawNoise();
    }

    void ScaleChange() {

        noiseGenerator.noiseScale = float.Parse(scale.text,CultureInfo.InvariantCulture);
        noiseGenerator.DrawNoise();
    }

    void OctavesChange() {
        int oct = int.Parse(octaves.text);
        if (oct < 0) {
            oct = 0;
            octaves.text = oct.ToString(CultureInfo.InvariantCulture);
        }
        noiseGenerator.octaves = oct;
        noiseGenerator.DrawNoise();
    }

    void PersistanceChange() {
        
        float val = float.Parse(persistance.text,CultureInfo.InvariantCulture);

        if (val < 0) {
            val = 0;
            persistance.text = val.ToString(CultureInfo.InvariantCulture);
        }else if (val > 1) {
            val = 1;
            persistance.text = val.ToString(CultureInfo.InvariantCulture);
        }
        noiseGenerator.persistance = val;
        noiseGenerator.DrawNoise();
    }

    void LacunarityChange() {
        float val = float.Parse(lacunarity.text, CultureInfo.InvariantCulture);
        if (val < 1) {
            val = 1;
            lacunarity.text = val.ToString();
        }
        noiseGenerator.lacunarity = val;
        noiseGenerator.DrawNoise();
    }

    void SeedChange() {
        int val = int.Parse(seed.text, CultureInfo.InvariantCulture);

        noiseGenerator.seed = val;
        noiseGenerator.DrawNoise();
    }

    void OffsetChange() {
        float val = float.Parse(offset.text, CultureInfo.InvariantCulture);

        noiseGenerator.offset.x = val;
        noiseGenerator.DrawNoise();
    }

    void LODChange() {
        int val = int.Parse(lod.text, CultureInfo.InvariantCulture);
        if (val < 1) {
            val = 1;
            lod.text = val.ToString(CultureInfo.InvariantCulture);
        } else if (val > 7) {
            val = 7;
            lod.text = val.ToString(CultureInfo.InvariantCulture);
        }
        switch (val) {
            case 1:
                noiseGenerator.LOD = 12;
                break;
            case 2:
                noiseGenerator.LOD = 8;
                break;
            case 3:
                noiseGenerator.LOD = 6;
                break;
            case 4:
                noiseGenerator.LOD = 4;
                break;
            case 5:
                noiseGenerator.LOD = 2;
                break;
            case 6:
                noiseGenerator.LOD = 1;
                break;
            case 7:
                noiseGenerator.LOD = 0;
                break;
        }
        

        noiseGenerator.DrawNoise();
    }

    void FlatshadingChange() {

        noiseGenerator.useFlatShading = flatshading.isOn;
        if (noiseGenerator.useFlatShading) {
            toggleOn.Stop();
            toggleOn.Play();
        } else {
            toggleOff.Stop();
            toggleOff.Play();
        }
        noiseGenerator.DrawNoise();
    }

    void FalloffChange() {

        noiseGenerator.useFalloff = falloff.isOn;

        if (noiseGenerator.useFalloff) {
            toggleOn.Stop();
            toggleOn.Play();
        } else {
            toggleOff.Stop();
            toggleOff.Play();
        }

        noiseGenerator.DrawNoise();
    }

    void SetLowerColor() {

        GradientColorKey [] gck = noiseGenerator.colorGradient.colorKeys;
        GradientAlphaKey [] gak = noiseGenerator.colorGradient.alphaKeys;
        gck[0].color = colorPicker.color;
        gak[0].alpha = colorPicker.color.a;

        noiseGenerator.colorGradient.SetKeys(gck, gak);

        change.Stop();
        change.Play();

        noiseGenerator.DrawNoise();

    }
    void SetUpperColor() {

        GradientColorKey[] gck = noiseGenerator.colorGradient.colorKeys;
        GradientAlphaKey[] gak = noiseGenerator.colorGradient.alphaKeys;
        gck[1].color = colorPicker.color;
        gak[1].alpha = colorPicker.color.a;

        noiseGenerator.colorGradient.SetKeys(gck, gak);

        change.Stop();
        change.Play();

        noiseGenerator.DrawNoise();

    }
    void SetBackgroundColor() {

        change.Stop();
        change.Play();

        backgroundMaterial.SetColor("_BaseColor",colorPicker.color);

    }


}
