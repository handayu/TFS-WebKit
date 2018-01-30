using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TempUtilityForm
{
    public class MaintainceTask
    {
        private void CreateVarietiesDicSqlByCtp()
        {
            //StringBuilder sb = new StringBuilder();

            //List<USeProductDetail> list = USeManager.Instance.OrderDriver.QueryProductDetails();
            //foreach (USeProductDetail product in list)
            //{
            //    string str = string.Format("update alpha.varieties set long_name = '{0}',volume_multiple = {1},price_tick = {2} where code = '{3}' and exchange = '{4}';",
            //        product.LongName, product.VolumeMultiple, product.PriceTick,
            //        product.ProductCode, product.Market.ToString());
            //    sb.AppendLine(str);
            //}

            //string s = sb.ToString();
            //string ss = "";
        }
        public void CreateVarietiesDicSql()
        {
            //update alpha.varieties set long_name = '白砂糖',volume_multiple = 10,price_tick = 1 where code = 'SR' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '化纤',volume_multiple = 5,price_tick = 2 where code = 'TA' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '棉花',volume_multiple = 5,price_tick = 5 where code = 'CF' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '沥青',volume_multiple = 10,price_tick = 2 where code = 'bu' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '黄大豆1号',volume_multiple = 10,price_tick = 1 where code = 'a' and exchange = 'DCE';
            //update alpha.varieties set long_name = '鲜鸡蛋',volume_multiple = 10,price_tick = 1 where code = 'jd' and exchange = 'DCE';
            //update alpha.varieties set long_name = '燃料油',volume_multiple = 50,price_tick = 1 where code = 'fu' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '原油',volume_multiple = 1000,price_tick = 0.1 where code = 'sc' and exchange = 'INE';
            //update alpha.varieties set long_name = '白银',volume_multiple = 15,price_tick = 1 where code = 'ag' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '铝',volume_multiple = 5,price_tick = 5 where code = 'al' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '铜',volume_multiple = 5,price_tick = 10 where code = 'cu' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '热轧板',volume_multiple = 10,price_tick = 1 where code = 'hc' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '镍',volume_multiple = 1,price_tick = 10 where code = 'ni' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '铅',volume_multiple = 5,price_tick = 5 where code = 'pb' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '螺纹钢',volume_multiple = 10,price_tick = 1 where code = 'rb' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '锡',volume_multiple = 1,price_tick = 10 where code = 'sn' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '线材',volume_multiple = 10,price_tick = 1 where code = 'wr' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '锌',volume_multiple = 5,price_tick = 5 where code = 'zn' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '动力煤ZC',volume_multiple = 100,price_tick = 0.2 where code = 'ZC' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '10年国债',volume_multiple = 10000,price_tick = 0.005 where code = 'T' and exchange = 'CFFEX';
            //update alpha.varieties set long_name = '5年国债',volume_multiple = 10000,price_tick = 0.005 where code = 'TF' and exchange = 'CFFEX';
            //update alpha.varieties set long_name = '中证500股指',volume_multiple = 200,price_tick = 0.2 where code = 'IC' and exchange = 'CFFEX';
            //update alpha.varieties set long_name = '沪深300股指',volume_multiple = 300,price_tick = 0.2 where code = 'IF' and exchange = 'CFFEX';
            //update alpha.varieties set long_name = '上证50股指',volume_multiple = 300,price_tick = 0.2 where code = 'IH' and exchange = 'CFFEX';
            //update alpha.varieties set long_name = '天然橡胶',volume_multiple = 10,price_tick = 5 where code = 'ru' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '玻璃',volume_multiple = 20,price_tick = 1 where code = 'FG' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '甲醇MA',volume_multiple = 10,price_tick = 1 where code = 'MA' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '硅铁',volume_multiple = 5,price_tick = 2 where code = 'SF' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '锰硅',volume_multiple = 5,price_tick = 2 where code = 'SM' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '胶合板',volume_multiple = 500,price_tick = 0.05 where code = 'bb' and exchange = 'DCE';
            //update alpha.varieties set long_name = '纤维板',volume_multiple = 500,price_tick = 0.05 where code = 'fb' and exchange = 'DCE';
            //update alpha.varieties set long_name = '铁矿石',volume_multiple = 100,price_tick = 0.5 where code = 'i' and exchange = 'DCE';
            //update alpha.varieties set long_name = '焦炭',volume_multiple = 100,price_tick = 0.5 where code = 'j' and exchange = 'DCE';
            //update alpha.varieties set long_name = '焦煤',volume_multiple = 60,price_tick = 0.5 where code = 'jm' and exchange = 'DCE';
            //update alpha.varieties set long_name = '聚乙烯',volume_multiple = 5,price_tick = 5 where code = 'l' and exchange = 'DCE';
            //update alpha.varieties set long_name = '棕榈油',volume_multiple = 10,price_tick = 2 where code = 'p' and exchange = 'DCE';
            //update alpha.varieties set long_name = '聚丙烯',volume_multiple = 5,price_tick = 1 where code = 'pp' and exchange = 'DCE';
            //update alpha.varieties set long_name = '聚氯乙烯',volume_multiple = 5,price_tick = 5 where code = 'v' and exchange = 'DCE';
            //update alpha.varieties set long_name = '黄金',volume_multiple = 1000,price_tick = 0.05 where code = 'au' and exchange = 'SHFE';
            //update alpha.varieties set long_name = '菜籽粕',volume_multiple = 10,price_tick = 1 where code = 'RM' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '油菜籽',volume_multiple = 10,price_tick = 1 where code = 'RS' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '豆粕',volume_multiple = 10,price_tick = 1 where code = 'm' and exchange = 'DCE';
            //update alpha.varieties set long_name = '豆油',volume_multiple = 10,price_tick = 2 where code = 'y' and exchange = 'DCE';
            //update alpha.varieties set long_name = '粳稻',volume_multiple = 20,price_tick = 1 where code = 'JR' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '黄大豆2号',volume_multiple = 10,price_tick = 1 where code = 'b' and exchange = 'DCE';
            //update alpha.varieties set long_name = '玉米淀粉',volume_multiple = 10,price_tick = 1 where code = 'cs' and exchange = 'DCE';
            //update alpha.varieties set long_name = '早籼稻',volume_multiple = 20,price_tick = 1 where code = 'RI' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '强筋麦',volume_multiple = 20,price_tick = 1 where code = 'WH' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '黄玉米',volume_multiple = 10,price_tick = 1 where code = 'c' and exchange = 'DCE';
            //update alpha.varieties set long_name = '普通小麦',volume_multiple = 50,price_tick = 1 where code = 'PM' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '晚籼稻',volume_multiple = 20,price_tick = 1 where code = 'LR' and exchange = 'CZCE';
            //update alpha.varieties set long_name = '菜籽油',volume_multiple = 10,price_tick = 2 where code = 'OI' and exchange = 'CZCE';
        }

        public void WaiPanVarieties()
        {
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('GC', 'COMEX', 'CMX金', 'COMEX金', 100, 0.1);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('HG', 'COMEX', 'CMX铜', 'COMEX铜', 25000, 0.0005);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('SI', 'COMEX', 'CMX银', 'COMEX银', 5000, 0.005);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('CAD', 'LME', 'LME铜', 'LME铜', 25, 0.5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('ZSD', 'LME', 'LME锌', 'LME锌', 25, 0.5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('AHD', 'LME', 'LME铝', 'LME铝', 25, 0.5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('NID', 'LME', 'LME镍', 'LME镍', 6, 5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('SND', 'LME', 'LME锡', 'LME锡', 5, 5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('PBD', 'LME', 'LME铅', 'LME铅', 25, 0.5);

            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('TRB', 'TOCOM', 'TOCOM橡胶', 'TOCOM橡胶', 5, 0.1);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('TF', 'SGX', '标胶20', '标胶20', 5, 0.1);
        }
    }
}
