using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float rotateSpeed = 20f;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GridManager _gridManager;

    private float _updateDelay;

    private Transform _transform;

    private void Awake() {
        _transform = this.transform;
        _updateDelay = _gridManager.updateDelay;
    }

    private void Start() 
    {
        SetStartingPosition();
        StartCoroutine(FollowGrid());
    }

    void FixedUpdate()
    {
        _transform.Rotate(0f, rotateSpeed * Time.fixedDeltaTime, 0f);
        
    }

    private IEnumerator FollowGrid()
    {
        WaitForSeconds delay = new WaitForSeconds(_updateDelay);
        while( true )
        {
            _transform.position += Vector3.up;
            yield return delay;
        }
    }

    private void SetStartingPosition()
    {
        float startingX = (float) GridManager.HORIZONTAL_CELL_AMOUNT / 2f;
        float startingY = _transform.position.y;
        float startingZ = (float) GridManager.VERTICAL_CELL_AMOUNT / 2f;
        Vector3 startingPosition = new Vector3(startingX, startingY, startingZ);

        _transform.position = startingPosition;
    }
}
