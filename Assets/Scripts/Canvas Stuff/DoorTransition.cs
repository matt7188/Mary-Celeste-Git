using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorTransition : MonoBehaviour {

    private Lighting_mananger AllLights;
    public Audio_Manager AudioManager;

    public Camera CameraViewer;
    public GameObject Backdrop;
    

    private Camera PlayerLocation;
    public GameObject Rotating;
    public float Time = 30;
    public float Fadein = 15;
    public float Fadeout = 15;
    public AudioClip Open;
    public AudioClip Close;
    private AudioSource source;

    private RawImage[] AllImages;

    public GameObject Ocean;
    //private Vector3 StartingOceanPlace;

	// Use this for initialization
	void Start () {

        AllLights = GameObject.FindObjectOfType<Lighting_mananger>();
        source = GetComponent<AudioSource>();

        transform.position = new Vector3(10000, 10000, 10000);
        PlayerLocation = FindObjectOfType<BasicMove>().GetComponentInChildren<Camera>();
        AllImages = Backdrop.GetComponentsInChildren<RawImage>();
        foreach (RawImage Change in AllImages){
            var tempcolor = Change.color;
            tempcolor.a = 0;
            Change.color = tempcolor;
        }
        //StartingOceanPlace = Ocean.transform.position;
    }
	
	private void Update()
    {
        
    }
    public void TransitionOpen (GameObject DoorSelected,string Scene_Load, string Scene_Unload,Rooms FinalPlace) {

        //Backdrop.
        CameraViewer.gameObject.transform.position = PlayerLocation.gameObject.transform.position;
        CameraViewer.gameObject.transform.rotation = PlayerLocation.gameObject.transform.rotation;

        transform.position = DoorSelected.transform.position;
        transform.rotation = DoorSelected.transform.rotation;
        StartCoroutine(openDoor(Scene_Unload, Scene_Load, FinalPlace));
    }

    IEnumerator openDoor(string Scene_Unload, string Scene_Load, Rooms FinalPlace)
    {
        float i = Time;
        float j = 0;
        float k = Fadeout;

        while (AllImages[0].color.a<1)
        {
            j++;
            foreach (RawImage Change in AllImages)
            {
                var tempcolor = Change.color;
                tempcolor.a=j/Fadein;
                Change.color = tempcolor;
            }
            yield return new WaitForSeconds(0.01f);
            
        }

        SceneManager.LoadScene(Scene_Load, LoadSceneMode.Additive);
        source.clip = Open;
        source.Play();
        if (AllLights!=null)
        AllLights.LightingCheck();
        /* if (FinalPlace.OceanLocation())
         {
             Ocean.transform.position = StartingOceanPlace;
         }
         else
             Ocean.transform.position = new Vector3(0, 0, 100000000);*/

        Ocean.SetActive(FinalPlace.OceanLocation());
        FindObjectOfType<BasicMove>().transform.position = FinalPlace.transform.position;
        

        for (int r = 0; r < SceneManager.sceneCount; r++)
            if (SceneManager.GetSceneAt(r).name == Scene_Unload)
            {
                SceneManager.UnloadSceneAsync(Scene_Unload);
            }

        while (i>0)
        {
            Rotating.transform.Rotate(Vector3.forward, 1);
            yield return new WaitForSeconds(0.02f);
            i--;
        }

        while (AllImages[0].color.a > 0)
        {
            foreach (RawImage Change in AllImages)
            {
                var tempcolor = Change.color;
                tempcolor.a = k / Fadeout;
                Change.color = tempcolor;
            }
            yield return new WaitForSeconds(0.01f);
            k--;
        }
        AudioManager.SwitchAmbiance(GameObject.FindObjectOfType<GameMananger>().Current_Room);
        source.clip = Close;
        source.Play();
        Rotating.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
