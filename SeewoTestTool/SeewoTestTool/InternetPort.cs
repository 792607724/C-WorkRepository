[Serializable]

// 定义InternetPort类来存储设备的IP和端口号
public class InternetPort
{
	private string deviceIP;
	private string devicePort;

    // get set简化写法，直接通过xxx.xxx返回或设置值
    public string Deviceip
	{
		get { return deviceIP; }
		set { deviceIP = value; }
	}

	public string Deviceport
	{
		get { return devicePort; }
		set { devicePort = value; }
	}

}
