using UnityEngine;
 
 public enum SortingOrigin
{
    Pivot, Bottom, Top, Left, Right
}
public enum SortingAxis
{
    X, Y
}

[RequireComponent(typeof(Renderer))]
public class SortByYAxis : MonoBehaviour
{

    [SerializeField]
    private int _originOrder = 100;
    [SerializeField]
    public float _precision = 1f;
    [SerializeField]
    private int _offset = 0;
    [SerializeField]
    private bool _runOnlyOnce = false;
    [SerializeField]
    private SortingOrigin _sortingSelector;
    [SerializeField]
    private SortingAxis _sortingAxis = SortingAxis.Y;

    private Renderer _renderer;
    private float _timeLeft;
    private float _updateFrequency = .1f;

    private void Awake() {
        _renderer = GetComponent<Renderer>();
    }
    private void Start() {
        UpdateSortOrder();
        if (_runOnlyOnce) {
            enabled = false;
        }
    }

    private void LateUpdate() {
        UpdateSortOrder();
    }

    private void UpdateSortOrder() {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0) {
            _timeLeft = _updateFrequency;
            Vector2 pos = _renderer.bounds.center;
            float width = _renderer.bounds.size.x;
            float height = _renderer.bounds.size.y;
            switch (_sortingSelector) {
                case SortingOrigin.Bottom:
                    pos += Vector2.down * height / 2;
                    break;
                case SortingOrigin.Top:
                    pos += Vector2.up * height / 2;
                    break;
                case SortingOrigin.Left:
                    pos += Vector2.left * width / 2;
                    break;
                case SortingOrigin.Right:
                    pos += Vector2.right * width / 2;
                    break;
                default:
                    pos = transform.position;
                    break;
            }
            float posFromAxis = _sortingAxis == SortingAxis.X ? pos.x : pos.y;
            _renderer.sortingOrder = (int)(_originOrder - posFromAxis / _precision + _offset);
        }
    }
}