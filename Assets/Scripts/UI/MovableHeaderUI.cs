using UnityEngine.EventSystems;
using UnityEngine;

public class MovableHeaderUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private Transform _targetTr; // �̵��� UI

    private Vector2 _beginPoint;
    private Vector2 _moveBegin;

    private void Awake()
    {
        // �̵� ��� UI�� �������� ���� ���, �ڵ����� �θ�� �ʱ�ȭ
        if (_targetTr == null)
            _targetTr = transform.parent;
    }

    // �巡�� ���� ��ġ ����
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _beginPoint = _targetTr.position;
        _moveBegin = eventData.position;
    }

    // �巡�� : ���콺 Ŀ�� ��ġ�� �̵�
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 targetPos = _beginPoint + (eventData.position - _moveBegin);

        if (eventData.position.x > Screen.width - 20 || eventData.position.y > Screen.height || eventData.position.x < 20 || eventData.position.y < 20)
            return;

        _targetTr.position = _beginPoint + (eventData.position - _moveBegin);
    }
}