using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public bool isGamePlay;
    public UIManager uiManager;
    
    [SerializeField] private float gameMaxTime;
    [SerializeField] private float gameStartTime;
    
    [SerializeField] private SaveData dataSaveSystem;
    [SerializeField] private Transform[] boxTransformList;
    [SerializeField] private int boxIndex;
    [SerializeField] private GameObject molePrefab;
    [SerializeField] private float moleAppearSpeed;
    public float timeForMoleLive;
    public int pointAll;
    public int pointPerHit;
    
    private GameObject currentMole;
    private Vector3 currentTarget;

    private float previousTime;
    private IEnumerator moveMoleWithDelay;

    private void Awake()
    {
        GM = this;
    }

    void Start()
    {
        boxIndex = 0;
        currentMole = Instantiate(molePrefab);
        currentMole.SetActive(false);
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGamePlay)
        {
            if(gameMaxTime < Time.time-gameStartTime)
            {
                GameOver();
                return;
            }
            uiManager.UpdateTimer(gameMaxTime-Time.time);
            if (!currentMole.activeInHierarchy)
            {
                boxIndex = Random.Range(0, boxTransformList.Length);
                currentMole.transform.parent = boxTransformList[boxIndex];
                
                currentMole.transform.localPosition = Vector3.zero;
                currentMole.transform.localPosition += Vector3.up*0.75f;
                //currentMole.transform.rotation = Quaternion.identity;
                var rendererList = currentMole.GetComponentsInChildren<MeshRenderer>();
                foreach (var meshRenderer in rendererList)
                {
                    meshRenderer.enabled = true;
                }
                currentMole.GetComponent<CapsuleCollider>().enabled = true;
                currentMole.SetActive(true);
            }
        
            if (Input.GetMouseButtonDown(0)) 
            {  
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
                RaycastHit hit;  
                if (Physics.Raycast(ray, out hit)) 
                {  
                    //Select stage    
                    if (hit.transform.CompareTag("Mole"))
                    {
                        pointAll += pointPerHit;
                        uiManager.UpdatePoint(pointAll);
                        dataSaveSystem.SavePositionData(currentMole.transform.parent.position);
                        dataSaveSystem.SaveTimeData(DateTime.Now.ToString());
                    
                        var rendererList = currentMole.GetComponentsInChildren<MeshRenderer>();
                        foreach (var meshRenderer in rendererList)
                        {
                            meshRenderer.enabled = false;
                        }
                        currentMole.GetComponent<CapsuleCollider>().enabled = false;
                        var coroutine = MoveMoleToBox();
                        StartCoroutine(coroutine);
                    }  
                }  
            }

            if (Time.time - previousTime > timeForMoleLive)
            {
                var rendererList = currentMole.GetComponentsInChildren<MeshRenderer>();
                foreach (var meshRenderer in rendererList)
                {
                    meshRenderer.enabled = false;
                }
                currentMole.GetComponent<CapsuleCollider>().enabled = false;
                var coroutine = MoveMoleToBox();
                StartCoroutine(coroutine);
            }
        }
    }

    public void StartGame()
    {
        gameStartTime = Time.time;
    }
    private void GameOver()
    {
        currentMole.SetActive(false);
        dataSaveSystem.SavePointData(pointAll.ToString());
        dataSaveSystem.SaveAllData();
        isGamePlay = false;
        gameStartTime = Time.time;
        uiManager.GameOver();
    }

    private void OnApplicationQuit()
    {
        GameOver();
    }

    IEnumerator MoveMoleToBox()
    {
        yield return new WaitForSeconds(moleAppearSpeed);
        currentMole.SetActive(false);
        previousTime = Time.time;
    }
}
