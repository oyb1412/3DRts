using UnityEngine;

public class UIHpBar : MonoBehaviour
{
    private IHit _parent;
    private SpriteRenderer _renderer;
    private Transform _original;
    private float _originalSizeX;
    private float _originalSizeY;
    private Collider _col;
    private void Start()
    {
        _original = transform.root;
        _renderer = GetComponent<SpriteRenderer>();
        _parent = GetComponentInParent<IHit>();
        _col = _original.GetComponent<Collider>();
        _originalSizeX = _renderer.size.x;
        _originalSizeY = _renderer.size.y;
        _parent.OnHpEvent += (hp => { _renderer.size = new Vector2(hp / 100 * _originalSizeX, _originalSizeY); });
        _parent.DeleteHpBarEvent += () => Destroy(this);
        GameObject go = GameObject.Find("@HpBar_root");
        if (go == null)
        {
            go = new GameObject("@HpBar_root");
        }
        transform.parent = go.transform;
    }

    private void LateUpdate()
    {
        transform.position= _original.transform.position + new Vector3(-_originalSizeX /2,0f,-_col.bounds.size.z / 2);
    }
}
