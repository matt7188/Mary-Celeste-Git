using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MIBH_Animation : MonoBehaviour {

     GameObject NewPlayer;
    Camera PlayerCam;
    GameObject CenterHand;


    public GameObject MIBH;
    public Transform LookAt;
    public Camera Main;
    private float timer;
    public GameObject FakeEnd;

    public Image Blackout;
    public Text Text;

    private AudioSource Playthis;
    public AudioSource Playthis2;

    public AudioClip Hello;
    public AudioClip DidYou;
    public AudioClip Notmuch;
    public AudioClip AShip;
    public AudioClip ButThis;
    public AudioClip AndIAssue;
    public AudioClip VerySoon;

    public AudioClip BuildUp;
    public AudioClip Push;
    public AudioClip Slam1;
    public AudioClip Slam2;
    public AudioClip Footsteps;
    public AudioClip Teleport;
    public AudioClip Snap;


    void Start () {

        NewPlayer = FindObjectOfType<BasicMove>().gameObject;
        PlayerCam = NewPlayer.GetComponentInChildren<Camera>();
        PlayerCam.gameObject.SetActive(false);

        CenterHand = FindObjectOfType<RayCastFromCenter>().gameObject;
        CenterHand.SetActive(false);



        Camera [] All=FindObjectsOfType<Camera>();
        foreach(Camera report in All)
        {
            Debug.Log(report.name);
        }

        timer = 0;

        Blackout.gameObject.SetActive(false);

        var tempcolor = Text.color;
        tempcolor.a = 0;
        Text.color = tempcolor;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {

        Playthis = GetComponent<AudioSource>();
        StartCoroutine(RunAnimation());
            GetComponent<Animator>().Play("Push", -1, 0f);
            this.GetComponent<DisolveMIBH>().offset = -1;
}
}
IEnumerator RunAnimation()
    {

        Playthis.clip = BuildUp;
        Playthis.Play();
        //yield return new WaitForSeconds(6f);

        int j = 90;
        while (j>-90)
        {
            j-=2;
            Main.transform.rotation = Quaternion.Euler(Main.transform.rotation.x, j, Main.transform.rotation.z);
                yield return new WaitForSeconds(.01f);
        }
        Playthis2.clip = Push;
        Playthis2.Play();
        Playthis.clip = Hello;
        Playthis.Play();
        yield return new WaitForSeconds(1.7f);
        FakeEnd.SetActive(false);
        StartCoroutine(CameraAnimation());
        yield return new WaitForSeconds(2f);
        /* while (transform.position.x < 20)
         {
             transform.position = new Vector3(transform.position.x + .03f, transform.position.y, transform.position.z);
             yield return new WaitForSeconds(.04f);
         }*/

        yield return new WaitForSeconds(4.8f);
        Playthis2.clip = Teleport;
        Playthis2.Play();
        while (transform.position.x < 52.4f)
        {
            transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(.01f);
        }
        Playthis.clip = DidYou;
        Playthis.Play();
        while (timer != -1)
        {
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(.67f);
        while (transform.position.z<-4.5)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + .01f);

            yield return new WaitForSeconds(.01f);
        }
        Playthis.clip = ButThis;
        Playthis.Play();
        yield return new WaitForSeconds(2.9f);
        Playthis.clip = AndIAssue;
        Playthis.Play();
        yield return new WaitForSeconds(6.9f);

        //Main.GetComponent<AudioSource>().clip = Snap;
        //Main.GetComponent<AudioSource>().Play();
        //Blackout.gameObject.SetActive(true);
        Playthis.clip = VerySoon;
        Playthis.Play();

        DisolveMIBH DisolveMIBHOut = GetComponent<DisolveMIBH>();

        while (DisolveMIBHOut.offset < 5.75f)
        {
            DisolveMIBHOut.offset += .15f;
            yield return new WaitForSeconds(.01f);
        }

        float i = 0;
        float k = 200;

 NewPlayer.SetActive(true);
        NewPlayer.transform.position = Main.transform.position;
        NewPlayer.transform.rotation = Main.transform.rotation;
        PlayerCam.gameObject.SetActive(true);
        CenterHand.SetActive(true);
        this.gameObject.SetActive(false);

        while (Text.color.a < 1)
        {
            i++;
            
                var tempcolor = Text.color;
                tempcolor.a = i / k;
                Text.color = tempcolor;

            yield return new WaitForSeconds(0.01f);

        }

       
    }

    IEnumerator CameraAnimation()
    {
        //Main.GetComponent<AudioSource>().Play();

        while (Main.transform.position.x<58)
        {
            Main.transform.position = new Vector3(Main.transform.position.x + 1, Main.transform.position.y, Main.transform.position.z);
            yield return new WaitForSeconds(.02f);
        }
        Playthis.clip = Slam1;
        Playthis.Play();
        Playthis2.clip = Slam2;
        Playthis2.Play();
        while (Main.transform.position.y > 0.5)
        {
            Main.transform.position = new Vector3(Main.transform.position.x, Main.transform.position.y-.05f, Main.transform.position.z);
            yield return new WaitForSeconds(.01f);
            Main.transform.LookAt(LookAt);
        }
        Playthis.clip = Slam2;
        Playthis.Play();
        Playthis2.clip = Footsteps;
        Playthis2.Play();
        while (timer < 11)
        {
            Main.transform.LookAt(LookAt);
            timer += .1f;
             yield return new WaitForSeconds(.1f);
        }
        while (Main.transform.position.y < 1.51)
        {
            Main.transform.position = new Vector3(Main.transform.position.x, Main.transform.position.y + .01f, Main.transform.position.z);
            yield return new WaitForSeconds(.01f);
            Main.transform.LookAt(LookAt);
        }
        Playthis.clip = Notmuch;
        Playthis.Play();
        yield return new WaitForSeconds(1.6f);
        while (Main.transform.position.z > -6.5)
        {
            Main.transform.position = new Vector3(Main.transform.position.x, Main.transform.position.y , Main.transform.position.z- .09f);
            yield return new WaitForSeconds(.01f);
            Main.transform.LookAt(LookAt);
        }
        Playthis.clip = Slam1;
        Playthis.Play();
        Playthis2.clip = Slam2;
        Playthis2.Play();
        while (Main.transform.position.y > 0)
        {
            Main.transform.position = new Vector3(Main.transform.position.x, Main.transform.position.y - .07f, Main.transform.position.z);
            yield return new WaitForSeconds(.01f);
            Main.transform.LookAt(LookAt);
        }
        Playthis2.clip = Slam2;
        Playthis2.Play();
        Playthis.clip = AShip;
        Playthis.Play();
        timer = 0;
        while (timer < 6)
        {
            Main.transform.LookAt(LookAt);
            timer += .1f;
            yield return new WaitForSeconds(.1f);
        }
        while (Main.transform.position.y < 1.51&& Main.transform.position.z < -5)
        {
            Main.transform.position = new Vector3(Main.transform.position.x, Main.transform.position.y + .01f, Main.transform.position.z + .01f);
            yield return new WaitForSeconds(.01f);
            Main.transform.LookAt(LookAt);
        }
        timer = -1;


    }




    }
