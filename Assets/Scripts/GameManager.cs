using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    [SerializeField] private GridManager _gridManager;  

    private void Awake() {
        Application.targetFrameRate = 60;
        // QualitySettings.vSyncCount = 1;
    }
}
