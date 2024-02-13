using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using System.IO;
using System.Text;
using System.Globalization;

public class TaskManager : MonoBehaviour
{
    // 함수 참조
    ArduinoManager arduinoManager;
    public bool isvent = true;

    public bool istutorial = false;

    // 연결 오브젝트
    public GameObject[] shops;
    public GameObject selectCanvas;
    private GameObject player;
    public GameObject teleportTargetObject;

    [Serializable]
    public struct Task
    {
        public float costTime;
        public string answer, select;
        public bool isRight;
    }

    // 내부 속성
    private Transform currentShop;
    int shopnum = 0;
    int tutorialnum = 0;
    private float timer = 0;
    private bool doTimer = false;

    // 이동 포인트
    public Transform playerReturnPosition;
    public Transform shopSpawnPoint;

    public Task[] tasks = new Task[12];
    private int taskNumber = 0;
    private List<int> availableNumbers = new List<int> { 0, 1, 2, 3 };
    private Dictionary<int, int> numberCounts = new Dictionary<int, int>(); // 숫자의 빈도를 저장하는 딕셔너리
    private int maxCount = 3; // 최대 허용 회수

    private void Start()
    {
        arduinoManager = GameObject.Find("ArduinoManager").GetComponent<ArduinoManager>();
        player = GameObject.FindWithTag("Player");

        SetNewTask();
    }

    private void Update()
    {
        if (doTimer)
        {
            timer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.A))
            SaveTasksToCSV();
    }

    public void SetSelectAnswer(string selectAnswer)
    {
        Task newTask = new Task
        {
            costTime = timer,
            select = selectAnswer,
            answer = currentShop.name,
            isRight = selectAnswer == currentShop.name,
        };

        Debug.Log(taskNumber + "번 task Data = " + newTask.costTime + "/" + newTask.select + "/" + newTask.answer);

        tasks[taskNumber] = newTask;
        taskNumber++;
        timer = 0;
        doTimer = false;
        SetNewTask();
    }

    public void Selectfragrance() //향기분출t
    {
        if (istutorial == false)
        {
            Debug.Log("shopN = " + shopnum);
            switch (shopnum)
            {
                case 0:
                    arduinoManager.click5();
                    break;
                case 1:
                    arduinoManager.click4();
                    break;
                case 2:
                    arduinoManager.click2();
                    break;
                case 3:
                    arduinoManager.click1();
                    break;
            }
        }
        else
        {
            switch (tutorialnum)
            {
                case 0:
                    arduinoManager.click5();
                    Debug.Log("555555");
                    break;
                case 1:
                    arduinoManager.click3();
                    Debug.Log("3333333");

                    break;
                case 2:
                    arduinoManager.click4();
                    break;
                case 3:
                    arduinoManager.click3();
                    break;
                case 4:
                    arduinoManager.click2();
                    break;
                case 5:
                    arduinoManager.click3();
                    break;
                case 6:
                    arduinoManager.click1();
                    break;
                case 7:
                    arduinoManager.click3();
                    break;
            }

            tutorialnum += 1;
            Debug.Log(tutorialnum + "df");
        }
    }

    public void MovementFinishEvent()
    {
        Debug.Log("도착");
        OpenText();
        doTimer = true;
    }

    public void SetNewTask()
    {
        CloseText();
        DestroyPreviousShop();
        
        
        if (taskNumber >= 12)
        {
            SaveTasksToCSV();
            Destroy(teleportTargetObject);
            player.transform.position = playerReturnPosition.position; // 플레이어를 처음 위치로 이동
            return;
        }

        Debug.Log("새로운 task 시작 : " + taskNumber);
        CreateNewShop();
    }

    /// <summary>
    /// 이동 후 정답 판별 UI On/Off
    /// </summary>
    public void OpenText()
    {
        selectCanvas.SetActive(true);
    }

    void CloseText()
    {
        selectCanvas.SetActive(false);
    }


    void CreateNewShop()
    {
        player.transform.position = playerReturnPosition.position; // 플레이어를 처음 위치로 이동
        shopnum = GetLimitedRandomNumber();
        currentShop = Instantiate(shops[shopnum], shopSpawnPoint).transform; // 새로운 Shop을 shopSpawnPoint 오브젝트의 위치로 복제
        currentShop.name = currentShop.name.Replace("(Clone)", ""); // "(Clone)" 접미사를 제거
        //arduinoManager.click3();
    }

    private int GetLimitedRandomNumber()
    {
        // 사용 가능한 숫자 중에서 랜덤하게 선택
        int index = Random.Range(0, availableNumbers.Count);
        int randomNumber = availableNumbers[index];

        // 선택된 숫자의 빈도 증가
        if (numberCounts.ContainsKey(randomNumber))
            numberCounts[randomNumber]++;
        else
            numberCounts[randomNumber] = 1;

        // 만약 빈도가 최대 회수(예: 3회)에 도달하면 다른 숫자 선택
        if (numberCounts[randomNumber] >= maxCount)
            availableNumbers.Remove(randomNumber);

        return randomNumber;
    }

    // 다음 Task를 준비하기 위해 현재 World에 존재하는 Shop(CurrentShop)을 파괴
    void DestroyPreviousShop()
    {
        if (!(currentShop is null))
            Destroy(currentShop.gameObject);
    }

    private void SaveTasksToCSV()
    {
        string fileName = "Result_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
        string directoryPath = Application.dataPath + "/csvFiles/";

        Debug.Log(directoryPath);

        // 디렉토리 없으면 생성
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }


        string filePath = directoryPath + fileName;

        // 파일이 없으면 파일을 생성하고 헤더를 추가합니다.
        if (!File.Exists(filePath))
        {
            string header = "걸린시간(Cost Time),선택(Select),정답(Answer),정답 여부(isRight)";
            File.WriteAllText(filePath, header + "\n", Encoding.UTF8);
        }

        string dateTimeFormat = "yy/MM/dd_HHmmss";

        // 데이터 추가
        foreach (Task task in tasks)
        {
            string line = task.costTime + "," + task.select + "," + task.answer + "," + task.isRight;
            File.AppendAllText(filePath, line + "\n", Encoding.UTF8);
        }

        Debug.Log("CSV file written at " + filePath);
    }
}