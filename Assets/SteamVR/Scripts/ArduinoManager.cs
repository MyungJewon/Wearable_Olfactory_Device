using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
public class ArduinoManager : MonoBehaviour
{

    public enum PortNumber
    {
        COM1, COM2, COM3, COM4,
        COM5, COM6, COM7, COM8,
        COM9, COM10, COM11, COM12,
        COM13, COM14, COM15, COM16
    }

    private SerialPort serial;
    private string fanvalue;
    [SerializeField]
    private PortNumber portNumber = PortNumber.COM5;
    [SerializeField]
    private string baudRate = "115200";

    // Start is called before the first frame update
    void Awake()
    {
        serial = new SerialPort(portNumber.ToString(), int.Parse(baudRate));

        if (!serial.IsOpen)
        {
            serial.Open();
        }
        else{
        }
        fanvalue = "3";
    }
    private void Update()
    {
        if (!serial.IsOpen)
        {
            serial.Open();
        }
    }
    public void click1()
    {
        //Debug.Log("버튼1");
        if(fanvalue=="5"){
           //Debug.Log("1꺼짐");
            fanvalue = "4";
            serial.Write(fanvalue);
        }
        else{
            //Debug.Log("1켜짐");
            fanvalue = "5";
            serial.Write(fanvalue);
        }  
    }
    
    // 2번 팬 동작 함수
    public void click2()
    {
        //Debug.Log("버튼2");
        if(fanvalue=="7"){
            //Debug.Log("2꺼짐");
            fanvalue = "6";
            serial.Write(fanvalue);
        }
        else{
            //Debug.Log("2켜짐");
            fanvalue = "7";
            serial.Write(fanvalue);
        }  
    }
    // 3번 팬 동작 함수
    public void click3()
    {
        //Debug.Log("버튼3");
        if(fanvalue=="9"){
            //Debug.Log("3꺼짐");
            fanvalue = "8";
            serial.Write(fanvalue);
        }
        else{
            //Debug.Log("3켜짐");
            fanvalue = "9";
            Debug.Log(serial);
            serial.Write(fanvalue);
        }  
    }
    // 4번 팬 동작 함수
    public void click4()
    {
        //Debug.Log("버튼4");
        if(fanvalue=="11"){
            //Debug.Log("4꺼짐");
            fanvalue = "10";
            serial.Write(fanvalue);
        }
        else{
           // Debug.Log("4켜짐");
            fanvalue = "11";
            serial.Write(fanvalue);
        }  
    }
    // 5번 팬 동작 함수
     public void click5()
    {
        //Debug.Log("버튼5");
        if(fanvalue=="13"){
            //Debug.Log("5꺼짐");
            fanvalue = "12";
            serial.Write(fanvalue);
        }
        else{
            //Debug.Log("5켜짐");
            fanvalue = "13";
            serial.Write(fanvalue);
        }  
    }
    
    // 유니티 플레이가 끝나면 모든 팬을 끄는 신호를 아두이노로 송신
    private void OnApplicationQuit()
    {
        Debug.Log("끝");
        fanvalue = "3";
        serial.Write(fanvalue);
    }
}
