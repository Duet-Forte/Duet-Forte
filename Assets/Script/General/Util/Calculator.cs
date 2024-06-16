


namespace Util
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class Calculator
    {
        public int CalcMaxExp(int currentLevel)
        {

            return ((currentLevel + 1) * (currentLevel + 1) * (currentLevel + 1)) - (currentLevel * currentLevel * currentLevel);


        }

    }
}
