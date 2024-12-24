// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("qFeT1ceSn0B17QevhAeoqWEVNnUH+wfHSl9dsusqNXT4RKCo+hkGT4YF1EjMm9YS1IZhlbOSzXTRB+JPOJt/s9YAiSg+ER3weSxIq/AYMX0p/dNOKI8+xla8424nj8SPTq6A2HtRQBOILorrJOa6BPltV73M6XkxgxiUFIdxBjNnmS0uixVqDgIlgcHI+PkgfbtaLybQfsueE3CgCcdsEfKnxnofFuFcf9iQBF0QFXHgkFuJQMPNwvJAw8jAQMPDwkAbn8gNcdSdxOcBoFIe+a8QKFIEUeKeuyPG+DFt6GNb0A827fMarU53xE9NhSm18kDD4PLPxMvoRIpENc/Dw8PHwsERHipW28SGc610anV+0LuqeWChv1AOsLch2CE+dcDBw8LD");
        private static int[] order = new int[] { 6,11,10,13,13,9,8,9,12,11,11,11,12,13,14 };
        private static int key = 194;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
