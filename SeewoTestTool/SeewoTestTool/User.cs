[Serializable]

// ����User�����洢�豸��¼web��username��password
public class User
{
	private string username;
	private string password;

	// get set��д����ֱ��ͨ��xxx.xxx���ػ�����ֵ
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