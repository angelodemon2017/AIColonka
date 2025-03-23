// Comment this line to disable usage of XR module
#define ENABLE_XR

using System.Collections.Generic;
using UnityEngine;

#if ENABLE_XR
using UnityEngine.XR;
#endif

namespace BeautifyEffect {

    static class VRCheck {

        public static bool isActive;
        public static bool isVrRunning;

#if !ENABLE_XR
            static bool IsActive() {
                return false;
            }

            static bool IsVrRunning() {
                return false;
            }

#endif

    }
}