using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInputNamespace
{
    public class Joystick : MonoBehaviour, ISimpleInputDraggable
    {
        public event System.Action onTouchOneTime;
        public event System.Action<Vector2> onDragged;
        public event System.Action onPointerDown;
        public event System.Action onPointerUp;
        public enum MovementAxes { XandY, X, Y };

        public SimpleInput.AxisInput xAxis = new SimpleInput.AxisInput("Horizontal");
        public SimpleInput.AxisInput yAxis = new SimpleInput.AxisInput("Vertical");
        private RectTransform joystickTR;
        private Graphic background;
        public MovementAxes movementAxes = MovementAxes.XandY;
        public float valueMultiplier = 1f;
#pragma warning disable 0649
        [SerializeField]
        private Image thumb;
        private RectTransform thumbTR;

        [SerializeField] private float movementAreaRadius = 75f;
        [Tooltip("Radius of the deadzone at the center of the joystick that will yield no input")]
        [SerializeField] private float deadzoneRadius;
        [SerializeField] private bool isDynamicJoystick = false;
        [SerializeField] private RectTransform dynamicJoystickMovementArea;

        [SerializeField] private bool canFollowPointer = false;
#pragma warning restore 0649

        private bool joystickHeld = false;
        private Vector2 pointerInitialPos;
        private float _1OverMovementAreaRadius;
        private float movementAreaRadiusSqr;
        private float deadzoneRadiusSqr;
        private Vector2 joystickInitialPos;
        private float opacity = 1f;
        private Vector2 m_value = Vector2.zero;
        private bool _isDragged;
        private bool _stopped = false;

        public Vector2 DragDelta { get; private set; }
        public bool IsPressed => joystickHeld;
        public Vector2 Value { get { return m_value; } }


        private void Awake()
        {
            joystickTR = (RectTransform)transform;
            thumbTR = thumb.rectTransform;
            background = GetComponent<Graphic>();

            if (isDynamicJoystick)
            {
                opacity = 0f;
                thumb.raycastTarget = false;
                if (background)
                    background.raycastTarget = false;

                OnUpdate();
            }
            else
            {
                thumb.raycastTarget = true;
                if (background)
                    background.raycastTarget = true;
            }

            _1OverMovementAreaRadius = 1f / movementAreaRadius;
            movementAreaRadiusSqr = movementAreaRadius * movementAreaRadius;
            deadzoneRadiusSqr = deadzoneRadius * deadzoneRadius;

            joystickInitialPos = joystickTR.anchoredPosition;
            thumbTR.localPosition = Vector3.zero;
        }
        private void Start()
        {
            SimpleInputDragListener eventReceiver;
            if (!isDynamicJoystick)
            {
                if (background)
                    eventReceiver = background.gameObject.AddComponent<SimpleInputDragListener>();
                else
                    eventReceiver = thumbTR.gameObject.AddComponent<SimpleInputDragListener>();
            }
            else
            {
                if (!dynamicJoystickMovementArea)
                {
                    dynamicJoystickMovementArea = new GameObject("Dynamic Joystick Movement Area", typeof(RectTransform)).GetComponent<RectTransform>();
                    dynamicJoystickMovementArea.SetParent(thumb.canvas.transform, false);
                    dynamicJoystickMovementArea.SetAsFirstSibling();
                    dynamicJoystickMovementArea.anchorMin = Vector2.zero;
                    dynamicJoystickMovementArea.anchorMax = Vector2.one;
                    dynamicJoystickMovementArea.sizeDelta = Vector2.zero;
                    dynamicJoystickMovementArea.anchoredPosition = Vector2.zero;
                }

                eventReceiver = dynamicJoystickMovementArea.gameObject.AddComponent<SimpleInputDragListener>();
            }

            eventReceiver.Listener = this;
        }
        private void OnEnable()
        {
            xAxis.StartTracking();
            yAxis.StartTracking();
            SimpleInput.OnUpdate += OnUpdate;
        }
        private void OnDisable()
        {
            OnPointerUp(null);

            xAxis.StopTracking();
            yAxis.StopTracking();

            SimpleInput.OnUpdate -= OnUpdate;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            _1OverMovementAreaRadius = 1f / movementAreaRadius;
            movementAreaRadiusSqr = movementAreaRadius * movementAreaRadius;
            deadzoneRadiusSqr = deadzoneRadius * deadzoneRadius;
        }
#endif

        public void OnPointerDown(PointerEventData eventData)
        {
            _stopped = false;
            onPointerDown?.Invoke();
            if (onTouchOneTime != null)
            {
                onTouchOneTime();
                onTouchOneTime = null;
            }
            joystickHeld = true;

            if (isDynamicJoystick)
            {
                pointerInitialPos = Vector2.zero;

                Vector3 joystickPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(dynamicJoystickMovementArea, eventData.position, eventData.pressEventCamera, out joystickPos);
                joystickTR.position = joystickPos;
            }
            else
                RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTR, eventData.position, eventData.pressEventCamera, out pointerInitialPos);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (_stopped) return;
            
            _isDragged = true;
            DragDelta = eventData.delta;
            onDragged?.Invoke(DragDelta);
            Vector2 pointerPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTR, eventData.position, eventData.pressEventCamera, out pointerPos);

            Vector2 direction = pointerPos - pointerInitialPos;
            if (movementAxes == MovementAxes.X)
                direction.y = 0f;
            else if (movementAxes == MovementAxes.Y)
                direction.x = 0f;

            if (direction.sqrMagnitude <= deadzoneRadiusSqr)
                m_value.Set(0f, 0f);
            else
            {
                if (direction.sqrMagnitude > movementAreaRadiusSqr)
                {
                    Vector2 directionNormalized = direction.normalized * movementAreaRadius;
                    if (canFollowPointer)
                        joystickTR.localPosition += (Vector3)(direction - directionNormalized);

                    direction = directionNormalized;
                }

                m_value = direction * _1OverMovementAreaRadius * valueMultiplier;
            }

            thumbTR.localPosition = direction;

            xAxis.value = m_value.x;
            yAxis.value = m_value.y;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            joystickHeld = false;
            m_value = Vector2.zero;
            onPointerUp?.Invoke();
            thumbTR.localPosition = Vector3.zero;
            if (!isDynamicJoystick && canFollowPointer)
                joystickTR.anchoredPosition = joystickInitialPos;

            xAxis.value = 0f;
            yAxis.value = 0f;
            _stopped = false;
        }
        public void ForceStop()
        {
            _isDragged = false;
            DragDelta = Vector2.zero;
            m_value.Set(0f, 0f);
            xAxis.ResetValue();
            yAxis.ResetValue();
            _stopped = true;
        }
        private void OnUpdate()
        {
            if (!isDynamicJoystick)
                return;
            if (_isDragged)
            {
                _isDragged = false;
            }
            else
            {
                DragDelta = Vector2.zero;
            }
            if (joystickHeld)
                opacity = Mathf.Min(1f, opacity + Time.unscaledDeltaTime * 4f);
            else
                opacity = Mathf.Max(0f, opacity - Time.unscaledDeltaTime * 4f);

            Color c = thumb.color;
            c.a = opacity;
            thumb.color = c;

            if (background)
            {
                c = background.color;
                c.a = opacity;
                background.color = c;
            }
        }
    }
}