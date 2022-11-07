// 获取网络时间
using System.Net;

public class RegisterUsageLimit
{
    private string activateTime;

    // get set简化写法，直接通过xxx.xxx返回或设置值
    public string ActivateTime
    {
        get { return activateTime; }
        set { activateTime = value; }
    }

    // 获取网络时间
    public string GetNetDateTime()
    {
        WebRequest request = null;
        WebResponse response = null;
        WebHeaderCollection headerCollection = null;
        string datetime = string.Empty;
        try
        {
            request = WebRequest.Create("https://www.baidu.com");
            request.Timeout = 3000;
            request.Credentials = CredentialCache.DefaultCredentials;
            response = request.GetResponse();
            headerCollection = response.Headers;
            foreach (var h in headerCollection.AllKeys)
            {
                if (h == "Date")
                {
                    datetime = headerCollection[h];
                }
            }
        }
        catch (Exception) 
        {
            return "NetWorkProblem"; 
        }
        finally
        {
            if (request != null)
            { request.Abort(); }
            if (response != null)
            { response.Close(); }
            if (headerCollection != null)
            { headerCollection.Clear(); }
        }
        if (string.IsNullOrEmpty(datetime))
        {
            return "NetWorkProblem";
        }
        else
        {
            datetime = Convert.ToDateTime(datetime).ToString("yyyy年MM月dd日HH时mm分ss秒");
            string originalTime = Convert.ToDateTime(datetime).ToString("yyyyMMddHHmmss");
            return datetime + "/" + originalTime;
        }
    }

}