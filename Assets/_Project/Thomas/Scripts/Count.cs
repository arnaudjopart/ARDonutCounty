using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Thomas
{
    [CreateAssetMenu(menuName = "ScriptableObject/Count")]
    public class Count : ScriptableObject
    {
        public int count;

        public void CountUp(int _value) { count += _value; }
    }
}
