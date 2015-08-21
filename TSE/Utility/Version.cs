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
            public static string GetVersion()
            {
                string version = "";
                for (int i = 0; i < versionNumber.Length; i++)
                {
                    if (i != 0)
                        version += ".";
                    version += versionNumber[i];
                }
                #if DEBUG
                return version + " [Entwicklungsversion]";
                #else
                return version;
                #endif
            }
        }
    }
}
