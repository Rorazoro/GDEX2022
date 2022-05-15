using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;

        [SerializeField] private string currentActionMapName = "player";

        public bool enableDebugLogs;
        public PlayerActionMap playerActionMap;
        public UIActionMap uiActionMap;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            playerActionMap = new PlayerActionMap(_playerInput);
            uiActionMap = new UIActionMap(_playerInput);
        }

        public void SwitchActionMap(string actionMapName)
        {
            _playerInput.SwitchCurrentActionMap(actionMapName);
            currentActionMapName = _playerInput.currentActionMap.name;
            if (enableDebugLogs)
                Debug.Log($"Action Map Switched: {currentActionMapName}");
        }

        private void DebugLog(InputAction.CallbackContext context)
        {
            if (enableDebugLogs)
                Debug.Log($"{context.action.name} {context.phase} : {context.action.ReadValueAsObject()}");
        }

        #region ActionMaps

        [Serializable]
        public class PlayerActionMap
        {
            private const string actionMapName = "Player";

            public PlayerActionMap(PlayerInput _playerInput)
            {
                //Storing all input actions for Player Action Map
                MoveAction = _playerInput.actions[$"{actionMapName}/Move"];
                LookAction = _playerInput.actions[$"{actionMapName}/Look"];
                SprintAction = _playerInput.actions[$"{actionMapName}/Sprint"];
                JumpAction = _playerInput.actions[$"{actionMapName}/Jump"];
                FireAction = _playerInput.actions[$"{actionMapName}/Fire"];
            }

            //Stored input actions for Player Action Map
            protected InputAction MoveAction;
            protected InputAction LookAction;
            protected InputAction SprintAction;
            protected InputAction JumpAction;
            protected InputAction FireAction;

            //Stored input actions values for Player Action Map. These are only used for viewing in the inspector.
            [SerializeField] private Vector2 move;
            [SerializeField] private Vector2 look;
            [SerializeField] private bool sprint;
            [SerializeField] private bool jump;
            [SerializeField] private bool fire;

            //Accessors for input actions for Player Action Map
            public Vector2 Move
            {
                get { return move = MoveAction.ReadValue<Vector2>(); }
            }

            public Vector2 Look
            {
                get { return look = LookAction.ReadValue<Vector2>(); }
            }

            public bool Sprint
            {
                get { return sprint = SprintAction.IsPressed(); }
            }

            public bool Jump
            {
                get { return jump = JumpAction.WasPressedThisFrame(); }
            }

            public bool JumpHold
            {
                get { return jump = JumpAction.IsPressed(); }
            }

            public bool Fire
            {
                get { return fire = FireAction.WasPressedThisFrame(); }
            }
        }

        [Serializable]
        public class UIActionMap
        {
            private const string actionMapName = "UI";

            public UIActionMap(PlayerInput _playerInput)
            {
                //Storing all input actions for UI Action Map
                NavigateAction = _playerInput.actions[$"{actionMapName}/Navigate"];
                SubmitAction = _playerInput.actions[$"{actionMapName}/Submit"];
                CancelAction = _playerInput.actions[$"{actionMapName}/Cancel"];
                PointAction = _playerInput.actions[$"{actionMapName}/Point"];
                ClickAction = _playerInput.actions[$"{actionMapName}/Click"];
                ScrollWheelAction = _playerInput.actions[$"{actionMapName}/ScrollWheel"];
                MiddleClickAction = _playerInput.actions[$"{actionMapName}/MiddleClick"];
                RightClickAction = _playerInput.actions[$"{actionMapName}/RightClick"];
            }

            //Stored input actions for UI Action Map
            protected InputAction NavigateAction;
            protected InputAction SubmitAction;
            protected InputAction CancelAction;
            protected InputAction PointAction;
            protected InputAction ClickAction;
            protected InputAction ScrollWheelAction;
            protected InputAction MiddleClickAction;
            protected InputAction RightClickAction;

            //Stored input actions values for Player Action Map. These are only used for viewing in the inspector.
            [SerializeField] Vector2 navigate;
            [SerializeField] Vector2 point;
            [SerializeField] float scrollWheel;
            [SerializeField] bool submit;
            [SerializeField] bool cancel;
            [SerializeField] bool click;
            [SerializeField] bool middleClick;
            [SerializeField] bool rightClick;

            //Accessors for input actions for Player Action Map
            public Vector2 Navigate
            {
                get { return navigate = NavigateAction.ReadValue<Vector2>(); }
            }

            public Vector2 Point
            {
                get { return point = PointAction.ReadValue<Vector2>(); }
            }

            public float ScrollWheel
            {
                get { return scrollWheel = Mathf.Clamp(ScrollWheelAction.ReadValue<Vector2>().y, -1, 1); }
            }

            public bool Submit
            {
                get { return submit = SubmitAction.WasPressedThisFrame(); }
            }

            public bool Cancel
            {
                get { return cancel = CancelAction.WasPressedThisFrame(); }
            }

            public bool Click
            {
                get { return click = ClickAction.WasPressedThisFrame(); }
            }

            public bool MiddleClick
            {
                get { return middleClick = MiddleClickAction.WasPressedThisFrame(); }
            }

            public bool RightClick
            {
                get { return rightClick = RightClickAction.WasPressedThisFrame(); }
            }
        }

        #endregion
    }
}