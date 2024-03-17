using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    private Vector2Int _limitMin;
    private Vector2Int _limitMax;
    private Vector2Int _limit;
    [SerializeField]private float Speed;
    private void Start()
    {
        _limit.x = Screen.width / 10;
        _limit.y = Screen.height / 10;
        
        _limitMin.x = Screen.width - Screen.width + _limit.x;
        _limitMin.y = Screen.height - Screen.height + _limit.y;
        _limitMax.x = Screen.width - _limit.x;
        _limitMax.y = Screen.height - _limit.y;
    }

    private void LateUpdate()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        if (Input.mousePosition.x < _limitMin.x && Input.mousePosition.x > 0)
        {
            transform.position += Vector3.left * (Speed * Time.deltaTime);
        }
        else if(Input.mousePosition.x > _limitMax.x && Input.mousePosition.x < Screen.width)
        {
            transform.position -= Vector3.left * (Speed * Time.deltaTime);
        }

        if (Input.mousePosition.y < _limitMin.y && Input.mousePosition.y > 0 )
        {
            transform.position += Vector3.back * (Speed * Time.deltaTime);
        }
        else if (Input.mousePosition.y > _limitMax.y && Input.mousePosition.y < Screen.height)
        {
            transform.position -= Vector3.back * (Speed * Time.deltaTime);
        }
    }
}
