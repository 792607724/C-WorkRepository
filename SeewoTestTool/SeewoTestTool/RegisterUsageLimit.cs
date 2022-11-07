// ��ȡ����ʱ��
using System.Net;

public class RegisterUsageLimit
{
    private string activateTime;

    // get set��д����ֱ��ͨ��xxx.xxx���ػ�����ֵ
    public string ActivateTime
    {
        get { return activateTime; }
        set { activateTime = value; }
    }

    // ��ȡ����ʱ��
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
            datetime = Convert.ToDateTime(datetime).ToString("yyyy��MM��dd��HHʱmm��ss��");
            string originalTime = Convert.ToDateTime(datetime).ToString("yyyyMMddHHmmss");
            return datetime + "/" + originalTime;
        }
    }

}