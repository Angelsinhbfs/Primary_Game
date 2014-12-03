using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Core
{

    public class PrimaryEnums
    {
        public enum AiState
        {
            Wander,
            Follow,
            Attack
        }

        public enum Color
        {
            Yellow  = 0,
            Red = 1,
            Blue = 2
        }

        public enum LockOn
        {
            PlayerOne,
            PlayerTwo,
            None
        }
        public enum FireDir
        {
            Front,
            Left,
            Right
        }

        
    }
}
