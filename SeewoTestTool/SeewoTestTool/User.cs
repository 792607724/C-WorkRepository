[Serializable]

// 定义InternetPort类来存储设备的IP和端口号
public class User
{
	private string username;
	private string password;

	public string Username
	{
		get { return username; }
		set { username = value; }
	}

	public string Password
	{
		get { return password; }
		set { password = value; }
	}

}