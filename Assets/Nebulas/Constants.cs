
using System;
/**
* 星云常量
*/
namespace io.nebulas {
    public class Constants
    {

        public const int RANDOM_LENGTH = 32;

        public const String CATEGORY = "jump";

        public const String DESCRIPTION = "confirmTransfer";

        public const String CALL_BACK = "https://pay.nebulas.io/api/pay";

        public const String PAY_CURRENCY = "NAS";

        public const String PAY_PAYLOAD_TYPE = "binary";

        public const String CALL_PAYLOAD_TYPE = "call";

        public const String CALL_PAYLOAD_FUNCTION = "set"; //合约中的方法名

        public const String NAS_NANO_PACKAGE_ANDROID_NAME = "io.nebulas.wallet.android";//nas nano package name

}
}