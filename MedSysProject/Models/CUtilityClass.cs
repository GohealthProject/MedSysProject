using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MedSysProject.Models
{
    public class CUtilityClass
    {
      
        public CUtilityClass()
        {
            
        }
        public static  string EmailText(string TradeNo,string proname,string proCount,string total)
        {
            string html = "";
            List<string> proList = new List<string>();
            List<string> proCountList =new List<string>();
            proCountList = proCount.Split('#').ToList();
            total = Int32.Parse(total).ToString("N0");
            proList = proname.Split('#').ToList();
            List<Product> products = new List<Product>();

            html = "<h2>你好！很高興您能來我們網站消費。</h2>";
            html += "<h3>您的EcPay交易編號為：" + TradeNo + "</h3>";
            html += "<table style='border-collapse:collapse;border:1px solid #ddd'><thead><tr style='border:1px solid #ddd;padding:8px;'><td style='border:1px solid #ddd;padding:8px;'>產品名稱</td><td style='padding:8px;'>數量</td></tr><thead><tbody>";
            for(int i =0; i < proList.Count-1; i++)
            {
                html += "<tr style='border:1px solid #ddd;padding:8px;'><td style='border:1px solid #ddd;padding:8px;'>" + proList[proList.Count-2-i] + "</td><td style='padding:8px;'>" + proCountList[i] + "</td></tr>";
            }
            html += "<tr style='border:1px solid #ddd;padding:8px;'><td style='border:1px solid #ddd;padding:8px;'>總價格:<td style='padding:8px;'> " + total + "元<td></tr>";
            html += "</tbody></table>";
            html += "期待你能回到我們網站再次消費，謝謝！<br />";
            html += "MedSys團隊敬上";
            return html;
        }
    }
}
