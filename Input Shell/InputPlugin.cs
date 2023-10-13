using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.InputShell
{
    public class InputPlugin : MonoBehaviour
    {
        [NonSerialized] public BaseInput ActiveInput;

        public T GetInput<T>() where T : BaseInput
        {
            return (T)ActiveInput;
        }

        public virtual void InputEnabled()
        {

        }

        public virtual void InputDisabled()
        {

        }

        public virtual void InputUpdate()
        {

        }
    }
}
