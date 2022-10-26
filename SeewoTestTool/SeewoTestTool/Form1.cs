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

        // ��֤�豸������״̬ ����ͨ��
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            string host = device_ip_textbox.Text;
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
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, int.Parse(port));
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.Connect(ipe);
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
                    MessageBox.Show($"�豸δ���ӣ����飡\nIP:{host}�� PORT:{port}");
                }

            }
        }

        // ��cmd��ִ���������
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
        private void send_Str(string sendStr)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                // Socket��������
                try
                {
                    output_rich_textbox.AppendText($"ָ���:{sendStr}\n");
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
        private string receive_Str()
        {
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
        private void device_disconnect_button_Click(object sender, EventArgs e)
        {
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
        private void choose_upgrade_firmware_button_Click(object sender, EventArgs e)
        {
            //if(true)
            if (clientSocket != null && clientSocket.Connected)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                output_rich_textbox.AppendText("ѡ�������̼���\n");
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
        private void upgrade_button_Click(object sender, EventArgs e)
        {
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
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                output_rich_textbox.AppendText("У��̼����ԣ�\n");
                string currentFirmware = null;
                try
                {
                    // У��̼� - ��session�л�ȡ�̼���ǰ�汾
                    string checkVersionCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\":\\\"getParam\\\",\\\"session\\\":\\\"{session}\\\",\\\"name\\\":\\\"DevInfo\\\"}}\"";
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
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("�򿪺��̵ƽ�����˸\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = true;
                    start_rg_flicker_button.Enabled = false;

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
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("�رպ��̵ƽ�����˸\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = true;

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
        private void get_poe_mic_info_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ȡ������Ϣ\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

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
        private void audioin1_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("Audio IN 1�ڲ���\n");
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
        private void audioin2_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("Audio IN 2�ڲ���\n");
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
        private void array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��������MIC����ֵ����\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // ��������MIC����ֵ����
                    output_rich_textbox.AppendText(session);
                    string beginDeviceMICVolumeTestCommand = $"curl -X POST \"http://10.66.30.69/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"startAudioTest\\\",\\\"format\\\": 0,\\\"soundmode\\\": 8,\\\"samplerate\\\": 16000,\\\"periodsize\\\": 1024}}\"";
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
        private void stop_array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("ֹͣ����MIC����ֵ����\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // ֹͣ����MIC����ֵ����
                    output_rich_textbox.AppendText(session);
                    string stopDeviceMICVolumeTestCommand = $"curl -X POST \"http://10.66.30.69/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"stopAudioTest\\\"}}\"";
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
        private void gain_array_mic_audio_level_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ȡ��·MIC��Ƶ����ֵ\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

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
        private void device_reset_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("�豸��λ\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�豸��λʧ�ܣ�\n{ex.ToString()}\n");
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

        // ����ر��¼������豸socket�����ͷŵ�
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("����ر��¼������豸socket�����ͷŵ�\n");
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
        private void richTextChanged_to(object sender, EventArgs e)
        {
            // �����λ�����õ���ǰ���ݵ�ĩβ
            output_rich_textbox.SelectionStart = output_rich_textbox.Text.Length;
            // ���������λ��
            output_rich_textbox.ScrollToCaret();
        }

        // ����������
        private void clear_output_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(output_rich_textbox.Text))
            {
                output_rich_textbox.Text = "";
                output_rich_textbox.ForeColor = Color.Black;
                Font font2 = new Font(FontFamily.GenericMonospace, 9, FontStyle.Regular);
                output_rich_textbox.Font = font2;
            }
        }

        string output_string = "";
        // �½�backgroundworker���̼�������������ֹUI������������
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
                                        output_string = executeCMDCommand($"curl -T {filePath} \"ftp://10.66.30.69/\"");
                                        progress_i += 50;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Pushing\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // �������� - �����ļ��� need_upgrade
                                        if (!System.IO.Directory.Exists("need_upgrade"))
                                        {
                                            output_string = executeCMDCommand("touch need_upgrade");
                                            progress_i += 10;
                                            backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Createing\n");
                                            System.Threading.Thread.Sleep(1000);
                                        }

                                        // �������� - ��ʼ����
                                        output_string = executeCMDCommand("curl -T need_upgrade \"ftp://10.66.30.69/\"");
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
                                            output_string = executeCMDCommand("curl \"ftp://10.66.30.69/upgrade_result\" -o upgrade_result");
                                            if (!System.IO.File.Exists("upgrade_result"))
                                            {
                                                // ���2s��û�л�ȡ������ļ�ֱ�ӱ�����ʧ��
                                                backgroundworker_firmwareupgrade.CancelAsync();
                                                output_string = "δ��⵽upgrade_result�ļ�������ʧ�ܣ�\n";
                                                break;
                                            }
                                            // ��⵽���ļ�������cat��ȡ����
                                            output_string = executeCMDCommand("cat upgrade_result");
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

        private void backgroundworker_firmwareupgrade_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            upgrade_progressbar.Value = e.ProgressPercentage;
            output_rich_textbox.AppendText($"�������ȣ�{Convert.ToString(e.ProgressPercentage)}%\n");


            // ��ʱ����ִ��cmd������֤
            output_rich_textbox.AppendText(output_string);
        }
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
                start_array_mic_audio_level_test_button.Enabled = false;
                stop_array_mic_audio_level_test_button.Enabled = false;
                gain_array_mic_audio_level_button.Enabled = false;
            }
            else
            {
                output_rich_textbox.AppendText("������ֹ��\n");
            }
        }
        
        // ��ȡ��ǰ�������ӵ������豸�����ڲ�ɸѡ��Seewo���豸
        private void getSeewoDevice_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ȡ��ǰ�������ӵ������豸�����ڲ�ɸѡ��Seewo���豸\n");
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
        private void getCurrentSN_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ȡ��ǰ�豸��SN��:\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // ��ȡSN��
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"SN\" : (.*)");
                    string currentSN = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("��ȡ��ǰ�豸��SN�ţ�" + currentSN + "\n");
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
        private void writeIn_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("д��ָ�����豸SN��:\n");
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
                        string writeDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"SN\\\": \\\"{writeINSN}\\\"}}}}\"";
                        output_string = executeCMDCommand(writeDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                        // ��ȡSN��
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
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
        private void login_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("�����¼���������SH256����ת��\n");
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
                        string loginCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"login\\\", \\\"username\\\": \\\"{username}\\\",\\\"password\\\": \\\"{password_sha256}\\\"}}\"";
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
        private void getCurrentPCBA_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ȡ��ǰ�豸PCBA��:\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // ��ȡ��ǰ�豸PCBA��
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
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
        private void writeInPCBA_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("д��ָ��PCBA��:\n");
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
                        string writeDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"PCBA\\\": \\\"{writeINPCBA}\\\"}}}}\"";
                        output_string = executeCMDCommand(writeDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                        // ��ȡPCBA��
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
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

        
    }
}