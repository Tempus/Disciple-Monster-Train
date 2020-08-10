using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.CardEffects
{
    public class WardManager
    {
        public static List<List<WardState>> wardStates; 

        public static void ResetWards()
        {
            wardStates = new List<List<WardState>>
            {
                new List<WardState>(),
                new List<WardState>(),
                new List<WardState>(),
                new List<WardState>(),
            };
        }

        public static void AddWard(WardState ward, int floor)
        {
            if (0 <= floor && floor <= 3)
            {
                wardStates[floor].Add(ward);
                ward.OnAdd(floor);
            }
        }

    }

    public class WardState
    {
        public virtual void OnAdd(int floor)
        {

        }
    }
}
