[Serializable]

// 定义InternetPort类来存储设备的IP和端口号
public class InternetPort
{
	private string deviceIP;
	private string devicePort;

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