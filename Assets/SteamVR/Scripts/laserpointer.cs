using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class laserpointer : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean selectAction;

    public GameObject laserPrefab; // 레이저 프리팹
    private GameObject laser; // 레이저(인스턴스)
    private Transform laserTransform; // 레이저 트랜스폼
    private Vector3 hitPoint; // 레이저가 부딪친 부분

    TaskManager taskManager;

    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportAction.GetState(handType))
        {
            RaycastHit hit;
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100))
            {
                hitPoint = hit.point;
                ShowLaser(hit);

                GameObject temp = hit.collider.gameObject;
                
                if (selectAction.GetState(handType) && hit.transform.CompareTag("Button"))
                {
                    Debug.Log(hit.collider.name);
                    switch (hit.collider.name)
                    {
                        case "Cafe":
                            taskManager.SetSelectAnswer("Cafe");
                            Debug.Log("선택 = Cafe");
                            break;
                        case "Orange":
                            taskManager.SetSelectAnswer("Orange");
                            Debug.Log("선택 = Orange");
                            break;
                        case "Flower":
                            taskManager.SetSelectAnswer("Flower");
                            Debug.Log("선택 = Flower");
                            break;
                        case "Pizza":
                            taskManager.SetSelectAnswer("Pizza");
                            Debug.Log("선택 = Pizza");
                            break;
                    }
                }
            }
        }
        else
        {
            laser.SetActive(false);
            laser.SetActive(false);
        }
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laser.transform.position = Vector3.Lerp(controllerPose.transform.position,
            hitPoint, 0.5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x,
            laserTransform.localScale.y,
            hit.distance);
    }
}