using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GridManager _gridManager;

    private float _updateDelay;

    private Transform _transform;

    private void Awake() {
        _transform = this.transform;
        _updateDelay = _gridManager.updateDelay;
    }
    
    private void Start() {
        StartCoroutine(FollowGrid());
    }
    private IEnumerator FollowGrid()
    {
        while( true )
        {
            _transform.position += new Vector3(0f, -1f, 0f);
            yield return new WaitForSeconds(_updateDelay);
        }
    }
}
