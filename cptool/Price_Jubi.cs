using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cptool
{
    public class Info
    {
        public Info(string key, string desc)
        {
            this.key = key;
            this.desc = desc;
            this.updatetime = DateTime.Now;
            this.price = 0;
            this.buy = 0;
            this.sell = 0;
            this.vol = 0;
            this.change = false;
        }
        public string key;
        public string desc;
        public DateTime updatetime;
        public double price;
        public double buy;
        public double sell;
        public double vol;
        public bool change = false;
        public bool active = false;
        public Info Clone()
        {
            Info i = new Info(this.key, this.desc);

            i.updatetime = DateTime.Now;
            i.price = price;
            i.buy = buy;
            i.sell = sell;
            i.vol = vol;
            i.change = change;
            this.change = false;
            return i;
        }
        public override string ToString()
        {
            return this.desc + ":" + this.key + "    "
                + "Price:" + this.price + "    "
                + "买卖:" + this.buy + "/" + this.sell + "    "
                + "vol:" + this.vol + "    " + this.updatetime.ToString();
        }
    }
    public interface IPrice
    {
        void Init(List<string> usei);
        string[] GetKeys();
        Info GetInfo(string key);
    }
    public class Price_Jubi : IPrice
    {
        public Price_Jubi()
        {
            infos["PGC "] = new Info("PGC ", "乐园通  ");
            infos["RSS "] = new Info("RSS ", "红贝壳  ");
            infos["FZ  "] = new Info("FZ  ", "冰河币  ");
            infos["VRC "] = new Info("VRC ", "维理币  ");
            infos["MAX "] = new Info("MAX ", "最大币  ");
            infos["TFC "] = new Info("TFC ", "传送币  ");
            infos["ZET "] = new Info("ZET ", "泽塔币  ");
            infos["QEC "] = new Info("QEC ", "企鹅币  ");
            infos["RIO "] = new Info("RIO ", "里约币  ");
            infos["XSGS"] = new Info("XSGS", "雪山古树");
            infos["YTC "] = new Info("YTC ", "一号币  ");
            infos["MTC "] = new Info("MTC ", "猴宝币  ");
            infos["GOOC"] = new Info("GOOC", "谷壳币  ");
            infos["ZCC "] = new Info("ZCC ", "招财币  ");
            infos["SKT "] = new Info("SKT ", "鲨之信  ");
            infos["LKC "] = new Info("LKC ", "幸运币  ");
            infos["MET "] = new Info("MET ", "美通币  ");
            infos["ETC "] = new Info("ETC ", "以太经典");
            infos["HLB "] = new Info("HLB ", "活力币  ");
            infos["DNC "] = new Info("DNC ", "暗网币  ");
            infos["XAS "] = new Info("XAS ", "阿希币  ");
            infos["MRYC"] = new Info("MRYC", "美人鱼币");
            infos["JBC "] = new Info("JBC ", "聚宝币  ");
            infos["PPC "] = new Info("PPC ", "点点币  ");
            infos["PLC "] = new Info("PLC ", "保罗币  ");
            infos["XPM "] = new Info("XPM ", "质数币  ");
            infos["DOGE"] = new Info("DOGE", "狗狗币  ");
            infos["EAC "] = new Info("EAC ", "地球币  ");
            infos["VTC "] = new Info("VTC ", "绿币    ");
            infos["WDC "] = new Info("WDC ", "世界币  ");
            infos["GAME"] = new Info("GAME", "游戏点  ");
            infos["IFC "] = new Info("IFC ", "无限币  ");
            infos["BTC "] = new Info("BTC ", "比特币  ");
            infos["LTC "] = new Info("LTC ", "莱特币  ");
            infos["ETH "] = new Info("ETH ", "以太坊  ");
            infos["KTC "] = new Info("KTC ", "肯特币  ");
            infos["ANS "] = new Info("ANS ", "小蚁股  ");
            infos["XRP "] = new Info("XRP ", "瑞波币  ");
            infos["PEB "] = new Info("PEB ", "普银    ");
            infos["BLK "] = new Info("BLK ", "黑币    ");
            infos["LSK "] = new Info("LSK ", "LISK    ");
            infos["NXT "] = new Info("NXT ", "未来币  ");
            infos["BTS "] = new Info("BTS ", "比特股  ");
        }
        System.Collections.Concurrent.ConcurrentDictionary<string, Info> infos = new System.Collections.Concurrent.ConcurrentDictionary<string, Info>();
        public string[] GetKeys()
        {
            return infos.Keys.ToArray();
        }
        public Info GetInfo(string key)
        {
            if (infos.ContainsKey(key) == false) return null;
            return infos[key].Clone();
        }
        Random r = new Random();

        public void Init(List<string> usei)
        {
            var ks = new List<string>(this.infos.Keys);
            foreach (var k in ks)
            {
                if (usei.Contains(k) == false)
                {
                    Info v;
                    this.infos.TryRemove(k, out v);
                }
                else
                {
                    this.infos[k].active = true;
                }
            }
            foreach (var c in infos)
            {
                if (c.Value.active == false) continue;
                string key = c.Key;
                System.Threading.Thread t = new System.Threading.Thread(() =>
                 {
                     System.Net.WebClient wc = new System.Net.WebClient();

                     while (true)
                     {
                         var keys = key.ToLower().Replace(" ", "");
                         var str = "https://www.jubi.com/api/v1/ticker/?coin=" + keys + "&nonce=" + r.Next();
                         try
                         {
                             var info = wc.DownloadString(str);
                             var p = MyJson.Parse(info) as MyJson.JsonNode_Object;
                             this.infos[key].price = double.Parse(p.GetDictItem("last").AsString());
                             this.infos[key].sell = double.Parse(p.GetDictItem("sell").AsString());
                             this.infos[key].buy = double.Parse(p.GetDictItem("buy").AsString());
                             this.infos[key].vol = double.Parse(p.GetDictItem("vol").ToString());
                             this.infos[key].updatetime = DateTime.Now;
                             this.infos[key].change = true;
                         }
                         catch
                         {

                         }
                         System.Threading.Thread.Sleep(1000);

                     }
                 });
                t.Start();

            }

        }

    }

    public class Price_19800 : IPrice
    {
        public Price_19800()
        {
            infos["BKC "] = new Info("BKC ", "百库币  ");
            infos["CFC "] = new Info("CFC ", "匠人币  ");
            infos["BKC "] = new Info("BKC ", "百库币  ");
            infos["GMC "] = new Info("GMC ", "游乐币  ");
            infos["BKC "] = new Info("BKC ", "百库币  ");
            infos["ATC "] = new Info("ATC ", "财产币  ");
            infos["BKC "] = new Info("BKC ", "百库币  ");
            infos["BKC "] = new Info("BKC ", "百库币  ");
            infos["BKC "] = new Info("BKC ", "百库币  ");
            infos["ELC "] = new Info("ELC ", "进化币  ");
            infos["XYC "] = new Info("XYC ", "逍遥币  ");
            infos["YTC "] = new Info("YTC ", "一号币  ");
            infos["NPLC"] = new Info("NPLC", "新保罗币");
            infos["WZC "] = new Info("WZC ", "技艺币  ");
            infos["GSS "] = new Info("GSS ", "魔鬼币  ");
            infos["MGC "] = new Info("MGC ", "魔酷币  ");
            infos["GYC "] = new Info("GYC ", "公益币  ");
            infos["FTC "] = new Info("FTC ", "羽毛币  ");
            infos["KPC "] = new Info("KPC ", "开普勒股");
            infos["SYS "] = new Info("SYS ", "系统币  ");
            infos["ARDR"] = new Info("ARDR", "阿朵    ");
            infos["BTS "] = new Info("BTS ", "比特股  ");
            infos["VTC "] = new Info("VTC ", "绿币    ");
            infos["ANS "] = new Info("ANS ", "小蚁股  ");
            infos["ETC "] = new Info("ETC ", "以太经典");
            infos["ETH "] = new Info("ETH ", "以太币  ");


        }
        System.Collections.Concurrent.ConcurrentDictionary<string, Info> infos = new System.Collections.Concurrent.ConcurrentDictionary<string, Info>();
        public string[] GetKeys()
        {
            return infos.Keys.ToArray();
        }
        public Info GetInfo(string key)
        {
            if (infos.ContainsKey(key) == false) return null;
            return infos[key].Clone();
        }
        Random r = new Random();
        public void Init(List<string> usei)
        {
            var ks = new List<string>(this.infos.Keys);
            foreach (var k in ks)
            {
                if (usei.Contains(k) == false)
                {
                    Info v;
                    this.infos.TryRemove(k, out v);
                }
                else
                {
                    this.infos[k].active = true;
                }
            }
            foreach (var c in infos)
            {
                if (c.Value.active == false) continue;

                string key = c.Key;
                System.Threading.Thread t = new System.Threading.Thread(() =>
                {
                    System.Net.WebClient wc = new System.Net.WebClient();

                    while (true)
                    {
                        var keys = key.ToLower().Replace(" ", "");
                        var str = "https://www.19800.com/api/v1/ticker?market=cny_" + keys + "&nonce=" + r.Next();

                        try
                        {
                            var info = wc.DownloadString(str);
                            var p = MyJson.Parse(info) as MyJson.JsonNode_Object;
                            if (p.ContainsKey("code") == false)
                            {

                            }
                            var code = p.GetDictItem("code").AsInt();
                            this.infos[key].updatetime = DateTime.Now;

                            if (code >= 0)
                            {
                                try
                                {
                                    var dat = p.GetDictItem("data").asDict();
                                    this.infos[key].price = dat["LastPrice"].AsDouble();
                                    this.infos[key].sell = dat["TopAsk"].AsDouble();
                                    this.infos[key].buy = dat["TopBid"].AsDouble();
                                    this.infos[key].vol = dat["Volume"].AsDouble();
                                    this.infos[key].change = true;
                                }
                                catch
                                {

                                }
                            }


                        }
                        catch (Exception err)
                        {

                        }
                        System.Threading.Thread.Sleep(1000);

                    }
                });
                t.Start();

            }

        }

    }

    public class Price_yuanbao : IPrice
    {
        public Price_yuanbao()
        {
            infos["BTC "] = new Info("BTC ", "比特币  ");
            infos["ETH "] = new Info("ETH ", "以太坊  ");
            infos["ETC "] = new Info("ETC ", "以太经典");
            infos["LTC "] = new Info("LTC ", "莱特币  ");
            infos["YBC "] = new Info("YBC ", "元宝币  ");
            infos["DOGE"] = new Info("DOGE", "狗狗币  ");
            infos["ZEC "] = new Info("ZEC ", "Zcash   ");
            infos["XLM "] = new Info("XLM ", "恒星币  ");
            infos["XRP "] = new Info("XRP ", "瑞波币  ");
            infos["ANS "] = new Info("ANS ", "小蚁股  ");
            infos["GAME"] = new Info("GAME", "游戏点  ");
            infos["BASH"] = new Info("BASH", "幸运链  ");
            infos["WTC "] = new Info("WTC ", "瓦特币  ");
            infos["XKC "] = new Info("XKC ", "小咖币  ");
            infos["MCC "] = new Info("MCC ", "行云币  ");
            infos["NBC "] = new Info("NBC ", "努比亚  ");
            infos["MC  "] = new Info("MC  ", "猴币    ");
            infos["MRYC"] = new Info("MRYC", "美人鱼币");
            infos["ABC "] = new Info("ABC ", "TAI     ");
            infos["LMC "] = new Info("LMC ", "邻萌宝  ");
            infos["SHC "] = new Info("SHC ", "分享币  ");
        }
        System.Collections.Concurrent.ConcurrentDictionary<string, Info> infos = new System.Collections.Concurrent.ConcurrentDictionary<string, Info>();
        public string[] GetKeys()
        {
            return infos.Keys.ToArray();
        }
        public Info GetInfo(string key)
        {
            if (infos.ContainsKey(key) == false) return null;
            return infos[key].Clone();
        }
        Random r = new Random();
        public void Init(List<string> usei)
        {
            var ks = new List<string>(this.infos.Keys);
            foreach (var k in ks)
            {
                if (usei.Contains(k) == false)
                {
                    Info v;
                    this.infos.TryRemove(k, out v);
                }
                else
                {
                    this.infos[k].active = true;
                }
            }
            foreach (var c in infos)
            {
                if (c.Value.active == false) continue;

                string key = c.Key;
                System.Threading.Thread t = new System.Threading.Thread(() =>
                {
                    System.Net.WebClient wc = new System.Net.WebClient();

                    while (true)
                    {
                        var keys = key.ToLower().Replace(" ", "");
                        var str = "https://www.yuanbao.com/api_market/getinfo_cny/coin/" + keys;

                        try
                        {
                            var info = wc.DownloadString(str);
                            var p = MyJson.Parse(info) as MyJson.JsonNode_Object;
                            if (p.ContainsKey("code") == false)
                            {

                            }
                            this.infos[key].updatetime = DateTime.Now;

                            try
                            {
                                this.infos[key].price = double.Parse(p["price"].ToString());
                                this.infos[key].sell = double.Parse(p["sale"].ToString());
                                this.infos[key].buy = double.Parse(p["buy"].ToString());
                                this.infos[key].vol = double.Parse(p["volume_24h"].ToString());
                                this.infos[key].change = true;
                            }
                            catch (Exception err)
                            {

                            }


                        }
                        catch (Exception err)
                        {

                        }
                        System.Threading.Thread.Sleep(1000);
                    }

                });
                t.Start();

            }

        }

    }
    public class InfoPack:List<Info>
    {
        public InfoPack(int count)
        {
            for(var i=0;i<count;i++)
            {
                this.Add(null);
            }
        }
        public string GetDesc()
        {
            for(var i=0;i<this.Count;i++)
            {
                if (this[i] != null)
                    return this[i].desc;
            }
            return null;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
    public class PriceTool
    {
        Dictionary<string, IPrice> prices = new Dictionary<string, IPrice>();
        Dictionary<string, int> cnt = new Dictionary<string, int>();
        public void Init()
        {
            prices["元宝"] = new Price_yuanbao();
            prices["聚币"] = new Price_Jubi();
            prices["19800"] = new Price_19800();
            foreach (var p in prices.Values)
            {
                foreach (var k in p.GetKeys())
                {
                    if (cnt.ContainsKey(k)) cnt[k]++;
                    else
                        cnt[k] = 1;
                }
            }

            var vs = new List<string>(cnt.Keys);
            foreach (var v in vs)
            {
                if (cnt[v] < 2)
                    cnt.Remove(v);
            }
            int i = 0;
            vs = new List<string>(cnt.Keys);
            foreach (var k in vs)
            {
                cnt[k] = i;
                i++;
            }
            foreach (var p in prices.Values)
            {
                p.Init(vs);
            }
        }
        public string[] GetKeys()
        {
            return cnt.Keys.ToArray();
        }
        public string[] GetPrices()
        {
            return prices.Keys.ToArray();
        }
        public Info GetInfo(string price,string key)
        {
            return prices[price].GetInfo(key);
        }
        public InfoPack GetInfos(string key)
        {
            var p = GetPrices();
            InfoPack iss = new InfoPack(p.Length);
            for(var i=0;i<iss.Count;i++)
            {
                iss[i] = prices[p[i]].GetInfo(key);
            }
            return iss;
        }
    }

}
