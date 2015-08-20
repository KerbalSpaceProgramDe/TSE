/*==============================*\
 *  TeamSpeak³ Management Bot   *
 *   KCST - Thomas P. Kerman    *
 * http://kerbalspaceprogram.de *
\*==============================*/

namespace TeamSpeakBot
{
    namespace Utility
    {
        // Versions-Klasse
        public class Version
        {
            // Versions Informationen
            private static int[] versionNumber = new int[] { 0, 0, 0, 1 };
            private static bool developmentBuild = false;
            public static string version
            {
                get
                {
                    #if DEBUG
                    developmentBuild = true;
                    #endif
                    return GetVersionNumber(versionNumber) + ((developmentBuild) ? " [Entwicklungsversion]" : "");
                }
            }

            private static string GetVersionNumber(int[] number)
            {
                string version = "";

                for (int i = 0; i < number.Length; i++)
                {
                    if (i != 0)
                        version += ".";
                    version += number[i];
                }

                return version;
            }
        }
    }
}
