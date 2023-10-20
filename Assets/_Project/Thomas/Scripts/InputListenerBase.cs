using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thomas
{
    public abstract class InputListenerBase : MonoBehaviour
    {
        public virtual void ProcessTouchDown(Vector2 _touchPosition) { }
        public virtual void ProcessTouchUp(Vector2 _touchPosition) { }
        public virtual void ProcessTouch(Vector2 _touchPosition) { }
        public virtual void ProcessButtonDown(Button _button) { }
    }
}