using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UserInput
{
    public enum KeyBoardOptions
    {
        Alphabetical,
        Numerical,
        Function,
        Punctuation,
        Any
    }

    /// <summary>
    /// Custom comparer for lists of KeyCodes
    /// </summary>
    public class KeyCodeComparer : IEqualityComparer<List<KeyCode>>
    {
        /// <summary>
        /// Checks that all occurrences are equal in both lists, rather than checking reference locations
        /// </summary>
        /// <param name="x">LHS list of keycodes</param>
        /// <param name="y">RHS list of keycodes</param>
        /// <returns>Bool to return</returns>
        public bool Equals(List<KeyCode> x, List<KeyCode> y)
        {
            if (x == null || y == null) return false;
            return x.All(y.Contains) && y.All(x.Contains);
        }

        /// <summary>
        /// Makes a hashcode generated from the contents
        /// </summary>
        /// <param name="obj">list of keycodes</param>
        /// <returns>int hashcode</returns>
        public int GetHashCode(List<KeyCode> obj)
        {
            int hCode = obj.Sum(x => (int)x);
            return hCode.GetHashCode();
        }
    }


    public class KeyCodeCombinationComparer : IEqualityComparer<Tuple<List<KeyCode>, KeyCode>>
    {
        public bool Equals(Tuple<List<KeyCode>, KeyCode> x, Tuple<List<KeyCode>, KeyCode> y)
        {
            if (x == null || y == null) return false;
            return (x.Item1.All(y.Item1.Contains) && y.Item1.All(x.Item1.Contains)) && (x.Item2 == y.Item2);
        }

        public int GetHashCode(Tuple<List<KeyCode>, KeyCode> obj)
        {
            int hCode = obj.Item1.Sum(x => (int)x) + (int)obj.Item2;
            return hCode.GetHashCode();
        }
    }

    public class KeyListener : MonoBehaviour
    {
        // All non-mouse/joystick keydowns
        [SerializeField] private KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Where(k => ((int)k < (int)KeyCode.Mouse0))
            .ToArray();

        private KeyCodeComparer _customComparer;
        private KeyCodeCombinationComparer _customCombinationComparer;
        private Dictionary<List<KeyCode>, UnityEvent<List<KeyCode>>> _subscribedKeyEvents;
        private Dictionary<Tuple<List<KeyCode>, KeyCode>, UnityEvent<Tuple<List<KeyCode>, KeyCode>>> _subscribedKeyCombinationEvents;

        private List<KeyCode> _keysDown;
        private List<KeyCode> _keysUp;

        public void Awake()
        {
            InitSubscribedKeyEvents();
            InitSubscribedKeyCombinationEvents();
        }

        public void Start()
        {
            InitSubscribedKeyEvents();
            InitSubscribedKeyCombinationEvents();
        }

        /// <summary>
        /// Resets the list of keys down
        /// </summary>
        public void OnEnable()
        {
            _keysDown = new List<KeyCode>();
            _keysUp = new List<KeyCode>();
        }

        /// <summary>
        /// Nulls the keydown list
        /// </summary>
        public void OnDisable()
        {
            _keysDown = null;
            _keysUp = null;
        }

        /// <summary>
        /// Catches all pressed keys and adds them to the stored keydown list.
        /// Catches all keypress releases and invokes the callback for all released keypresses
        /// </summary>
        public void Update()
        {
            // Catch all keydowns
            CatchKeyDowns();

            // Catch all keyups
            ProcessKeyDowns();
        }

        private void ProcessKeyDowns()
        {
            if (_keysDown.Count <= 0) return;
            
            _keysUp = new List<KeyCode>();
            for (var i = 0; i < _keysDown.Count; i++)
            {
                var kc = _keysDown[i];
                
                if (!Input.GetKeyUp(kc)) continue;
                
                _keysDown.RemoveAt(i);
                i--;
                _keysUp.Add(kc);
            }

            for (var i = 0; i < _keysUp.Count; i++)
            {
                if (ExecuteKeyCombinationCallback(_keysDown, _keysUp[i]))
                {
                    _keysUp.RemoveAt(i);
                }
            }

            // Invoke callbacks with all keyups
            ExecuteKeyCallback(_keysUp);
        }

        private void CatchKeyDowns()
        {
            if (!Input.anyKeyDown) return;
            foreach (var kc in keyCodes)
            {
                if (Input.GetKeyDown(kc))
                {
                    _keysDown.Add(kc);
                }
            }
        }


        private void InitSubscribedKeyEvents()
        {
            if (_customComparer == null)
            {
                _customComparer = new KeyCodeComparer();
            }
            if (_subscribedKeyEvents == null)
            {
                _subscribedKeyEvents = new Dictionary<List<KeyCode>, UnityEvent<List<KeyCode>>>(_customComparer);
            }
        }
        private void InitSubscribedKeyCombinationEvents()
        {
            if (_customCombinationComparer == null)
            {
                _customCombinationComparer = new KeyCodeCombinationComparer();
            }
            if (_subscribedKeyCombinationEvents == null)
            {
                _subscribedKeyCombinationEvents = new Dictionary<Tuple<List<KeyCode>, KeyCode>, UnityEvent<Tuple<List<KeyCode>, KeyCode>>>(_customCombinationComparer);
            }
        }

        /// <summary>
        /// Adds a UnityAction mapped to a list of common keypresses
        /// </summary>
        /// <param name="key">The combination of keys pressed in order to fire the callback</param>
        /// <param name="callback">The UnityAction to invoke after the required keys are pressed</param>
        /// <returns>bool whether the adding is successful. A false indicates that the keys are already in use.</returns>
        public bool AddKey(List<KeyCode> key, UnityAction<List<KeyCode>> callback)
        {
            if (!key.Any() || callback == null) return false;
            if (_subscribedKeyEvents == null) InitSubscribedKeyEvents();
            if (!_subscribedKeyEvents.ContainsKey(key)) _subscribedKeyEvents.Add(key, new UnityEvent<List<KeyCode>>());

            _subscribedKeyEvents[key].AddListener(callback);
            return true;
        }

        public bool AddKeyCombination(Tuple<List<KeyCode>, KeyCode> combination, UnityAction<Tuple<List<KeyCode>, KeyCode>> callback)
        {
            if (combination.Item1 == null || callback == null) return false;
            if (_subscribedKeyCombinationEvents == null) InitSubscribedKeyCombinationEvents();
            if (!_subscribedKeyCombinationEvents.ContainsKey(combination)) _subscribedKeyCombinationEvents.Add(combination, new UnityEvent<Tuple<List<KeyCode>, KeyCode>>());

            _subscribedKeyCombinationEvents[combination].AddListener(callback);
            return true;
        }



        /// <summary>
        /// Adds a number key combinations that are grouped together.
        /// </summary>
        /// <param name="option">The KeyBoardOption enum</param>
        /// <param name="callback">The UnityAction to invoke after the required keys are pressed</param>
        /// <returns></returns>
        public bool AddOption(KeyBoardOptions option, UnityAction<List<KeyCode>> callback)
        {
            var keys = new List<KeyCode>();
            switch (option)
            {
                case KeyBoardOptions.Alphabetical:
                    keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => (int)k >= (int)KeyCode.A && (int)k <= (int)KeyCode.Z).ToArray());
                    break;

                case KeyBoardOptions.Numerical:
                    // No keypad numerals because they don't cast to the right ascii values.
                    keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => (int)k >= (int)KeyCode.Alpha0 && (int)k <= (int)KeyCode.Alpha9).ToArray());
                    break;

                case KeyBoardOptions.Function:
                    keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(k => (int)k >= (int)KeyCode.F1 && (int)k <= (int)KeyCode.F15).ToArray());
                    break;

                case KeyBoardOptions.Punctuation:
                    keys.Add(KeyCode.Period);
                    keys.Add(KeyCode.KeypadPeriod);
                    keys.Add(KeyCode.Comma);
                    keys.Add(KeyCode.Slash);
                    keys.Add(KeyCode.Backslash);
                    keys.Add(KeyCode.Colon);
                    keys.Add(KeyCode.Semicolon);
                    break;

                case KeyBoardOptions.Any:
                    keys.AddRange(Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>());
                    break;

                default:
                    throw new NotImplementedException();
            }

            // Check if all keys are added successfully
            var returnValue = true;
            foreach (KeyCode k in keys)
            {
                if (!AddKey(new List<KeyCode> {k}, callback))
                {
                    returnValue = false;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Clears all actions from each key. DOT NOT USE LIGHTLY.
        /// </summary>
        public void ClearActions()
        {
            foreach (var entry in _subscribedKeyEvents)
            {
                entry.Value.RemoveAllListeners();
            }
        }

        /// <summary>
        /// Removes a listener from each keycode list.
        /// </summary>
        /// <param name="listener">Listener to remove</param>
        public void ClearActions(UnityAction<List<KeyCode>> listener)
        {
            if (listener == null) return;
            foreach (var entry in _subscribedKeyEvents)
            {
                entry.Value.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Removes delegated listeners.
        /// </summary>
        /// <param name="args">List of keycodes to match</param>
        public void ClearActions(List<KeyCode> args)
        {
            if (args != null)
            {
                _subscribedKeyEvents[args].RemoveAllListeners();
            }
        }

        /// <summary>
        /// Removes a specific listener from a specific keycode list
        /// </summary>
        /// <param name="args">List of keycodes mapped to the listener</param>
        /// <param name="listener">Listener method to remove</param>
        public void ClearActions(List<KeyCode> args, UnityAction<List<KeyCode>> listener)
        {
            if (args != null && listener != null)
            {
                _subscribedKeyEvents[args].RemoveListener(listener);
            }
        }

        /// <summary>
        /// Returns an event matched by the keypair
        /// </summary>
        /// <param name="key">List of keycodes</param>
        /// <returns></returns>
        public UnityEvent<List<KeyCode>> GETEvent(List<KeyCode> key)
        {
            return !_subscribedKeyEvents.ContainsKey(key) ? null : _subscribedKeyEvents[key];
        }

        /// <summary>
        /// Invokes the events and all delegates for the given list of keycodes
        /// </summary>
        /// <param name="key">List of keycodes</param>
        public bool ExecuteKeyCallback(List<KeyCode> key)
        {
            if (!_subscribedKeyEvents.ContainsKey(key)) return false;
            _subscribedKeyEvents[key].Invoke(key);
            return true;
        }

        public bool ExecuteKeyCombinationCallback(List<KeyCode> pressed, KeyCode release)
        {
            if (pressed == null) return false;

            Tuple<List<KeyCode>, KeyCode> key = new Tuple<List<KeyCode>, KeyCode>(pressed, release);
            if (_subscribedKeyCombinationEvents.ContainsKey(key))
            {
                _subscribedKeyCombinationEvents[key].Invoke(key);
                return true;
            }
            return false;

        }
    }
}