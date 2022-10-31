[Serializable]

// 定义User类来存储设备登录web的username和password
public class User
{
	private string username;
	private string password;

	// get set简化写法，直接通过xxx.xxx返回或设置值
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