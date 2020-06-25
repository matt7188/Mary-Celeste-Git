using UnityEngine;
using System.Collections;

/// <summary>
/// Page Curler component
/// 
/// You can directly manipulate the exposed fields to control the page turning effect,
/// or use an animation to drive it. This component also has additional interface functions
/// for changing the textures, reseting to the initial state, etc...
/// </summary>
[ExecuteInEditMode]
public class PageCurl : MonoBehaviour 
{
    public float initRotation = 90.0f;  //initial page rotation
    public float endRotation = 270.0f;  //end page rotation
    [Range(0f, 1f)]
    public float rotRatio = 0f;     //rotation progress

    [Range(0f, 90f)]
    public float theta = 0f;    //cone angle
    [Range(0f, -50f)]
    public float apex = 0f;     //cone position
    public bool invertDirection = false;   //invert cone position

    //Initial values (saved for resetting to init position)
    float initTheta;
    float initApex;

    Material[] materialRefs = null;     //references to the page materials
    Vector3 localScale;
	
    void Start()
    {
        initTheta = theta;
        initApex = apex;
        localScale = transform.localScale;
    }

	void Update () 
    {
        //Assign the page curling effect values
        float thetaRad = theta * Mathf.Deg2Rad;
        float sinTheta = Mathf.Sin(thetaRad);
        float cosTheta = Mathf.Cos(thetaRad);

        foreach (Material mat in GetMaterials())
        {
            mat.SetFloat("_ScaleX", localScale.x);
            mat.SetFloat("_ScaleY", localScale.z);
            mat.SetFloat("_SinTheta", sinTheta);
            mat.SetFloat("_CosTheta", cosTheta);
            mat.SetFloat("_Apex", apex);
            mat.SetFloat("_ConeSide", invertDirection ? -1f : 1f);
        }

        //Adjusts the page rotation
        Quaternion rot0 = Quaternion.Euler(0f, -180f, initRotation);
        Quaternion rot1 = Quaternion.Euler(0f, -180f, endRotation);
        transform.localRotation = Quaternion.Slerp(rot0, rot1, rotRatio);
	}

    //Play the default page flip animation, and return its duration
    public float Flip(bool backwards = false)
    {
        string animId = backwards ? "PageFlip_Inv" : "PageFlip";
        GetComponent<Animation>().Stop();
        GetComponent<Animation>().Play(animId);
        return GetComponent<Animation>()[animId].length;
    }

    //Reset to its initial state
    public void Reset()
    {
        GetComponent<Animation>().Stop();
        rotRatio = 0f;
        theta = initTheta;
        apex = initApex;
    }

    //Access the front side texture
    public Texture FrontTexture
    {
        get
        {
            return GetMaterials()[0].mainTexture;
        }
        set
        {
            GetMaterials()[0].mainTexture = value;
        }
    }

    //Access the back side texture
    public Texture BackTexture
    {
        get
        {
            return GetMaterials()[1].mainTexture;
        }
        set
        {
            GetMaterials()[1].mainTexture = value;
        }
    }

    //Animation event called when the page flip motion has ended
    public void OnPageFlip()
    {
        //Do nothing
    }

    //Return the material list
    private Material[] GetMaterials()
    {
        if (materialRefs == null)
        {
            if (Application.isEditor)
                materialRefs = GetComponent<Renderer>().sharedMaterials;
            else
                materialRefs = GetComponent<Renderer>().materials;
        }
        return materialRefs;
    }

}
