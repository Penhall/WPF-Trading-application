using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Global
{
    public class UtilityVoiceFeedback
    {
        public static bool EnableVoiceFeedback;

        private static SpeechSynthesizer oUtilityVoiceFeedback;

        public static SpeechSynthesizer GetInstance
        {
            get
            {
                if (oUtilityVoiceFeedback == null)
                {
                    oUtilityVoiceFeedback = new SpeechSynthesizer();
                }
                return oUtilityVoiceFeedback;
            }

        }

    }
}
