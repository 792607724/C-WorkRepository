using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace SeewoTestTool
{
    public partial class Form1 : Form
    {
        Socket clientSocket;
        Dictionary<string, InternetPort> internetPorts = new Dictionary<string, InternetPort>();
        Dictionary<string, User> users = new Dictionary<string, User>();
        /**
         * ���ڹ��췽����
         * ��������У�
         *  1���ؼ�ʵ����
         *  2�����ݶ�д����
         *      A��"data.bin"��IP�Ͷ˿ںŵļ��书��д��
         *      B��"data1.bin"��Web��¼У��username��password����д��
         */
        public Form1()
        {
            InitializeComponent();

            FileStream fs = new FileStream("data.bin", FileMode.OpenOrCreate);
            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                internetPorts = bf.Deserialize(fs) as Dictionary<string, InternetPort>;
                foreach (InternetPort internetPort in internetPorts.Values)
                {
                    device_ip_textbox.Text = internetPort.Deviceip;
                }
                for (int i = 0; i < internetPorts.Count; i++)
                {
                    if (device_ip_textbox.Text != "")
                    {
                        if (internetPorts.ContainsKey(device_ip_textbox.Text))
                        {
                            string port_temp = internetPorts[device_ip_textbox.Text].Deviceport;
                            if (int.Parse(port_temp) == 80)
                            {
                                radioButton_80.Checked = true;
                            }
                            else
                            {
                                radioButton_8080.Checked = true;

                            }
                            rememberCheckBox.Checked = true;
                        }
                    }
                }

                
                
            }
            FileStream fs1 = new FileStream("data1.bin", FileMode.OpenOrCreate);
            if (fs1.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                users = bf.Deserialize(fs1) as Dictionary<string, User>;
                foreach (User user in users.Values)
                {
                    username_textbox.Text = user.Username;
                }
                for (int i = 0; i < users.Count; i++)
                {
                    if (username_textbox.Text != "")
                    {
                        if (users.ContainsKey(username_textbox.Text))
                        {
                            string password_temp = users[username_textbox.Text].Password;
                            password_textbox.Text = password_temp;
                        }
                    }
                }
            }
            fs.Close();
            fs1.Close();
            // �����Զ������豸����
            device_connect_button_Click(null, null);
            login_button_Click(null, null);
        }

        private readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        private void CallBackMethod(IAsyncResult asyncresult)
        {
            //ʹ�������̼߳��� 
            TimeoutObject.Set();
        }

        string ip_users;
        // ��֤�豸������״̬ ����ͨ��
    
        /**
         * �����豸����¼�
         */
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("��ִ�в����������豸�����ӡ���\n");
            string host = device_ip_textbox.Text;
            ip_users = host;
            string port;
            if (radioButton_80.Checked)
            {
                port = radioButton_80.Text;
            }
            else 
            {
                port = radioButton_8080.Text;
            }
            if (string.IsNullOrEmpty(host))
            {
                output_rich_textbox.AppendText("�豸����IP��ַ�Ͷ˿ںŲ���Ϊ�գ�\n");
            }
            else
            {
                // Socket Connection Build
                try
                {
                    TimeoutObject.Reset();
                    
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, int.Parse(port));

                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.BeginConnect(ipe, CallBackMethod, clientSocket);
                    if (TimeoutObject.WaitOne(2000, false))
                    {
                        if (clientSocket.Connected)
                        {
                            output_rich_textbox.AppendText($"�豸ip:{host}:{port}�������ϣ�\n");
                            device_connect_button.Enabled = false;
                            device_disconnect_button.Enabled = true;
                            device_status_label.Text = "������";
                            device_ip_textbox.Enabled = false;
                            radioButton_80.Enabled = false;
                            radioButton_8080.Enabled = false;
                            login_button.Enabled = true;
                            device_reset_button.Enabled = true;
                            rebootDevice_button.Enabled = true;
                            // ���Ӽ�סIP�Ͷ˿ڹ���
                            if (rememberCheckBox.Checked == true)
                            {
                                FileStream fileStream = new FileStream("data.bin", FileMode.OpenOrCreate);
                                BinaryFormatter binaryFormatter = new BinaryFormatter();
                                InternetPort internetPort = new InternetPort();
                                internetPort.Deviceip = host;
                                internetPort.Deviceport = port;
                                if (internetPorts.ContainsKey(internetPort.Deviceip))
                                {
                                    // ������ھ������
                                    //internetPorts.Remove(internetPort.Deviceip);
                                    internetPorts.Clear();
                                }
                                internetPorts.Add(internetPort.Deviceip, internetPort);
                                binaryFormatter.Serialize(fileStream, internetPorts);
                                fileStream.Close();
                            }
                            login_button_Click(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"���ӳ�ʱ�����飡\nIP:{host}�� PORT:{port}");
                    }
                    
                    /*
                    send_Str("am start com.android.browser");
                    string rec_Str = receive_Str();
                    */
                }
                catch (Exception ex)
                {
                    device_status_label.Text = "�ѶϿ�";
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    output_rich_textbox.AppendText($"�豸����IP��ַ�Ͷ˿ںŴ��������Ƿ�������ȷ��\n����Log���£�{ex.ToString()}\n");
                    
                }

            }
        }

        // ��cmd��ִ���������
        /**
         *  �����ַ������������CMD��ִ����Ӧ��ָ��
         */
        private string executeCMDCommand(string command)
        {
            Process process_cmd = new Process();
            process_cmd.StartInfo.FileName = "cmd.exe";
            process_cmd.StartInfo.RedirectStandardInput = true;
            process_cmd.StartInfo.RedirectStandardOutput = true;
            process_cmd.StartInfo.CreateNoWindow = true;
            process_cmd.StartInfo.UseShellExecute = false;
            process_cmd.Start();
            process_cmd.StandardInput.WriteLine(command + "&exit");
            process_cmd.StandardInput.AutoFlush = true;
            string output_string = process_cmd.StandardOutput.ReadToEnd();
            process_cmd.WaitForExit();
            process_cmd.Close();
            return output_string;

        }

        // Socket���������
        /**
         *  �ṩ��Socket�˷������������ǰ����δ�õ��������ú�����������Ŀ��ʹ�ÿ���������
         */
        private void send_Str(string sendStr)
        {
            output_rich_textbox.AppendText($"��ִ�в�����Socket����ָ������{sendStr}����\n");
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                // Socket��������
                try
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
                    clientSocket.Send(sendBytes);
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Socket��������ִ��ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // Socket���շ������ݺ���
        /**
         *  �ṩ��Socket�˽����豸�ն˷���ֵ��������ǰ����δ�õ��������ú�����������Ŀ��ʹ�ÿ���������
         */
        private string receive_Str()
        {
            output_rich_textbox.AppendText("��ִ�в�����Socket���շ������ݡ���\n");
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                while (true)
                {
                    try
                    {
                        string recStr = "";
                        byte[] recBytes = new byte[4096];
                        //  �������ķ�ʽ���գ����յ�����ǰ����Ӧ��
                        int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
                        if (bytes == 0)
                        {
                            break;
                        }
                        recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
                        output_rich_textbox.AppendText($"�������ݣ�{recStr}\n");
                        return recStr;
                    }
                    catch (Exception ex)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        device_disconnect_button.Enabled = false;
                        device_connect_button.Enabled = true;
                        device_ip_textbox.Enabled = true;
                        radioButton_80.Enabled = true;
                        radioButton_8080.Enabled = true;
                        device_status_label.Text = "�ѶϿ�";
                        output_rich_textbox.AppendText($"Socket��������ִ��ʧ�ܣ�\n{ex.ToString()}\n");
                        break;
                    }
                }
                return "";
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
                return "";
            }
        }

        // �Ͽ��豸����
        /**
         * �Ͽ��豸��ť����¼�
         */
        private void device_disconnect_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("��ִ�в������Ͽ���ǰ�������豸����\n");
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    output_rich_textbox.AppendText("�Ͽ���ǰ�������豸\n");
                    device_disconnect_button.Enabled = false;
                    device_connect_button.Enabled = true;
                    clientSocket.Close();
                    device_status_label.Text = "�ѶϿ�";
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    check_current_firmware_button.Enabled = false;
                    upgrade_button.Enabled = false;
                    getCurrentSN_button.Enabled = false;
                    writeIn_button.Enabled = false;
                    login_button.Enabled = false;
                    getCurrentPCBA_button.Enabled = false;
                    writeInPCBA_button.Enabled = false;
                    start_array_mic_audio_level_test_button.Enabled = false;
                    stop_array_mic_audio_level_test_button.Enabled = false;
                    gain_array_mic_audio_level_button.Enabled = false;
                    gainCurrentVersion_button.Enabled = false;
                    login_button.Text = "��¼";
                    login_button.Enabled = true;
                    device_reset_button.Enabled = false;
                    rebootDevice_button.Enabled = false;
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = false;
                    get_poe_mic_info_button.Enabled = false;
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�Ͽ��豸����ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
        }

        string filePath = null;
        // ѡ�������Ĺ̼�·��
        /**
         * ѡ�������̼�����ť����¼�
         */
        private void choose_upgrade_firmware_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("��ִ�в�����ѡ�������̼�����\n");
            //if(true)
            if (clientSocket != null && clientSocket.Connected)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = dialog.FileName;
                    if (string.IsNullOrEmpty(filePath))
                    {
                        upgrade_firmware_textbox.Text = "δѡ����ȷ�Ĺ̼�·��";
                    }
                    else
                    {
                        upgrade_firmware_textbox.Text = filePath;
                        output_rich_textbox.AppendText($"��ǰѡ��Ĺ̼�·��Ϊ��\n{filePath}\n");

                    }
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // �̼���������
        /**
         * ������ť����¼�
         */
        private void upgrade_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("��ִ�в������̼���������\n");
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                string filePath = upgrade_firmware_textbox.Text;
                if (string.IsNullOrEmpty(filePath))
                {
                    upgrade_firmware_textbox.Text = "δѡ����ȷ�Ĺ̼�·��";
                }
                else
                {
                    // �̼���ʼ����
                    try
                    {
                        output_rich_textbox.AppendText("��ʼ�����������ĵȴ���ɣ�\n");
                        // ʹ�ú�̨�߳�ȥ������������ֹUI��������
                        upgrade_progressbar.Value = 0;
                        upgrade_button.Enabled = false;
                        if (backgroundworker_firmwareupgrade.IsBusy)
                        {
                            return;
                        }
                        backgroundworker_firmwareupgrade.RunWorkerAsync("Hello");
                    }
                    catch (Exception ex)
                    {
                        output_rich_textbox.AppendText($"�̼���������ʧ�ܣ�\n{ex.ToString()}\n");
                        upgrade_progressbar.Value = 0;
                    }
                    finally
                    {

                    }
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // У�鵱ǰ�̼�
        /**
         * ���鵱ǰ�̼����汾��ť����¼�
         */
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�����У�鵱ǰ�豸�̼��汾��̼����汾�Ƿ�һ�¡���\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                output_rich_textbox.AppendText("У��̼����ԣ�\n");
                string currentFirmware = null;
                try
                {
                    // У��̼� - ��session�л�ȡ�̼���ǰ�汾
                    string checkVersionCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\":\\\"getParam\\\",\\\"session\\\":\\\"{session}\\\",\\\"name\\\":\\\"DevInfo\\\"}}\"";
                    output_string = executeCMDCommand(checkVersionCommand);
                    MatchCollection results_2 = Regex.Matches(output_string, "\"SoftwaveVersion\" : (.*)");
                    string currentVersion = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("��ǰ�汾�ǣ�" + currentVersion + "\n");

                    // ������Firmwareѹ������ѹ��info.txt�л�ȡ��Ҫ�����İ汾���к˶�
                    if (string.IsNullOrEmpty(filePath))
                    {
                        output_rich_textbox.AppendText("���ȵ����ѡ�������̼������ٽ��к˶Բ�����\n");
                    }
                    else
                    {
                        using (var zip = ZipFile.OpenRead(filePath))
                        {
                            foreach (var entry in zip.Entries)
                            {
                                if (entry.FullName == "info.txt")
                                {
                                    output_rich_textbox.AppendText($"��ǰ����Ĺ̼��ļ�Ϊ��{entry.FullName}\n");
                                    using (var stream = entry.Open())
                                    using (var reader = new StreamReader(stream))
                                    {
                                        var content_get = reader.ReadToEnd();
                                        MatchCollection results_3 = Regex.Matches(content_get.ToString(), "project=(.*)");
                                        string toProduct = results_3[0].ToString().Split("=")[1];
                                        MatchCollection results_4 = Regex.Matches(content_get.ToString(), "version=(.*)");
                                        string toVersion = results_4[0].ToString().Split("=")[1];
                                        output_rich_textbox.AppendText($"��ǰ�˶Եİ汾Ϊ��{toVersion}, ��ĿΪ��{toProduct}\n");

                                        Font font = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);
                                        if (currentVersion == toVersion && toProduct == "SXW0301")
                                        {
                                            output_rich_textbox.ForeColor = Color.Green;
                                            output_rich_textbox.SelectionFont = font;
                                            checked_firmware_textbox.Text = $"�̼�У��ɹ�����ǰ�̼��汾��{currentVersion}";
                                            output_rich_textbox.AppendText($"�̼�У��ɹ�����ǰ�̼��汾��{currentVersion}\n");
                                        }
                                        else
                                        {
                                            output_rich_textbox.ForeColor = Color.Red;
                                            output_rich_textbox.SelectionFont = font;
                                            checked_firmware_textbox.Text = $"�̼�У��ʧ�ܣ���ǰ�̼��汾��{currentVersion}";
                                            output_rich_textbox.AppendText($"�̼�У��ʧ�ܣ���ǰ�̼��汾��{currentVersion}\n");
                                        }
                                    }
                                }

                            }
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"У��̼�����ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                { 
                    
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // �򿪺��̵ƽ�����˸
        /**
         * �򿪺���ָʾ�ƽ�����˸��ť����¼�
         */
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в������򿪺��̵ƽ�����˸����\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // �򿪺��̵ƽ�����˸
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"ledTest\\\",\\\"open\\\": true}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "�򿪺��̵ƽ�����˸����δִ�гɹ�";
                    if (back_code == "0")
                    {
                        result = "�ɹ�";
                        stop_rg_flicker_button.Enabled = true;
                        start_rg_flicker_button.Enabled = false;
                    }
                    else
                    {
                        result = "ʧ��";
                    }
                    output_rich_textbox.AppendText("�򿪺��̵ƽ�����˸�����" + result + "��back_code����"+ back_code + "\n");

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�򿪺��̵ƽ�����˸����ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // �رպ��̵ƽ�����˸
        /**
         * �رպ���ָʾ�ƽ�����˸��ť����¼�
         */
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в������رպ��̵ƽ�����˸����\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // �رպ��̵ƽ�����˸
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"ledTest\\\",\\\"open\\\": false}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "�رպ��̵ƽ�����˸����δִ�гɹ�";
                    if (back_code == "0")
                    {
                        result = "�ɹ�";
                        stop_rg_flicker_button.Enabled = false;
                        start_rg_flicker_button.Enabled = true;
                    }
                    else
                    {
                        result = "ʧ��";
                    }
                    output_rich_textbox.AppendText("�رպ��̵ƽ�����˸�����" + result + "��back_code����" + back_code + "\n");

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�رպ��̵ƽ�����˸����ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // ��ȡ������Ϣ
        /**
         * ��ȡ������Ϣ��ť����¼�
         */
        private void get_poe_mic_info_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�������ȡ������Ϣ����\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // ��ȡ������Ϣ
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"microphoneInfo\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "��ȡ������Ϣ����δִ�гɹ�";
                    if (back_code == "0")
                    {
                        result = "�ɹ�";
                        MatchCollection results_2 = Regex.Matches(output_string, "\"micHardwareVersion\" : (.*)");
                        string micHardwareVersion = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",","");
                        MatchCollection results_3 = Regex.Matches(output_string, "\"micSoftwareVersion\" : (.*)");
                        string micSoftwareVersion = results_3[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                        poe_mic_hardware_info_label.Text = micHardwareVersion;
                        poe_mic_firmware_info_label.Text = micSoftwareVersion;
                    }
                    else
                    {
                        result = "ʧ��";
                    }
                    output_rich_textbox.AppendText("��ȡ������Ϣ�����" + result + "��back_code����" + back_code + "\n");
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"��ȡ������Ϣʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // Audio IN 1�ڲ���
        /**
         * Audio IN 1�ڲ��Ե���¼�����ʱ�ò��ϣ������÷���
         */
        private void audioin1_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�����Audio IN 1�ڲ��ԡ���\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Audio IN 1�ڲ���ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // Audio IN 2�ڲ���
        /**
         * Audio IN 2�ڲ��Ե���¼�����ʱ�ò��ϣ������÷���
         */
        private void audioin2_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�����Audio IN 2�ڲ��ԡ���\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Audio IN 2�ڲ���ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // ��������MIC����ֵ����
        /**
         * ����������˷�����ֵ����
         */
        private void array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�������������MIC����ֵ���ԡ���\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // ��������MIC����ֵ����
                    output_rich_textbox.AppendText(session);
                    string beginDeviceMICVolumeTestCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"startAudioTest\\\",\\\"format\\\": 0,\\\"soundmode\\\": 8,\\\"samplerate\\\": 16000,\\\"periodsize\\\": 1024}}\"";
                    output_string = executeCMDCommand(beginDeviceMICVolumeTestCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    
                    if (Int32.Parse(backCode) == 0)
                    {
                        output_rich_textbox.AppendText($"ִ�н��Ϊ��PASS���ѿ�������MIC����ֵ���ԣ�backCode:[{backCode}]\n");
                    }
                    else if(Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL��δ��������MIC����ֵ���ԣ�backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"��������MIC����ֵ����ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // ֹͣ����MIC����ֵ����
        /**
         * ֹͣ������˷�����ֵ����
         */
        private void stop_array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�����ֹͣ����MIC����ֵ���ԡ���\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // ֹͣ����MIC����ֵ����
                    output_rich_textbox.AppendText(session);
                    string stopDeviceMICVolumeTestCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"stopAudioTest\\\"}}\"";
                    output_string = executeCMDCommand(stopDeviceMICVolumeTestCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                    if (Int32.Parse(backCode) == 0)
                    {
                        output_rich_textbox.AppendText($"ִ�н��Ϊ��PASS����ֹͣ����MIC����ֵ���ԣ�backCode:[{backCode}]\n");
                    }
                    else if (Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL���޷��ر�����MIC����ֵ���ԣ�backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"ֹͣ����MIC����ֵ����ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // ��ȡ��·MIC��Ƶ����ֵ
        /**
         * ����������˷�����ֵ���Ժ󣬶�8·��˷��ֵ���л�ȡ
         */
        private void gain_array_mic_audio_level_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�������ȡ��·MIC��Ƶ����ֵ����\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // ��ȡ��·MIC��Ƶ����ֵ
                    output_rich_textbox.AppendText(session);
                    string gainDeviceMICVolumeCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getAudioVolume\\\"}}\"";
                    output_string = executeCMDCommand(gainDeviceMICVolumeCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",","");
                    if (Int32.Parse(backCode) == 0)
                    {
                        MatchCollection results_2 = Regex.Matches(output_string, "\"volumes\" : (.*)");
                        string volume1 = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace("[","").Replace("]","").Split(",")[0];
                        string volume2 = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace("[", "").Replace("]", "").Split(",")[1];
                        string volume3 = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace("[", "").Replace("]", "").Split(",")[2];
                        string volume4 = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace("[", "").Replace("]", "").Split(",")[3];
                        string volume5 = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace("[", "").Replace("]", "").Split(",")[4];
                        string volume6 = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace("[", "").Replace("]", "").Split(",")[5];
                        string volume7 = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace("[", "").Replace("]", "").Split(",")[6];
                        string volume8 = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace("[", "").Replace("]", "").Split(",")[7];
                        output_rich_textbox.AppendText($"ִ�н��Ϊ��PASS����ȡ��·MIC��Ƶ����ֵ��backCode:[{backCode}]\n");
                        volume1_value_label.Text = volume1;
                        volume2_value_label.Text = volume2;
                        volume3_value_label.Text = volume3;
                        volume4_value_label.Text = volume4;
                        volume5_value_label.Text = volume5;
                        volume6_value_label.Text = volume6;
                        volume7_value_label.Text = volume7;
                        volume8_value_label.Text = volume8;
                        output_rich_textbox.AppendText($"volume1��{volume1}\nvolume2��{volume2}\nvolume3��{volume3}\nvolume4��{volume4}\nvolume5��{volume5}\nvolume6��{volume6}\nvolume7��{volume7}\nvolume8��{volume8}\n");

                        if ((Int32.Parse(volume1) > 0) && (Int32.Parse(volume2) > 0) && (Int32.Parse(volume3) > 0) && (Int32.Parse(volume4) > 0) && (Int32.Parse(volume5) > 0) && (Int32.Parse(volume6) > 0) && (Int32.Parse(volume7) > 0) && (Int32.Parse(volume8) > 0))
                        {
                            audioin1_result_label.Text = "PASS";
                            audioin2_result_label.Text = "PASS";
                        }
                        else
                        {
                            audioin1_result_label.Text = "FAIL";
                            audioin2_result_label.Text = "FAIL";
                        }

                    }
                    else if (Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL���޷���ȡ��·MIC��Ƶ����ֵ��backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"��ȡ��·MIC��Ƶ����ֵʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // �豸��λ
        /**
         * �豸��λ��ť����¼�
         */
        private void device_reset_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в������豸��λ����\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // �豸��λ
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"control\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"restoreSettings\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "��λ����δִ�гɹ�";
                    if (back_code == "0")
                    {
                        result = "�ɹ�";
                        device_disconnect_button_Click(null, null);
                    }
                    else
                    {
                        result = "ʧ��";
                    }
                    output_rich_textbox.AppendText("�豸��λ�����" + result + "\n");
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "�ѶϿ�";
                    output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
                }

            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"�豸��λʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // ����ر��¼������豸socket�����ͷŵ�
        /**
         * ��д����ر��¼����ر�ʱ�Զ��ͷ�һЩ����
         */
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            output_rich_textbox.AppendText("��ִ�в���������ر��¼������豸socket�����ͷŵ�����\n");
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    clientSocket.Close();
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"����رս��豸socket�����ͷŵ�ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
        }

        // ����б�ʼ����ʾ��������Log
        /**
         * ������������ݹ�궨λ���
         */
        private void richTextChanged_to(object sender, EventArgs e)
        {
            // �����λ�����õ���ǰ���ݵ�ĩβ
            output_rich_textbox.SelectionStart = output_rich_textbox.Text.Length;
            // ���������λ��
            output_rich_textbox.ScrollToCaret();
        }

        // ����������
        /**
         * ��հ�ť����¼�
         */
        private void clear_output_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(output_rich_textbox.Text))
            {
                output_rich_textbox.Text = "";
                output_rich_textbox.ForeColor = Color.Black;
                Font font2 = new Font(FontFamily.GenericMonospace, 9, FontStyle.Regular);
                output_rich_textbox.Font = font2;
            }
            output_rich_textbox.AppendText("��ִ�в��������������ݡ���\n");
        }

        string output_string = "";
        // �½�backgroundworker���̼�������������ֹUI������������
        /**
         *  BackgroundWorker��̨�¼���DoWork��д���˴�ֻ�����ݺ�ʱ������������UI����
         */
        private void backgroundworker_firmwareupgrade_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                if (backgroundworker_firmwareupgrade.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    int progress_i = 0;
                    //�������� - ���ӹ̼����飬���ѹ�����е�info.txt����Ŀ�����Ƿ���ȷ
                    using (var zip = ZipFile.OpenRead(filePath))
                    {
                        foreach (var entry in zip.Entries)
                        {
                            if (entry.FullName == "info.txt")
                            {
                                using (var stream = entry.Open())
                                using (var reader = new StreamReader(stream))
                                {
                                    var content_get = reader.ReadToEnd();
                                    MatchCollection results_3 = Regex.Matches(content_get.ToString(), "project=(.*)");
                                    string toProduct = results_3[0].ToString().Split("=")[1];
                                    MatchCollection results_4 = Regex.Matches(content_get.ToString(), "version=(.*)");
                                    string toVersion = results_4[0].ToString().Split("=")[1];
                                    if (toProduct != "SXW0301")
                                    {
                                        backgroundworker_firmwareupgrade.CancelAsync();
                                        output_string = $"��ǰ��ѡ�̼��쳣���Ǳ���Ŀ�̼���ǰ�̼����ڣ���{toProduct}��������ʧ�ܣ�\n";
                                    }
                                    else
                                    {
                                        // �������� - ���̼��ƽ�ȥ ����ȷ�������ˣ����ip������������ȡ
                                        output_string = executeCMDCommand($"curl -T {filePath} \"ftp://{ip_users}/\"");
                                        progress_i += 50;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Pushing\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // �������� - �����ļ��� need_upgrade
                                        if (!System.IO.Directory.Exists("need_upgrade"))
                                        {
                                            output_string = executeCMDCommand("touch need_upgrade");
                                        }
                                        progress_i += 10;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Createing\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // �������� - ��ʼ����
                                        output_string = executeCMDCommand($"curl -T need_upgrade \"ftp://{ip_users}/\"");
                                        progress_i += 5;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // �������� - �������״̬
                                        bool upgradeDone = false;
                                        if (System.IO.File.Exists("upgrade_result"))
                                        {
                                            output_string = executeCMDCommand("rm -rf upgrade_result");
                                        }
                                        while (!upgradeDone)
                                        {
                                            output_string = executeCMDCommand($"curl \"ftp://{ip_users}/upgrade_result\" -o upgrade_result");
                                            if (!System.IO.File.Exists("upgrade_result"))
                                            {
                                                // ���2s��û�л�ȡ������ļ�ֱ�ӱ�����ʧ��
                                                backgroundworker_firmwareupgrade.CancelAsync();
                                                output_string = "δ��⵽upgrade_result�ļ�������ʧ�ܣ�\n";
                                                break;
                                            }
                                            // ��⵽���ļ�������cat��ȡ����
                                            output_string = executeCMDCommand("type upgrade_result");
                                            if (output_string.Contains("start"))
                                            {
                                                continue;
                                            }
                                            if (output_string.Contains("success") || output_string.Contains("fail"))
                                            {
                                                progress_i = 100;
                                                backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                                backgroundworker_firmwareupgrade.CancelAsync();
                                                break;
                                            }
                                            progress_i += 5;
                                            if (progress_i >= 100)
                                            {
                                                progress_i = 100;
                                            }
                                            System.Threading.Thread.Sleep(500);
                                            output_string = "���������У����Ժ�";
                                            backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output_string = $"���������쳣���쳣����Log���£�\n{ex.ToString()}\n";
            }
            
        }

        /**
         *  BackgroundWorker�Ľ��ȸı������ʵʱ�ϱ���ʱ�����չ������˴�����UI����
         */
        private void backgroundworker_firmwareupgrade_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            upgrade_progressbar.Value = e.ProgressPercentage;
            output_rich_textbox.AppendText($"�������ȣ�{Convert.ToString(e.ProgressPercentage)}%\n");


            // ��ʱ����ִ��cmd������֤
            output_rich_textbox.AppendText(output_string);
        }

        /**
         *  BackgroundWorker��̨�¼�����ʱ������ɺ�Ĳ������˴���Ϊ����UI������������β�����ݻ��գ������ӳ
         */
        private void backgroundworker_firmwareupgrade_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error!=null)
            {
                output_rich_textbox.AppendText(e.Error.ToString());
                return;
            }
            if (!e.Cancelled)
            {
                output_rich_textbox.AppendText("����������������鿴�����\n");
                Font font = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);
                if (output_string.Contains("success"))
                {
                    output_rich_textbox.ForeColor = Color.Green;
                    output_rich_textbox.SelectionFont = font;
                }
                else if (output_string.Contains("fail"))
                {
                    output_rich_textbox.ForeColor = Color.Red;
                    output_rich_textbox.SelectionFont = font;
                }
                output_rich_textbox.AppendText(output_string);
                if (output_string.Contains("Upgrade finish"))
                {
                    output_rich_textbox.AppendText("�ȴ�20s�豸���������У��ڼ��޷��������ߡ���\n");
                    System.Threading.Thread.Sleep(20000);
                    // ������������������Ҫ���������豸���豸״̬�Ǳ���Ҫͬ������
                    device_disconnect_button_Click(null, null);
                }
                output_rich_textbox.AppendText("�豸������ɣ�\n");
                check_current_firmware_button.Enabled = false;
                upgrade_button.Enabled = false;
                getCurrentSN_button.Enabled = false;
                writeIn_button.Enabled = false;
                getCurrentPCBA_button.Enabled = false;
                writeInPCBA_button.Enabled = false;
                login_button.Enabled = false;
                login_button.Text = "��¼";
                login_button.Enabled = true;
                start_array_mic_audio_level_test_button.Enabled = false;
                stop_array_mic_audio_level_test_button.Enabled = false;
                gain_array_mic_audio_level_button.Enabled = false;
                gainCurrentVersion_button.Enabled = false;
                device_reset_button.Enabled = false;
                rebootDevice_button.Enabled = false;
                stop_rg_flicker_button.Enabled = false;
                start_rg_flicker_button.Enabled = false;
                get_poe_mic_info_button.Enabled = false;
                device_disconnect_button_Click(null, null);
            }
            else
            {
                output_rich_textbox.AppendText("������ֹ��\n");
            }
        }
        
        // ��ȡ��ǰ�������ӵ������豸�����ڲ�ɸѡ��Seewo���豸
        /**
         *  ˢ�����ڵ���¼�����ȡ��ǰ�豸������������IP��ַ
         */
        private void getSeewoDevice_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�������ȡ��ǰ�������ӵ������豸�����ڲ�ɸѡ��Seewo���豸����\n");
            try
            {
                
                foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                {
                    output_rich_textbox.AppendText($"�ӿ�����{netItem.Name}���ӿ����ͣ�{netItem.NetworkInterfaceType}��" +
                        $"�ӿ�MAC��{netItem.GetPhysicalAddress().ToString()}\n");
                    foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                    {
                        output_rich_textbox.AppendText($"   �ӿ�����{netItem.Name}��IP��{ipIntProp.Address.ToString()}��IP���ͣ�{ipIntProp.Address.AddressFamily}\n");
                    }
                }

                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints = ipGlobalProperties.GetActiveTcpListeners();
                foreach (IPEndPoint endPoint in endPoints)
                {
                    output_rich_textbox.AppendText($"�˿ڣ�{endPoint.Port}��IP��{endPoint.Address.ToString()}��IP���ͣ�{endPoint.Address.AddressFamily.ToString()}\n");
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"��ȡ�豸ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // ��ȡ��ǰ�豸��SN��
        /**
         *  ��ȡ��ǰ�豸���кŰ�ť����¼�
         */
        private void getCurrentSN_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�������ȡ��ǰ�豸��SN�š���\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // ��ȡSN��
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"SN\" : (.*)");
                    string currentSN = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("��ǰ�豸��SN��Ϊ��" + currentSN + "\n");
                    currentSN_textbox.Text = currentSN;
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "�ѶϿ�";
                    output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
                }

            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"��ȡ��ǰ�豸��SN��ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }


        // д��ָ�����豸SN��
        /**
         * д�����кŰ�ť����¼�
         */
        private void writeIn_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�����д��ָ�����豸SN�š���\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {

                    // д��ָ�����豸SN��
                    string writeINSN = writeInSN_textbox.Text;
                    if (string.IsNullOrEmpty(writeINSN) || writeINSN.Length != 22 || !new Regex("^[A-Z|0-9]+$").IsMatch(writeINSN))
                    {
                        writeInSN_textbox.Text = "��д����ȷ��SN���ٽ���ˢ�룬ʾ����FCSC03V000019179E00001��ֻ�ܰ�����д��ĸ�����֣��ҳ���Ϊ22λ��\n";
                        output_rich_textbox.AppendText("��д����ȷ��SN���ٽ���ˢ�룬ʾ����FCSC03V000019179E00001��ֻ�ܰ�����д��ĸ�����֣��ҳ���Ϊ22λ��\n");
                    }
                    else 
                    {
                        output_rich_textbox.AppendText(session);
                        string writeDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"SN\\\": \\\"{writeINSN}\\\"}}}}\"";
                        output_string = executeCMDCommand(writeDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                        // ��ȡSN��
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        MatchCollection results_2 = Regex.Matches(output_string, "\"SN\" : (.*)");
                        string currentSN = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                        output_rich_textbox.AppendText("��ȡ��ǰ�豸��SN�ţ�" + currentSN + "\n");
                        if (currentSN == writeINSN && Int32.Parse(backCode) == 0)
                        {
                            output_rich_textbox.AppendText($"��ǰд����Ϊ��PASS����ǰSN�ţ�{currentSN}\n");
                            writeInSN_textbox.Text = currentSN;
                        }
                        else
                        {
                            output_rich_textbox.AppendText($"��ǰд����Ϊ��FAIL����ǰSN�ţ�{currentSN}\n");
                            writeInSN_textbox.Text = currentSN;
                        }
                    }
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "�ѶϿ�";
                    output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"д��ָ�����豸SN��ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // ����ת��ΪSHA256�����ַ��� 
        public static string SHA256EncryptString(string data)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);

                    using (SHA256 sha256 = SHA256.Create())
                    {
                        if (sha256 != null)
                        {
                            byte[] hash = sha256.ComputeHash(bytes);
                            if (hash.Length > 0)
                            {
                                for (int i = 0; i < hash.Length; i++)
                                {
                                    builder.Append(hash[i].ToString("x2"));
                                }
                                return builder.ToString();
                            }
                        }
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                builder.Clear();
                return "";
            }
            finally
            {
                builder.Clear();
            }
        }
        string session;
        // �����¼���������SH256����ת��
        /**
         * Web�˵�¼У�麯������Ҫ�Ƕ��������SHA256����ת��
         */
        private void login_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в����������¼���������SH256����ת������\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    try
                    {
                        string username = username_textbox.Text;
                        string password = password_textbox.Text;
                        string password_sha256 = SHA256EncryptString(password);
                        output_rich_textbox.AppendText($"��¼�ɹ������Խ��й̼�������⣺{password_sha256}\n");
                        // ��¼������ȡsession - ����IP�Ժ�������Ϊ������ǰ�ݶ�
                        string loginCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"login\\\", \\\"username\\\": \\\"{username}\\\",\\\"password\\\": \\\"{password_sha256}\\\"}}\"";
                        output_string = executeCMDCommand(loginCommand);
                        MatchCollection results = Regex.Matches(output_string, "\"session\" : (.*)");
                        session = results[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                        output_rich_textbox.AppendText("��ǰ�̼�У��Session�ǣ�" + session + "\n");
                        check_current_firmware_button.Enabled = true;
                        upgrade_button.Enabled = true;
                        getCurrentSN_button.Enabled = true;
                        writeIn_button.Enabled = true;
                        getCurrentPCBA_button.Enabled = true;
                        writeInPCBA_button.Enabled = true;
                        start_array_mic_audio_level_test_button.Enabled = true;
                        stop_array_mic_audio_level_test_button.Enabled = true;
                        gain_array_mic_audio_level_button.Enabled = true;
                        gainCurrentVersion_button.Enabled = true;
                        login_button.Text = "�ѵ�¼";
                        login_button.Enabled = false;
                        stop_rg_flicker_button.Enabled = false;
                        start_rg_flicker_button.Enabled = true;
                        get_poe_mic_info_button.Enabled = true;
                        // ���Ӽ�סusername��password����
                        if (rememberCheckBox.Checked == true)
                        {
                            FileStream fileStream = new FileStream("data1.bin", FileMode.OpenOrCreate);
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            User user = new User();
                            user.Username = username;
                            user.Password = password;
                            if (users.ContainsKey(user.Username))
                            {
                                // ������ھ������
                                users.Clear();
                            }
                            users.Add(user.Username, user);
                            binaryFormatter.Serialize(fileStream, users);
                            fileStream.Close(); 
                        }
                    }
                    catch (Exception ex)
                    {
                        output_rich_textbox.AppendText($"��¼ʧ�ܣ���������û������������������飺\n excetion:{ex.ToString()}\n");
                    }
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�����¼���������SH256����ת��ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // ��ȡ��ǰ�豸PCBA��
        /**
         * ��ȡ��ǰ�豸PCBA�Ű�ť����¼�
         */
        private void getCurrentPCBA_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�������ȡ��ǰ�豸PCBA�š���\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // ��ȡ��ǰ�豸PCBA��
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"PCBA\" : (.*)");
                    string currentPCBA = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                    output_rich_textbox.AppendText("��ȡ��ǰ�豸PCBA�ţ�" + currentPCBA + "\n");
                    currentPCBA_textbox.Text = currentPCBA;
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "�ѶϿ�";
                    output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
                }

            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"��ȡ��ǰ�豸PCBA��ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // д��ָ��PCBA��
        /**
         * д��PCBA�ŵ���¼�
         */
        private void writeInPCBA_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�����д��ָ��PCBA�š���\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {

                    // д��ָ��PCBA��
                    string writeINPCBA = writeINPCBA_textbox.Text;
                    if (string.IsNullOrEmpty(writeINPCBA) || writeINPCBA.Length != 19 || !new Regex("^[A-Z|0-9]+$").IsMatch(writeINPCBA))
                    {
                        writeINPCBA_textbox.Text = "��д����ȷ��PCBA���ٽ���ˢ�룬ʾ����ABCDEFGHI0123456789��ֻ�ܰ�����д��ĸ�����֣��ҳ���Ϊ19λ��\n";
                        output_rich_textbox.AppendText("��д����ȷ��PCBA���ٽ���ˢ�룬ʾ����ABCDEFGHI0123456789��ֻ�ܰ�����д��ĸ�����֣��ҳ���Ϊ19λ��\n");
                    }
                    else
                    {
                        output_rich_textbox.AppendText(session);
                        string writeDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"PCBA\\\": \\\"{writeINPCBA}\\\"}}}}\"";
                        output_string = executeCMDCommand(writeDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                        // ��ȡPCBA��
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        MatchCollection results_2 = Regex.Matches(output_string, "\"PCBA\" : (.*)");
                        string currentPCBA = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                        output_rich_textbox.AppendText("��ȡ��ǰ�豸��PCBA�ţ�" + currentPCBA + "\n");
                        if (currentPCBA == writeINPCBA && Int32.Parse(backCode) == 0)
                        {
                            output_rich_textbox.AppendText($"��ǰд����Ϊ��PASS����ǰPCBA�ţ�{currentPCBA}\n");
                            writeINPCBA_textbox.Text = currentPCBA;
                        }
                        else
                        {
                            output_rich_textbox.AppendText($"��ǰд����Ϊ��FAIL����ǰSN�ţ�����3��д��Ҳ��ʧ�ܣ�Ӳ��ֻ��д���Σ�{currentPCBA}\n");
                            writeINPCBA_textbox.Text = currentPCBA;
                        }
                    }
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "�ѶϿ�";
                    output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"д��ָ��PCBA��ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // ��ȡ��ǰ�豸�汾
        /**
         *  ��ȡ��ǰ�豸�Ĺ̼��汾�Ű�ť����¼�
         */
        private void gainCurrentVersion_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в�������ȡ��ǰ�豸�汾����\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // У��̼� - ��session�л�ȡ�̼���ǰ�汾
                    string checkVersionCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\":\\\"getParam\\\",\\\"session\\\":\\\"{session}\\\",\\\"name\\\":\\\"DevInfo\\\"}}\"";
                    output_string = executeCMDCommand(checkVersionCommand);
                    MatchCollection results_2 = Regex.Matches(output_string, "\"SoftwaveVersion\" : (.*)");
                    string currentVersion = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("��ǰ�汾�ǣ�" + currentVersion + "\n");
                    currentVersion_label.Text = currentVersion;
                }
                catch (Exception ex)
                {
                    currentVersion_label.Text = "��ȡ��ǰ�豸�汾ʧ��";
                    output_rich_textbox.AppendText($"��ȡ��ǰ�豸�汾ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // �����豸����
        /**
         * �����豸��ť����¼�
         */
        private void rebootDevice_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в����������豸����\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // �����豸����
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"control\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"reboot\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "�����豸����δִ�гɹ�";
                    if (back_code == "0")
                    {
                        result = "�ɹ�";
                        device_disconnect_button_Click(null, null);
                        output_rich_textbox.AppendText("�ȴ�20s�豸���������У��ڼ��޷��������ߡ���\n");
                        System.Threading.Thread.Sleep(20000);
                    }
                    else
                    {
                        result = "ʧ��";
                    }
                    output_rich_textbox.AppendText("�����豸�����" + result + "\n");
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "�ѶϿ�";
                    output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"�����豸ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }
    }
}