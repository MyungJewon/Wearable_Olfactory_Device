using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellAngine : MonoBehaviour
{
    ArduinoManager arduinoManager;
    public GameObject player;
    public GameObject[] smellarea;
    Vector3[] areavector;
    void Start(){
        areavector=new Vector3[smellarea.Length];
        arduinoManager=GameObject.Find("ArduinoManager").GetComponent<ArduinoManager>();
        for(int i=0; i<smellarea.Length;i++){
            areavector[i]=smellarea[i].transform.position;
        }
    }
    void Update(){
        if(player.transform.position.x<areavector[0].x){
            arduinoManager.click1();
        }
        else if(player.transform.position.x<areavector[1].x){
            arduinoManager.click2();
        }
        else if(player.transform.position.x<areavector[2].x){
            arduinoManager.click4();
        }
        else{
            arduinoManager.click3();
        }
    }
    
}
