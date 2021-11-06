using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BeehiveManagementSystem
{
    static class HoneyVault
    {
        public const float NECTAR_CONVERSION_RATIO = 0.3f;  // Very important. It decides how fast the honey (life source) is generate 
        public const float LOW_LEVEL_WARNING = 10f;
        private static float honey = 25f;
        private static float nectar = 100f;

        public static void ConvertNectarToHoney(float amount)
        {
            if(amount > nectar)
            {
                nectar = 0;
                honey += nectar * NECTAR_CONVERSION_RATIO;
            }
            else
            {
                nectar -= amount;
                honey += amount * NECTAR_CONVERSION_RATIO;
            }
        }
        public static bool ConsumeHoney(float amount)
        {
            if (amount <= honey)
            {
                honey -= amount;
                return true;
            }
            else return false;
        }
        public static void CollectNectar(float amount)
        {
            if (amount > 0f) nectar += amount;
        }
        public static string StatusReport // Chu y ki thuat o khoi nay
        {
            get
            {
                string status = $"{honey:0.0} units of honey \n{nectar:0.0} units of nectar\n";
                string warnings = "";
                if (honey < LOW_LEVEL_WARNING) warnings += "\nLOW HONEY - ADD A HONEY MANUFACTURER";
                if (nectar < LOW_LEVEL_WARNING) warnings += "\nLOW NECTAR - ADD A NECTAR MANUFACTURER";
                return status + warnings;

            }
        }
    }
}
