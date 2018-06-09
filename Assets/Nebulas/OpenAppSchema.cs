/**
 * 打开调启App
 */
namespace io.nebulas.schema
{
    public class OpenAppSchema
    {

        /**
         * 获取schema url
         * @param paramsJSON json 字符串
         * @return schemaurl
         */
        public static string getSchemaUrl(string paramsJSON)
        {
            return string.Format("openapp.nasnano://virtual?params={0}", paramsJSON);
        }
    }
}
