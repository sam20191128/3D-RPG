using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;

namespace Pinwheel.FantasyEffect
{
    internal class CompileLogger
    {
        private const string PACKAGE_NAME = "Fantasy Effect";
        private const string PACKAGE_NAME_PLACEHOLDER = "${PACKAGE_NAME}";

        private const string WEBSITE = "http://pinwheel.studio";
        private const string WEBSITE_PLACEHOLDER = "${WEBSITE}";

        private const string PATREON = "https://www.patreon.com/tntam";
        private const string PATREON_PLACEHOLDER = "${PATREON}";

        private const float LOG_MESSAGE_PROBABIILITY = 0.02F;
        private static string[] messages = new string[]
        {
            "Thanks for using the ${PACKAGE_NAME}, please visit ${WEBSITE} for more interesting products!",
            "Thanks for using the ${PACKAGE_NAME}, if you happy with it, please consider supporting us on ${PATREON}!",
        };

        [DidReloadScripts]
        public static void ShowMessageOnCompileSucceeded()
        {
            ValidatePackageAndNamespace();
            if (Random.value<LOG_MESSAGE_PROBABIILITY)
            {
                if (messages.Length == 0)
                    return;
                int msgIndex = Random.Range(0, messages.Length);
                string msg = messages[msgIndex]
                    .Replace(PACKAGE_NAME_PLACEHOLDER, PACKAGE_NAME)
                    .Replace(WEBSITE_PLACEHOLDER, WEBSITE)
                    .Replace(PATREON_PLACEHOLDER, PATREON);
                Debug.Log(msg);
            }
        }

        private static void ValidatePackageAndNamespace()
        {
            bool isPackageNameInvalid = PACKAGE_NAME.Equals("PACKAGE_NAME");
            bool isNamespaceInvalid = typeof(CompileLogger).Namespace.Contains("PACKAGE_NAME");
            if (isPackageNameInvalid)
            {
                string message = "<color=red>Invalid PACKAGE_NAME in CompileLogger, fix it before release!</color>";
                Debug.Log(message);
            }
            if(isNamespaceInvalid)
            {
                string message = "<color=red>Invalid NAMESPACE in CompileLogger, fix it before release!</color>";
                Debug.Log(message);
            }
        }
    }
}