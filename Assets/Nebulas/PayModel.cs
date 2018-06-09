using System;
using Newtonsoft.Json;


namespace io.nebulas.model {
    /**
     * 转账详情类
     */

    public class PayModel
    {

        public String to;       //目标地址 (*)
        public String value;    // 转账金额**  单位为wei (1NAS =10^18 wei) （*）
        public String currency; // 转账种类NAS

        public PayloadModel payload;
    }

    public class PayloadModel
    {

        public String type;      // 调用合约
        public String function;  // 合约中的方法名(*)
        public String[] args;    // 函数参数列表 (*)

    }
    public class PageParamsModel
    {

        public PayModel pay;

        public GoodsModel goods;

        public String serialNumber;

        public String callback;
    }
    public class OpenAppMode
    {

        public String category;    // 唤起类型
        public String des;         // 确认转账

        public PageParamsModel pageParams;

        public static String getOpenAppModel(OpenAppMode openAppMode)
        {
            return JsonConvert.SerializeObject(openAppMode);
        }
    }
    public class GoodsModel
    {

        public String desc;
        public String name;

    }


}
