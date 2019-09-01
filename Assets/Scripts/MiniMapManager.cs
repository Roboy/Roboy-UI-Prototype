﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    public enum MiniMapTechnique {YAH, PointCloud};

    public MiniMapTechnique currentMinimap = MiniMapTechnique.YAH;

    public GameObject YAH_Minimap;
    public GameObject Pointcloud_Minimap;
    public PC_MeshManager MeshManager;

    private GameObject ROSManager;

    // Start is called before the first frame update
    void Start()
    {
        ROSManager = GameObject.FindWithTag("ROSManager");
    }

    // Update is called once per frame
    void Update()
    {
        // Switch MiniMap
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchMinimap();
        }
    }

    public void SwitchMinimap()
    {
        switch (currentMinimap)
        {
            case MiniMapTechnique.YAH:
                SwitchToPointCloud();
                break;
            case MiniMapTechnique.PointCloud:
                SwitchToYAH();
                break;
        }
    }

    public void SwitchToYAH()
    {
        Debug.Log("Switch Minimap to YAH");
        currentMinimap = MiniMapTechnique.YAH;
        Pointcloud_Minimap.SetActive(false);
        ROSManager.GetComponent<PointCloudSubscriber>().messageProcessingActive = false;
        MeshManager.messageProcessingActive = false;
        MeshManager.removeMeshes();
        YAH_Minimap.SetActive(true);
    }

    public void SwitchToPointCloud()
    {
        Debug.Log("Switch Minimap to PC");
        currentMinimap = MiniMapTechnique.PointCloud;
        YAH_Minimap.SetActive(false);
        ROSManager.GetComponent<PointCloudSubscriber>().messageProcessingActive = true;
        MeshManager.messageProcessingActive = true;
        Pointcloud_Minimap.SetActive(true);
    }

}