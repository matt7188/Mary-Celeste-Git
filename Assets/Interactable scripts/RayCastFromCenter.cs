using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RayCastFromCenter : MonoBehaviour {

    public static bool Clickable;

    Camera cam;

    Image m_Image;
    //Set this in the Inspector

    public Sprite Nutral;
    public Sprite Pickup;
    public Sprite Lock;
    public Sprite Move;
    public Sprite Investigate;
    public Sprite Solve;

    GameObject Looking_At;

    public bool ShowingImage;

    public static bool right;
    Vector3 centerSpot;

    void Start()
    {
        m_Image = GetComponent<Image>();
        m_Image.sprite = Nutral;
        cam = Camera.main;
        ShowingImage = true;
        right = false;
        centerSpot = this.transform.position;
    }
    

    void Update()
    {
        
        if (ShowingImage)
        {
            m_Image.enabled = true;

             if (Input.GetMouseButtonDown(1))
                {
                right = !right;
                    BasicMove.Lock = right;
                }


             if (right)
            {
                this.transform.position = Input.mousePosition;
            }
        else
        {

            this.transform.position = centerSpot;
        }

            if (cam == null)
            {
                cam = Camera.main;
            }
            else
            {
                Ray ray;
                if (right)
                    ray = cam.ScreenPointToRay(Input.mousePosition);
                else
                    ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;

                if (Clickable && Physics.Raycast(ray, out hit) && hit.transform.gameObject.GetComponent<Interactable>() != null)
                {
                    // print("I'm looking at " + hit.transform.name);

                    Looking_At = hit.transform.gameObject;
                    switch (Looking_At.GetComponent<Interactable>().Type_of_Interaction)
                    {
                        case Interactable.TypeOfInteraction.Pickup:
                            m_Image.sprite = Pickup;
                            //print("Picking up");
                            break;
                        case Interactable.TypeOfInteraction.Lock:
                            m_Image.sprite = Lock;
                            break;
                        case Interactable.TypeOfInteraction.Key:
                            m_Image.sprite = Pickup;
                            break;
                        case Interactable.TypeOfInteraction.Door:
                            m_Image.sprite = Move;
                            break;
                        case Interactable.TypeOfInteraction.Information:
                            m_Image.sprite = Investigate;
                            break;
                        case Interactable.TypeOfInteraction.People:
                            m_Image.sprite = Solve;
                            break;
                        case Interactable.TypeOfInteraction.Puzzle:
                            m_Image.sprite = Solve;
                            break;
                        default:
                            print("nope");
                            break;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        Looking_At.GetComponent<Interactable>().clickOn();
                    }

                }
                else
                {
                    // print("I'm looking at nothing!");
                    m_Image.sprite = Nutral;
                    Looking_At = null;
                }
            }
        }
        else
        {
            m_Image.enabled = false;
        }
    }

   public GameObject CurrentlyLookingAt()
    {
        return Looking_At;
    }

}
