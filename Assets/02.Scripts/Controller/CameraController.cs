using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2Int _limitMin;
    private Vector2Int _limitMax;
    private int _limit;
    private float _speed;
    private void Start()
    {
        _limitMin.x = Screen.width - Screen.width + _limit;
        _limitMin.y = Screen.height - Screen.height + _limit;
        
        _limitMax.x = Screen.width - _limit;
        _limitMax.y = Screen.height - _limit;
    }

    private void LateUpdate()
    {
        //SetPosition();
    }

    private void SetPosition()
    {
        if (Input.mousePosition.x < _limitMin.x)
        {
            transform.position += Vector3.left * (_speed * Time.deltaTime);
        }
        else if(Input.mousePosition.x > _limitMax.x)
        {
            transform.position -= Vector3.left * (_speed * Time.deltaTime);
        }

        if (Input.mousePosition.y < _limitMin.y)
        {
            transform.position += Vector3.back * (_speed * Time.deltaTime);
        }
        else if (Input.mousePosition.y > _limitMax.y)
        {
            transform.position -= Vector3.back * (_speed * Time.deltaTime);
        }
    }
}
