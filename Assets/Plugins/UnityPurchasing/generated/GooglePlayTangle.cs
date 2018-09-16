#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("K5p8T7W6nYJcPe4dJQDS8ti8Z2EcihSEdYri1TtZ75lnKP+1+/bM/rO3PX2IgqdtOCASRjRInM/jpYsFH5ySna0fnJefH5ycnR0YG3VKTAxivRfENv+f1ztP6y/OGQWQhrFlEsAenOkfkZE/ybvnrBLdlumAYyR0rR+cv62Qm5S3G9UbapCcnJyYnZ6Q2A7tzygMAK59WIEddXOaBtSzhNIHKZSrBfZ1IboMfyQ5+sX/SotNMqpP+8pG1i4VGZjVJrRRR2T7SmOb8HmgJKNvjq3PXBHUNb6dlSj/jMVYs059LaC4rQaeIy9M5RxbnYNqYs7uqb13WJMd27V4DUKuDEce+0mhzIzf00+IvI0OAesurZ1vnEHArsM7elsYIzftsJ+enJ2c");
        private static int[] order = new int[] { 13,10,8,10,6,8,13,13,9,9,10,11,12,13,14 };
        private static int key = 157;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
