using SeevisionTestTool;
using Sunny.UI;
using Sunny.UI.Win32;
using SXW0301_Production_line;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Application = System.Windows.Forms.Application;
using Image = System.Drawing.Image;

namespace SeewoTestTool
{
    public partial class Form1 : Form
    {
        Socket clientSocket;
        Dictionary<string, InternetPort> internetPorts = new Dictionary<string, InternetPort>();
        Dictionary<string, User> users = new Dictionary<string, User>();
        Dictionary<string, TestResult> testResults = new Dictionary<string, TestResult>();
        TestResult testResult_Tester = new TestResult();
        AudioPlayControl audioPlayControl = new AudioPlayControl();
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
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            // ����tabPage1 - ����SN��PCBAˢд�Ĺ��ܣ�ֻ�������ڲ�����ʹ��
            this.tabPage1.SetDisabled();
            refreshTestResult_button_Click(null, null);
            uiButton1.FillColor = Color.Red;
            uiButton1.FillHoverColor = Color.MediumVioletRed;
            uiButton1.FillPressColor = Color.DarkRed;
            output_rich_textbox.AppendText("��ӭʹ���������Ա궨�����\n");
            uiLabel20.ForeColor = Color.Red;
            uiLabel21.ForeColor = Color.Red;
            uiLabel23.ForeColor = Color.Red;
            uiLabel26.ForeColor = Color.Red;
            seewoProductPN_label.ForeColor = Color.Blue;
            seewoProductSN_label.ForeColor = Color.Blue;
            seewoWorkOrder_label.ForeColor = Color.Blue;
            seewoCustomerPN_label.ForeColor = Color.Blue;
            currentMac_label.ForeColor = Color.Blue;
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
            // �˺�����洢��ȡ
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

            // ���Խ���洢��ȡ
            refreshTestResult_button_Click(null, null);


            fs.Close();
            fs1.Close();
            // �����Զ������豸����
            //device_connect_button_Click(null, null);
            //login_button_Click(null, null);
        }

        private readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        private void CallBackMethod(IAsyncResult asyncresult)
        {
            //ʹ�������̼߳��� 
            TimeoutObject.Set();

        }

        // ����豸�Ƿ�����
        private bool check_device_online()
        {
            string host = device_ip_textbox.Text;
            // ������ping��ֱͨ��FAIL�������¼���ִ��
            try
            {
                string temp_check_ping_ip_exists = executeCMDCommand($"ping {host} -n 1");
                if (temp_check_ping_ip_exists.Contains("����ʱ"))
                {
                    output_rich_textbox.AppendText("�豸IP����ʧ�ܣ���ȷ���豸������������Լ����绷�������Ƿ����������ԣ�\n");
                    //MessageBox.Show("�豸IP����ʧ�ܣ���ȷ���豸������������Լ����绷�������Ƿ����������ԣ�\n"); 
                    return false;
                }
                else
                {
                    return true;
                }

                // �ж�����ǣ�219.198.235.11����Ҫ�������豸ǰ����ping������ͨ·�ɣ�������ǣ�����Ҫping 219.198.235.11 -t -S 219.198.235.17
                if (host == "219.198.235.11")
                {
                    string localPCHost = null;
                    foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                        {
                            string inetName = netItem.Name;
                            string inetAddress = ipIntProp.Address.ToString();
                            string inetType = ipIntProp.Address.AddressFamily.ToString();
                            //output_rich_textbox.AppendText($"   �ӿ�����{inetName}��IP��{inetAddress}��IP���ͣ�{inetType}\n");
                            if (inetName == "��̫��" && inetType == "InterNetwork")
                            {
                                localPCHost = inetAddress;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(localPCHost))
                    {
                        output_rich_textbox.AppendText($"��ǰ����Ϊ������ֱ��������PC��IP��ַΪ��{localPCHost}\n");
                        string back_temp = executeCMDCommand($"ping {host} -t -S {localPCHost} -n 1");
                        output_rich_textbox.AppendText($"��ǰ����Ϊ������ֱ����IP��219.198.235.11\nPING ��ͨĬ��·�ɽ��Ϊ��{back_temp}\n");
                    }
                    else
                    {
                        MessageBox.Show("�����豸�������ӣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                output_rich_textbox.AppendText("������豸�Ƿ����ߡ���������\n");
                return true;
            }

        }

        string ip_users;
        string host;
        // ��֤�豸������״̬ ����ͨ��

        /**
         * �����豸����¼�
         */
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("��ִ�в����������豸�����ӡ���\n");
            buildRoute();
            if (check_device_online())
            {
                host = device_ip_textbox.Text;
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
                ip_users = host + ":" + port;
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
                        clientSocket.BeginConnect(ipe, new AsyncCallback(CallBackMethod), clientSocket);
                        if (TimeoutObject.WaitOne(5000, false))
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
                                button1.Enabled = true;
                                button2.Enabled = true;
                                upgrade_progressbar.Value = 0;
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
                                Thread.Sleep(1);
                                login_button_Click(null, null);
                                // ���λ
                                refreshTestResult_button_Click(null, null);
                                testResults["���Խ��"].NetworkTestResult = "PASS";
                                writeTestResult();
                            }
                        }
                        else
                        {
                            // ���λ
                            refreshTestResult_button_Click(null, null);
                            testResults["���Խ��"].NetworkTestResult = "FAIL";
                            writeTestResult();
                            output_rich_textbox.AppendText("����ʧ�ܣ������������ӣ�\n");
                            MessageBox.Show($"���ӳ�ʱ�����飡\nIP:{host}�� PORT:{port}\n���������ӳ����޹����ɳ��������豸���������Ӳ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        }

        // ��cmd��ִ���������
        /**
         *  �����ַ������������CMD��ִ����Ӧ��ָ��
         */
        private string executeCMDCommand(string command)
        {
            Process process_cmd = new Process();
            string output_string = null;
            try
            {
                process_cmd.StartInfo.FileName = "cmd.exe";
                process_cmd.StartInfo.RedirectStandardInput = true;
                process_cmd.StartInfo.RedirectStandardOutput = true;
                process_cmd.StartInfo.CreateNoWindow = true;
                process_cmd.StartInfo.UseShellExecute = false;
                process_cmd.Start();
                process_cmd.StandardInput.WriteLine(command + "&exit");
                process_cmd.StandardInput.AutoFlush = true;
                output_string = process_cmd.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                output_string = ex.ToString();
            }
            finally
            {
                process_cmd.WaitForExit();
                process_cmd.Close();
            }
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
                    device_disconnect_button_Click(null, null);
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
                        device_disconnect_button_Click(null, null);
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
            if (check_device_online())
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    try
                    {
                        output_rich_textbox.AppendText("�Ͽ���ǰ�������豸\n");
                        stop_array_mic_audio_level_test_button_Click(null, null);
                        device_disconnect_button.Enabled = false;
                        device_connect_button.Enabled = true;
                        clientSocket.Close();
                        clientSocket = null;
                        upgrade_progressbar.Value = 0;
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
                        gain_array_mic_audio_level_button.Enabled = false;
                        gainCurrentVersion_button.Enabled = false;
                        login_button.Text = "��¼";
                        login_button.Enabled = true;
                        device_reset_button.Enabled = false;
                        rebootDevice_button.Enabled = false;
                        stop_rg_flicker_button.Enabled = false;
                        start_rg_flicker_button.Enabled = false;
                        get_poe_mic_info_button.Enabled = false;
                        login_button.Enabled = false;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        audioIn1_test_button.Enabled = false;
                        audioIn2_test_button.Enabled = false;
                        poe1NetworkTest_button.Enabled = false;
                        openLiveCamera_buttton.Enabled = false;
                        openMergeCamera_buttton.Enabled = false;
                        writeInMac_button.Enabled = false;
                        beginAudioRecord_button.Enabled = false;
                        extractRecordFile_button.Enabled = false;
                        openLiveCameraPASS_buttton.Enabled = false;
                        openLiveCameraFAIL_buttton.Enabled = false;
                        openMergeCameraPASS_buttton.Enabled = false;
                        openMergeCameraFAIL_buttton.Enabled = false;
                        redGreenPASS_button.Enabled = false;
                        redGreenFAIL_button.Enabled = false;
                        getCurrentMacAddress_button.Enabled = false;
                        beginResetTest_button.Enabled = false;
                        enterAgingMode_button.Enabled = false;
                        stopPinkNoise_button.Enabled = false;
                        login_button.Text = "�豸���Ӻ�\n���Զ���¼";
                        if (autoTest_t != null)
                        {
                            autoTest_t.Interrupt();
                            autoTest_t = null;
                        }
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
            if (check_device_online())
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
                            // ������ع��ܣ���ֹ�û���������ʱ������������������������ʧ��
                            uiGroupBox1.Enabled = false;
                            uiGroupBox7.Enabled = false;
                            uiGroupBox9.Enabled = false;
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
                            device_disconnect_button_Click(null, null);
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

        }

        // У�鵱ǰ�̼�
        /**
         * ���鵱ǰ�̼����汾��ť����¼�
         */
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
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

                                                // ���λ
                                                refreshTestResult_button_Click(null, null);
                                                testResults["���Խ��"].FirmwareVerifiedResult = "PASS";
                                                writeTestResult();

                                                output_rich_textbox.AppendText($"�̼�У��ɹ�����ǰ�̼��汾��{currentVersion}\n");
                                            }
                                            else
                                            {
                                                output_rich_textbox.ForeColor = Color.Red;
                                                output_rich_textbox.SelectionFont = font;
                                                checked_firmware_textbox.Text = $"�̼�У��ʧ�ܣ���ǰ�̼��汾��{currentVersion}";

                                                // ���λ
                                                refreshTestResult_button_Click(null, null);
                                                testResults["���Խ��"].FirmwareVerifiedResult = "FAIL";
                                                writeTestResult();

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

        }


        // �򿪺��̵ƽ�����˸
        /**
         * �򿪺���ָʾ�ƽ�����˸��ť����¼�
         */
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
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
                        output_rich_textbox.AppendText("�򿪺��̵ƽ�����˸�����" + result + "��back_code����" + back_code + "\n");

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

        }

        // �رպ��̵ƽ�����˸
        /**
         * �رպ���ָʾ�ƽ�����˸��ť����¼�
         */
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
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

        }

        // ��ȡ������Ϣ
        /**
         * ��ȡ������Ϣ��ť����¼�
         */
        private void get_poe_mic_info_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("�ù���Ŀǰδʵ�֣�Ԥ��λ������������ʵ�֣�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        string micHardwareVersion = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
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
            if (check_device_online())
            {
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
                            gain_array_mic_audio_level_button.Enabled = true;
                            audioIn1_test_button.Enabled = true;
                            audioIn2_test_button.Enabled = true;
                        }
                        else if (Int32.Parse(backCode) == -1)
                        {
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL��δ��������MIC����ֵ���ԣ�backCode:[{backCode}]\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"��������MIC����ֵ����ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n");
                        output_rich_textbox.AppendText($"��������MIC����ֵ����ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n");
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

        }

        // ֹͣ����MIC����ֵ����
        /**
         * ֹͣ������˷�����ֵ����
         */
        private void stop_array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
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
                            gain_array_mic_audio_level_button.Enabled = false;
                            audioIn1_test_button.Enabled = false;
                            audioIn2_test_button.Enabled = false;
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��PASS����ֹͣ����MIC����ֵ���ԣ�backCode:[{backCode}]\n");
                        }
                        else if (Int32.Parse(backCode) == -1)
                        {
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL���޷��ر�����MIC����ֵ���ԣ�backCode:[{backCode}]\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"ֹͣ����MIC����ֵ����ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n");
                        output_rich_textbox.AppendText($"ֹͣ����MIC����ֵ����ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n");
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

        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked == true)
            {
                foreach (CheckBox chk in (sender as CheckBox).Parent.Controls)
                {
                    if (chk != sender)
                    {
                        chk.Checked = false;
                    }
                }
            }
        }


        Thread bareBoardTest_t;
        private void bareBoardTest_func()
        {
            Thread.Sleep(5000);
            refreshTestResult_button_Click(null, null);
            testResults["���Խ��"].ArrayMicResult = "��δ����";
            writeTestResult();
            recordingGif_label.Image = Image.FromFile("./img/recordingFinish.png");
        }

        Thread gainingArrayMic_t;

        private void gainingArrayMicLevel_func()
        {
            try
            {
                if (bareBoardTest_t != null && !bareBoardTest_t.IsBackground)
                {
                    bareBoardTest_t.Interrupt();
                    bareBoardTest_t = null;
                }
                else if (bareBoardTest_t != null && bareBoardTest_t.IsBackground)
                {
                    MessageBox.Show("ǰһ��MIC������������У����Ժ�");
                    return;
                }
            }
            catch (Exception ex)
            {

            }
            duration = recordTime_textbox.Text;
            recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
            if (bareBoardTest_checkBox.Checked || mothreBoardTest_checkBox.Checked)
            {
                standardAudioVolume_textbox.Text = "0";
            }
            //if (true)
            if (String.IsNullOrEmpty(standardAudioVolume_textbox.Text) || !new Regex("^[0-9]+$").IsMatch(standardAudioVolume_textbox.Text))
            {
                MessageBox.Show("�궨����ֵ����Ϊ�գ�\n���������˷����ֵ����ݣ����������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                standardAudioVolume_textbox.Text = "";
            }
            else if (check_device_online())
            {
                gain_array_mic_audio_level_button.Enabled = false;
                // ÿ�λ�ȡMIC��Ƶ����ֵ��Ҫ��¼��1s���ȡ
                recordTime_textbox.Text = "1";
                beginAudioRecord_button_Click(null, null);
                audioRecord_t.Join();
                float standard_volume = float.Parse(standardAudioVolume_textbox.Text);
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
                        output_rich_textbox.AppendText(results_1.ToString());
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                        if (Int32.Parse(backCode) == 0)
                        {
                            MatchCollection results_2 = Regex.Matches(output_string, "\\\"rmsdb\\\" : \\[\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*)\\n   ]");
                            string[] temp = results_2[0].ToString().Replace("\"rmsdb\" : [", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Replace(" ", "").Split(",");
                            string volume1 = temp[0].ToString();
                            string volume2 = temp[1].ToString();
                            string volume3 = temp[2].ToString();
                            string volume4 = temp[3].ToString();
                            string volume5 = temp[4].ToString();
                            string volume6 = temp[5].ToString();
                            string volume7 = temp[6].ToString();
                            string volume8 = temp[7].ToString();
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��PASS����ȡ��·MIC��Ƶ����ֵ��backCode:[{backCode}]\n");
                            volume1_value_label.Text = volume1;
                            volume2_value_label.Text = volume2;
                            volume4_value_label.Text = volume4;
                            volume5_value_label.Text = volume5;
                            volume6_value_label.Text = volume6;
                            volume8_value_label.Text = volume8;
                            output_rich_textbox.AppendText($"rmsdb1��{volume1}\nrmsdb2��{volume2}\nrmsdb3��{volume3}\nrmsdb4��{volume4}\nrmsdb5��{volume5}\nrmsdb6��{volume6}\nrmsdb7��{volume7}\nrmsdb8��{volume8}\n");

                            float volume_1_f = float.Parse(volume1);
                            float volume_2_f = float.Parse(volume2);
                            float volume_3_f = float.Parse(volume3);
                            float volume_4_f = float.Parse(volume4);
                            float volume_5_f = float.Parse(volume5);
                            float volume_6_f = float.Parse(volume6);
                            float volume_7_f = float.Parse(volume7);
                            float volume_8_f = float.Parse(volume8);

                            float[] volumes_f = { volume_1_f, volume_2_f, volume_4_f, volume_5_f, volume_6_f, volume_8_f };
                            float maxINArray = volumes_f.Max();
                            float minINArray = volumes_f.Min();
                            if (bareBoardTest_checkBox.Checked)
                            {
                                if (Math.Abs(minINArray) >= 15 && Math.Abs(maxINArray) <= 65 && Math.Abs(maxINArray - minINArray) <= 5)
                                {
                                    // ���λ
                                    refreshTestResult_button_Click(null, null);
                                    testResults["���Խ��"].ArrayMicResult = "PASS";
                                    writeTestResult();
                                }
                                else
                                {
                                    // ���λ
                                    refreshTestResult_button_Click(null, null);
                                    testResults["���Խ��"].ArrayMicResult = "FAIL";
                                    writeTestResult();
                                }
                                /**
                                if (bareBoardTest_t != null)
                                {
                                    //MessageBox.Show("���Ժ�");
                                }
                                else
                                {
                                    bareBoardTest_t = new Thread(bareBoardTest_func);
                                    bareBoardTest_t.IsBackground = true;
                                    bareBoardTest_t.Start();
                                }
                                */
                                bareBoardTest_t = new Thread(bareBoardTest_func);
                                bareBoardTest_t.IsBackground = true;
                                bareBoardTest_t.Start();
                            }
                            else if (mothreBoardTest_checkBox.Checked)
                            {
                                if (Math.Abs(minINArray) >= 15 && Math.Abs(maxINArray) <= 65 && Math.Abs(maxINArray - minINArray) <= 5)
                                {
                                    // ���λ
                                    refreshTestResult_button_Click(null, null);
                                    testResults["���Խ��"].ArrayMicResult = "PASS";
                                    writeTestResult();
                                }
                                else
                                {
                                    // ���λ
                                    refreshTestResult_button_Click(null, null);
                                    testResults["���Խ��"].ArrayMicResult = "FAIL";
                                    writeTestResult();
                                }
                            }
                            else
                            {
                                if (Math.Abs(Math.Abs(standard_volume) - Math.Abs(volume_1_f)) <= 2 && Math.Abs(Math.Abs(standard_volume) - Math.Abs(volume_2_f)) <= 2
                                && Math.Abs(Math.Abs(standard_volume) - Math.Abs(volume_4_f)) <= 2 && Math.Abs(Math.Abs(standard_volume) - Math.Abs(volume_5_f)) <= 2
                                && Math.Abs(Math.Abs(standard_volume) - Math.Abs(volume_6_f)) <= 2 && Math.Abs(Math.Abs(standard_volume) - Math.Abs(volume_8_f)) <= 2)
                                {
                                    // ���λ
                                    refreshTestResult_button_Click(null, null);
                                    testResults["���Խ��"].ArrayMicResult = "PASS";
                                    writeTestResult();
                                }
                                else
                                {
                                    // ���λ
                                    refreshTestResult_button_Click(null, null);
                                    testResults["���Խ��"].ArrayMicResult = "FAIL";
                                    writeTestResult();
                                }
                            }
                        }
                        else if (Int32.Parse(backCode) == -1)
                        {
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL���޷���ȡ��·MIC��Ƶ����ֵ��backCode:[{backCode}]\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"��ȡ��·MIC��Ƶ����ֵʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n{ex.ToString()}");
                        output_rich_textbox.AppendText($"��ȡ��·MIC��Ƶ����ֵʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n");
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
        }

        // ��ȡ��·MIC��Ƶ����ֵ
        /**
         * ����������˷�����ֵ���Ժ󣬶�8·��˷��ֵ���л�ȡ
         */
        private void gain_array_mic_audio_level_button_Click(object sender, EventArgs e)
        {
            if (gainingArrayMic_t != null)
            {
                gainingArrayMic_t.Interrupt();
                gainingArrayMic_t = null;
                //MessageBox.Show("ǰһ��¼�����ڽ����С��������Ժ��ٽ��е����");
            }
            recordingGif_label.Visible = true;
            recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
            gainingArrayMic_t = new Thread(gainingArrayMicLevel_func);
            gainingArrayMic_t.IsBackground = true;
            gainingArrayMic_t.Start();
        }

        // �豸��λ
        /**
         * �豸��λ��ť����¼�
         */
        private void device_reset_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
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
                    if (writeInMac_t != null)
                    {
                        writeInMac_t.Interrupt();
                        writeInMac_t = null;
                    }
                    // �ͷŵ���Ƶ���Ե�����
                    stop_array_mic_audio_level_test_button_Click(null, null);
                    clientSocket.Close();
                    // �ر�SXW0301_Production_line.exe
                    executeCMDCommand("taskkill /f /t /im SeevisionTestTool.exe");
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"����رս��豸socket�����ͷŵ�ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                //executeCMDCommand("taskkill /f /t /im SeevisionTestTool.exe");
                new Regist().Show();
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
                //Font font2 = new Font(FontFamily., 9, FontStyle.Regular);
                //output_rich_textbox.Font = font2;
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
                                        string ftp_ip_user = ip_users.Split(":")[0];
                                        output_string = executeCMDCommand($"curl -T {filePath} \"ftp://{ftp_ip_user}/\"");
                                        progress_i += 30;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Pushing\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // �������� - �����ļ��� need_upgrade
                                        if (!System.IO.Directory.Exists("need_upgrade"))
                                        {
                                            output_string = executeCMDCommand("type nul> need_upgrade");
                                        }
                                        progress_i += 20;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Createing\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // �������� - ��ʼ����
                                        output_string = executeCMDCommand($"curl -T need_upgrade \"ftp://{ftp_ip_user}/\"");
                                        progress_i += 5;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // �������� - �������״̬
                                        bool upgradeDone = false;
                                        if (System.IO.File.Exists("upgrade_result"))
                                        {
                                            output_string = executeCMDCommand("del upgrade_result");
                                        }
                                        while (!upgradeDone)
                                        {
                                            output_string = executeCMDCommand($"curl \"ftp://{ftp_ip_user}/upgrade_result\" -o upgrade_result");
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
                                                //progress_i = 100;
                                                backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                                backgroundworker_firmwareupgrade.CancelAsync();
                                                break;
                                            }
                                            progress_i += 5;
                                            if (progress_i >= 100)
                                            {
                                                //progress_i = 100;
                                            }
                                            System.Threading.Thread.Sleep(500);
                                            output_string = "���������У����Ժ�\n���������������������������лл��ϣ�\n";
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
                device_disconnect_button_Click(null, null);
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


        private void buildRoute()
        {
            // �ж�����ǣ�219.198.235.11����Ҫ�������豸ǰ����ping������ͨ·�ɣ�������ǣ�����Ҫping 219.198.235.11 -t -S 219.198.235.17\
            if (host == "219.198.235.11")
            {
                string localPCHost = null;
                foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                    {
                        string inetName = netItem.Name;
                        string inetAddress = ipIntProp.Address.ToString();
                        string inetType = ipIntProp.Address.AddressFamily.ToString();
                        //output_rich_textbox.AppendText($"   �ӿ�����{inetName}��IP��{inetAddress}��IP���ͣ�{inetType}\n");
                        if (inetName == "��̫��" && inetType == "InterNetwork")
                        {
                            localPCHost = inetAddress;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(localPCHost))
                {
                    int times = 0;
                    while (true)
                    {
                        times += 1;
                        string back_temp = executeCMDCommand($"ping {host} -t -S {localPCHost} -n 1");
                        Thread.Sleep(1000);
                        output_rich_textbox.AppendText($"����ʾ����{times}�롿�ؽ�·�������������Ժ󡭡�\n");
                        if (back_temp.Contains("TTL"))
                        {
                            break;
                        }
                        if (times >= 30)
                        {
                            output_rich_textbox.AppendText("30����·������û�и���û�гɹ���������������");
                            break;
                        }
                    }
                }
            }
        }

        int lastValue;
        private void waitForReboot()
        {
            //MessageBox.Show("���Ե��豸���������С���\n", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            while (true)
            {
                Font font = new Font(FontFamily.GenericMonospace, 25, FontStyle.Bold);
                output_rich_textbox.ForeColor = Color.MediumVioletRed;
                output_rich_textbox.SelectionFont = font;
                output_rich_textbox.AppendText("���Ե��豸���������С���\n");
                Thread.Sleep(5000);
                buildRoute();
                /**
                string temp_check_ping_ip_exists = executeCMDCommand($"ping {device_ip_textbox.Text} -n 1");
                if (upgrade_progressbar.Value != 0)
                {
                    lastValue = 100 - upgrade_progressbar.Value;
                    if (lastValue != 0)
                    {
                        upgrade_progressbar.Value += 10;
                    }
                }
                */
                if (true)
                //if (temp_check_ping_ip_exists.Contains("TTL"))
                {
                    Thread.Sleep(8000);
                    thread_reboot1.Interrupt();
                    device_disconnect_button_Click(null, null);
                    output_rich_textbox.AppendText("�豸������ɣ�\n");
                    uiGroupBox1.Enabled = true;
                    uiGroupBox7.Enabled = true;
                    uiGroupBox9.Enabled = true;
                    upgrade_progressbar.Value = 100;
                    check_current_firmware_button.Enabled = false;
                    upgrade_button.Enabled = false;
                    getCurrentSN_button.Enabled = false;
                    writeIn_button.Enabled = false;
                    getCurrentPCBA_button.Enabled = false;
                    writeInPCBA_button.Enabled = false;
                    login_button.Enabled = false;
                    login_button.Text = "��¼";
                    login_button.Enabled = true;
                    gain_array_mic_audio_level_button.Enabled = false;
                    gainCurrentVersion_button.Enabled = false;
                    device_reset_button.Enabled = false;
                    rebootDevice_button.Enabled = false;
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    get_poe_mic_info_button.Enabled = false;
                    poe1NetworkTest_button.Enabled = false;
                    uiGroupBox1.Enabled = true;
                    uiGroupBox7.Enabled = true;
                    uiGroupBox9.Enabled = true;

                    Thread.Sleep(3000);
                    // ������������������Ҫ���������豸���豸״̬�Ǳ���Ҫͬ������
                    device_connect_button_Click(null, null);
                    return;
                }
            }


        }
        Thread thread_reboot1;

        /**
         *  BackgroundWorker��̨�¼�����ʱ������ɺ�Ĳ������˴���Ϊ����UI������������β�����ݻ��գ������ӳ
         */
        private void backgroundworker_firmwareupgrade_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
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
                    upgrade_button.Enabled = true;
                }
                else if (output_string.Contains("fail"))
                {
                    output_rich_textbox.ForeColor = Color.Red;
                    output_rich_textbox.SelectionFont = font;
                }
                output_rich_textbox.AppendText(output_string);
                if (output_string.Contains("Upgrade finish"))
                {
                    thread_reboot1 = new Thread(waitForReboot);
                    thread_reboot1.IsBackground = true;
                    thread_reboot1.Start();
                    /**
                    output_rich_textbox.AppendText("�ȴ�20���豸���������У��ڼ��޷��������ߡ���\n");
                    System.Threading.Thread.Sleep(20000);
                    */
                }
                else
                {
                    MessageBox.Show("�����жϣ�����������������ʧ�ܣ����飡\n��ʼ�������������豸�����Ե����ӽ������", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    output_rich_textbox.AppendText("�����жϣ�����������������ʧ�ܣ����飡\n��ʼ�������������豸�����Ե����ӽ������");
                    device_disconnect_button_Click(null, null);
                    device_connect_button_Click(null, null);
                    uiGroupBox1.Enabled = true;
                    uiGroupBox7.Enabled = true;
                    uiGroupBox9.Enabled = true;
                }
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
            output_rich_textbox.AppendText("��ִ�в�������ȡ��ǰ�������ӵ�����ͨ����̫�������ӵ��豸����\n");
            try
            {
                foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                {
                    /*
                    output_rich_textbox.AppendText($"�ӿ�����{netItem.Name}���ӿ����ͣ�{netItem.NetworkInterfaceType}��" +
                        $"�ӿ�MAC��{netItem.GetPhysicalAddress().ToString()}\n");
                    */
                    foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                    {
                        string inetName = netItem.Name;
                        string inetAddress = ipIntProp.Address.ToString();
                        string inetType = ipIntProp.Address.AddressFamily.ToString();
                        //output_rich_textbox.AppendText($"   �ӿ�����{inetName}��IP��{inetAddress}��IP���ͣ�{inetType}\n");
                        if (inetName == "��̫��" && inetType == "InterNetwork")
                        {
                            output_rich_textbox.AppendText($"   �ӿ�����{inetName}��IP��{inetAddress}��IP���ͣ�{inetType}\n");
                        }
                    }
                }
                /*
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints = ipGlobalProperties.GetActiveTcpListeners();
                foreach (IPEndPoint endPoint in endPoints)
                {
                    output_rich_textbox.AppendText($"�˿ڣ�{endPoint.Port}��IP��{endPoint.Address.ToString()}��IP���ͣ�{endPoint.Address.AddressFamily.ToString()}\n");
                }
                */
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
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в�������ȡ��ǰ�豸��SN�š���\n");
                try
                {
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        // ��ȡSN��
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"SeewoProductSN\" : (.*)");
                        string currentSN = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
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

        }


        // д��ָ�����豸SN��
        /**
         * д�����кŰ�ť����¼�
         */
        private void writeIn_button_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("������ʱ�ر��Է�ֹ�������ÿ�����ֻ������д����ᡣ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //if (true)
            if (check_device_online())
            {
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
                            string writeDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"SeewoProductSN\\\": \\\"{writeINSN}\\\"}}}}\"";
                            output_string = executeCMDCommand(writeDeviceInfoCommand);
                            MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                            string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                            // ��ȡSN��
                            string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                            output_string = executeCMDCommand(fetchDeviceInfoCommand);
                            MatchCollection results_2 = Regex.Matches(output_string, "\"SeewoProductSN\" : (.*)");
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

        Thread autoTest_t;
        private void autoTest_func()
        {
            start_rg_flicker_button_Click(null, null);
            gainCurrentVersion_button_Click(null, null);
            getCurrentMacAddress_button_Click(null, null);
            getCompleteAndBareBoardInfo_func();
        }

        string session;
        // �����¼���������SH256����ת��
        /**
         * Web�˵�¼У�麯������Ҫ�Ƕ��������SHA256����ת��
         */
        private void login_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
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
                            gain_array_mic_audio_level_button.Enabled = true;
                            gainCurrentVersion_button.Enabled = true;
                            login_button.Text = "�ѵ�¼";
                            login_button.Enabled = false;
                            stop_rg_flicker_button.Enabled = false;
                            start_rg_flicker_button.Enabled = true;
                            get_poe_mic_info_button.Enabled = true;
                            poe1NetworkTest_button.Enabled = true;
                            openLiveCamera_buttton.Enabled = true;
                            openMergeCamera_buttton.Enabled = true;
                            audioIn1_test_button.Enabled = true;
                            audioIn2_test_button.Enabled = true;
                            writeInMac_button.Enabled = true;
                            beginAudioRecord_button.Enabled = true;
                            extractRecordFile_button.Enabled = true;
                            openLiveCameraPASS_buttton.Enabled = true;
                            openLiveCameraFAIL_buttton.Enabled = true;
                            openMergeCameraPASS_buttton.Enabled = true;
                            openMergeCameraFAIL_buttton.Enabled = true;
                            redGreenPASS_button.Enabled = true;
                            redGreenFAIL_button.Enabled = true;
                            getCurrentMacAddress_button.Enabled = true;
                            beginResetTest_button.Enabled = true;
                            enterAgingMode_button.Enabled = true;
                            uiButton1.Enabled = true;
                            stopPinkNoise_button.Enabled = true;
                            recordingGif_label.Image = Image.FromFile("./img/recordingFinish.png");
                            uiButton1.Enabled = true;

                            if (nextDeviceConnect_t != null)
                            {
                                nextDeviceConnect_t.Interrupt();
                                nextDeviceConnect_t = null;
                            }
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

                            /**
                             * ����򿪺��Զ�ִ��ִ�в��Թ���
                             */
                            if (autoTest_t != null)
                            {
                                //MessageBox.Show("��ǰ���ڴ򿪲����С��������Ժ�");
                            }
                            else
                            {
                                autoTest_t = new Thread(autoTest_func);
                                autoTest_t.IsBackground = true;
                                autoTest_t.Start();
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

        }

        // ��ȡ��ǰ�豸PCBA��
        /**
         * ��ȡ��ǰ�豸PCBA�Ű�ť����¼�
         */
        private void getCurrentPCBA_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
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

        }

        // д��ָ��PCBA��
        /**
         * д��PCBA�ŵ���¼�
         */
        private void writeInPCBA_button_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("������ʱ�ر��Է�ֹ�������ÿ�����ֻ������д����ᡣ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //if (true)
            if (check_device_online())
            {
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

        }

        // ��ȡ��ǰ�豸�汾
        /**
         *  ��ȡ��ǰ�豸�Ĺ̼��汾�Ű�ť����¼�
         */
        private void gainCurrentVersion_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
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

        }

        // �����豸����
        /**
         * �����豸��ť����¼�
         */
        private void rebootDevice_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в����������豸����\n");
                try
                {
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        // �����豸����
                        stop_array_mic_audio_level_test_button_Click(null, null);
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"control\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"reboot\\\"}}\"";
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                        string result = "�����豸����δִ�гɹ�";
                        if (back_code == "0")
                        {
                            uiGroupBox1.Enabled = false;
                            uiGroupBox7.Enabled = false;
                            uiGroupBox9.Enabled = false;
                            result = "�ɹ�";
                            thread_reboot1 = new Thread(waitForReboot);
                            thread_reboot1.IsBackground = true;
                            thread_reboot1.Start();
                            //device_disconnect_button_Click(null, null);
                            /**
                            output_rich_textbox.AppendText("�ȴ�20���豸���������У��ڼ��޷��������ߡ���\n");
                            System.Threading.Thread.Sleep(20000);
                            */
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
        public static byte[] ConvertToBinary(string Path)
        {
            FileStream stream = new FileInfo(Path).OpenRead();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            return buffer;
        }

        // �궨����д�����
        private void calibrationDataWriteIn_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ִ�в������궨����д���������\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // �궨����д�����
                    string filePath = "";
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = false;
                    dialog.Title = "��ѡ��궨�����ļ�";
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        filePath = dialog.FileName;
                        output_rich_textbox.AppendText($"��ǰд��ı궨����Ϊ����{filePath}��\n");
                        //byte[] br2Binary = ConvertToBinary(filePath);
                        string fetchDeviceInfoCommand = $"curl -s -X POST --data-binary @{filePath} \"http://{ip_users}/writeLdcCalib_api\" -H \"Content-Type: text/plain\"";
                        Font font = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);
                        output_rich_textbox.ForeColor = Color.Green;
                        output_rich_textbox.SelectionFont = font;
                        output_rich_textbox.AppendText("��ȴ�һ��ʱ��д����ɣ��˼乤���޷�ʹ��\n");
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        output_rich_textbox.AppendText($"��{fetchDeviceInfoCommand}���궨����д����������" + output_string + "\n");

                        string result = "�궨����д�����δִ�гɹ�";
                        if (output_string.Contains("OK"))
                        {
                            result = "�ɹ�";
                        }
                        else
                        {
                            result = "ʧ��";
                        }
                        output_rich_textbox.AppendText("�궨����д����������" + result + "\n");

                    }
                    else
                    {
                        output_rich_textbox.AppendText("δѡ��궨���ݣ�ѡ�����ܽ��б궨����д�룡\n");
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
                output_rich_textbox.AppendText($"�궨����д�����ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        private void openByCMD_CameraProcess()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                // ������ģ����Թ���
                Process process_cmd = new Process();
                string output_string = null;
                try
                {
                    process_cmd.StartInfo.FileName = "SXW0301_Production_line.exe";
                    process_cmd.StartInfo.RedirectStandardInput = true;
                    //process_cmd.StartInfo.RedirectStandardOutput = true;
                    process_cmd.StartInfo.CreateNoWindow = false;
                    process_cmd.StartInfo.UseShellExecute = false;
                    process_cmd.Start();
                    process_cmd.WaitForExit();
                    //process_cmd.StandardInput.AutoFlush = true;
                }
                catch (Exception ex)
                {
                    output_string = ex.ToString();
                }
                finally
                {

                    process_cmd.Close();
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

        // ������ģ����Թ���
        Thread thread = null;
        private void open3CameraTest_button_Click(object sender, EventArgs e)
        {
            //if (true)
            try
            {
                if (thread == null)
                {
                    thread = new Thread(openByCMD_CameraProcess);
                    output_rich_textbox.AppendText("��ִ�в�����������ģ����Թ��ߡ���\n");
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    if (thread.IsAlive)
                    {
                        output_rich_textbox.AppendText("�Ѵ�����ģ����Թ��ߣ������ظ���Ŷ\n");
                    }
                    else
                    {
                        thread = new Thread(openByCMD_CameraProcess);
                        output_rich_textbox.AppendText("��ִ�в�����������ģ����Թ��ߡ���\n");
                        thread.IsBackground = true;
                        thread.Start();
                    }

                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"������ģ����Թ���ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        SXW0301_Production_line.Fom1 fom1;
        Thread form1_t;
        private void process_calibration_func()
        {
            MethodInvoker MethInvo = new MethodInvoker(enterCalibration);
            BeginInvoke(MethInvo);
        }
        private void enterCalibration()
        {
            if (fom1 == null)
            {
                fom1 = new SXW0301_Production_line.Fom1();
                fom1.Show();
            }
            else if (fom1.IsDisposed)
            {
                fom1 = new SXW0301_Production_line.Fom1();
                fom1.Activate();
                fom1.Show();
            }
            else
            {
                output_rich_textbox.AppendText("�Ѵ�����궨���ߣ������ظ���Ŷ\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в�����������궨���߲�������\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    if (form1_t != null)
                    {
                        form1_t.Interrupt();
                        form1_t = null;
                    }
                    form1_t = new Thread(process_calibration_func);
                    form1_t.IsBackground = false;
                    form1_t.Start();
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
        }

        SXW0301_Production_line.Form3 form3;
        Thread form3_t;
        private void process4()
        {
            MethodInvoker MethInvo = new MethodInvoker(enterForm3);
            BeginInvoke(MethInvo);
        }
        private void enterForm3()
        {
            // ��ƴ��ͼ����ǰ�� Merge���� curl �޸�������61440
            output_rich_textbox.AppendText("�������޸���Ƶ�����ʣ����ԵȻ��Զ��򿪽��桭����\n");
            int rate = 35000;
            string updateBitRateCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"Camera0Chn0\\\",\\\"value\\\": {{\\\"BitRate\\\": {rate}}}}}\"";
            output_string = executeCMDCommand(updateBitRateCommand);
            MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
            string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
            if (backCode == "0")
            {
                string getBitRateCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"Camera0Chn0\\\"}}\"";
                output_string = executeCMDCommand(getBitRateCommand);
                if (output_string.Contains(rate.ToString()))
                {
                    output_rich_textbox.AppendText($"��ǰƴ������Ƶ����Ϊ����{rate}��\n");
                    if (form3 == null)
                    {
                        form3 = new SXW0301_Production_line.Form3();
                        form3.Show();
                    }
                    else if (form3.IsDisposed)
                    {
                        form3 = new SXW0301_Production_line.Form3();
                        form3.Activate();
                        form3.Show();
                    }
                    else
                    {
                        output_rich_textbox.AppendText("�Ѵ�����ƴ��ͼ��⹤�ߣ������ظ���Ŷ\n");
                    }
                }
                else
                {
                    output_rich_textbox.AppendText($"ƴ�������ʡ�{rate}������ʧ��,����ֵ\n{output_string}\n�����飡\n");
                }
            }
            else
            {
                output_rich_textbox.AppendText($"ƴ�������ʡ�{rate}������ʧ��,����ֵ��{backCode}�������飡");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в�����������ƴ��ͼ��⹤�߲�������\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    if (form3_t != null)
                    {
                        form3_t.Interrupt();
                        form3_t = null;
                    }
                    form3_t = new Thread(process4);
                    form3_t.IsBackground = false;
                    form3_t.Start();
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

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void uiGroupBox3_Click(object sender, EventArgs e)
        {

        }

        // ���ŷۺ�����
        private void playPinkNoise()
        {
            audioPlayControl.PlayMusic();
        }


        // ֹͣ���ŷۺ�����
        private void stopPinkNoise()
        {
            audioPlayControl.StopMusic();
        }

        Thread audioIn1_t;

        private void audioIn1_func()
        {
            if (String.IsNullOrEmpty(audioInTestStandard_textbox.Text) || !new Regex("^[0-9]+$").IsMatch(audioInTestStandard_textbox.Text))
            {
                MessageBox.Show("�궨����ֵ����Ϊ�գ�\n���������˷����ֵ����ݣ����������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                output_rich_textbox.AppendText("�궨����ֵ����Ϊ�գ�\n���������˷����ֵ����ݣ����������룡\n");
                audioInTestStandard_textbox.Text = "";
                if (audioIn1_t != null)
                {
                    audioIn1_t.Interrupt();
                    audioIn1_t = null;
                }
            }
            //if (true)
            else if (check_device_online())
            {
                audioIn1_test_button.Enabled = false;
                // ÿ�λ�ȡMIC��Ƶ����ֵ��Ҫ��¼��1s���ȡ
                recordTime_textbox.Text = "1";
                beginAudioRecord_button_Click(null, null);
                audioRecord_t.Join();
                output_rich_textbox.AppendText("��ִ�в�����Audio IN 1 ���ԡ���\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    try
                    {
                        duration = recordTime_textbox.Text;
                        // ��ȡ��·MIC��Ƶ����ֵ
                        output_rich_textbox.AppendText(session);
                        string gainDeviceMICVolumeCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getAudioVolume\\\"}}\"";
                        output_string = executeCMDCommand(gainDeviceMICVolumeCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                        if (Int32.Parse(backCode) == 0)
                        {
                            MatchCollection results_2 = Regex.Matches(output_string, "\\\"rmsdb\\\" : \\[\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*)\\n   ]");
                            string[] temp = results_2[0].ToString().Replace("\"rmsdb\" : [", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Replace(" ", "").Split(",");
                            string volume1 = temp[0].ToString();
                            string volume2 = temp[1].ToString();
                            string volume3 = temp[2].ToString();
                            string volume4 = temp[3].ToString();
                            string volume5 = temp[4].ToString();
                            string volume6 = temp[5].ToString();
                            string volume7 = temp[6].ToString();
                            string volume8 = temp[7].ToString();
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��PASS����ȡ��·MIC��Ƶ����ֵ��backCode:[{backCode}]\n");
                            volume1_value_label.Text = volume1;
                            volume2_value_label.Text = volume2;
                            volume4_value_label.Text = volume4;
                            volume5_value_label.Text = volume5;
                            volume6_value_label.Text = volume6;
                            volume8_value_label.Text = volume8;
                            output_rich_textbox.AppendText($"rmsdb1��{volume1}\nrmsdb2��{volume2}\nrmsdb3��{volume3}\nrmsdb4��{volume4}\nrmsdb5��{volume5}\nrmsdb6��{volume6}\nrmsdb7��{volume7}\nrmsdb8��{volume8}\n");

                            float volume_1_f = float.Parse(volume1);
                            float volume_2_f = float.Parse(volume2);
                            float volume_3_f = float.Parse(volume3);
                            float volume_4_f = float.Parse(volume4);
                            float volume_5_f = float.Parse(volume5);
                            float volume_6_f = float.Parse(volume6);
                            float volume_7_f = float.Parse(volume7);
                            float volume_8_f = float.Parse(volume8);

                            audioIn1_label.Text = volume3;

                            float audioInStandard = float.Parse(audioInTestStandard_textbox.Text);

                            if (Math.Abs(Math.Abs(audioInStandard) - Math.Abs(volume_3_f)) <= 3)
                            {
                                // ���λ
                                refreshTestResult_button_Click(null, null);
                                testResults["���Խ��"].AudioInResult = "PASS";
                                writeTestResult();
                            }
                            else
                            {
                                // ���λ
                                refreshTestResult_button_Click(null, null);
                                testResults["���Խ��"].AudioInResult = "FAIL";
                                writeTestResult();
                            }
                        }
                        else if (Int32.Parse(backCode) == -1)
                        {
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL���޷���ȡ��·MIC��Ƶ����ֵ��backCode:[{backCode}]\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Audio IN 1 ����ʧ��ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n{ex.ToString()}");
                        output_rich_textbox.AppendText($"Audio IN 1 ����ʧ��ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n");
                    }
                    finally
                    {
                        if (audioIn1_t != null)
                        {
                            audioIn1_t.Interrupt();
                            audioIn1_t = null;
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
            recordingGif_label.Image = Image.FromFile("./img/recordingFinish.png");
        }

        // Audio IN 1 ����
        private void audioIn1_test_button_Click_1(object sender, EventArgs e)
        {
            if (audioIn1_t != null)
            {
                MessageBox.Show("ǰһ��Audio IN 1���ڽ����С��������Ժ��ٽ��е����");
            }
            else
            {
                playPinkNoise();
                recordingGif_label.Visible = true;
                recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
                audioIn1_t = new Thread(audioIn1_func);
                audioIn1_t.IsBackground = true;
                audioIn1_t.Start();
            }

        }

        Thread audioIn2_t;
        private void audioIn2_func()
        {
            if (String.IsNullOrEmpty(audioInTestStandard_textbox.Text) || !new Regex("^[0-9]+$").IsMatch(audioInTestStandard_textbox.Text))
            {
                MessageBox.Show("�궨����ֵ����Ϊ�գ�\n���������˷����ֵ����ݣ����������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                output_rich_textbox.AppendText("�궨����ֵ����Ϊ�գ�\n���������˷����ֵ����ݣ����������룡\n");
                audioInTestStandard_textbox.Text = "";
                if (audioIn2_t != null)
                {
                    audioIn2_t.Interrupt();
                    audioIn2_t = null;
                }
            }
            //if (true)
            else if (check_device_online())
            {
                audioIn2_test_button.Enabled = false;
                // ÿ�λ�ȡMIC��Ƶ����ֵ��Ҫ��¼��1s���ȡ
                recordTime_textbox.Text = "1";
                beginAudioRecord_button_Click(null, null);
                audioRecord_t.Join();
                output_rich_textbox.AppendText("��ִ�в�����Audio IN 2 ���ԡ���\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    try
                    {
                        duration = recordTime_textbox.Text;
                        recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
                        // ��ȡ��·MIC��Ƶ����ֵ
                        output_rich_textbox.AppendText(session);
                        string gainDeviceMICVolumeCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getAudioVolume\\\"}}\"";
                        output_string = executeCMDCommand(gainDeviceMICVolumeCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                        if (Int32.Parse(backCode) == 0)
                        {
                            MatchCollection results_2 = Regex.Matches(output_string, "\\\"rmsdb\\\" : \\[\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*)\\n   ]");
                            string[] temp = results_2[0].ToString().Replace("\"rmsdb\" : [", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Replace(" ", "").Split(",");
                            string volume1 = temp[0].ToString();
                            string volume2 = temp[1].ToString();
                            string volume3 = temp[2].ToString();
                            string volume4 = temp[3].ToString();
                            string volume5 = temp[4].ToString();
                            string volume6 = temp[5].ToString();
                            string volume7 = temp[6].ToString();
                            string volume8 = temp[7].ToString();
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��PASS����ȡ��·MIC��Ƶ����ֵ��backCode:[{backCode}]\n");
                            volume1_value_label.Text = volume1;
                            volume2_value_label.Text = volume2;
                            volume4_value_label.Text = volume4;
                            volume5_value_label.Text = volume5;
                            volume6_value_label.Text = volume6;
                            volume8_value_label.Text = volume8;
                            output_rich_textbox.AppendText($"rmsdb1��{volume1}\nrmsdb2��{volume2}\nrmsdb3��{volume3}\nrmsdb4��{volume4}\nrmsdb5��{volume5}\nrmsdb6��{volume6}\nrmsdb7��{volume7}\nrmsdb8��{volume8}\n");

                            float volume_1_f = float.Parse(volume1);
                            float volume_2_f = float.Parse(volume2);
                            float volume_3_f = float.Parse(volume3);
                            float volume_4_f = float.Parse(volume4);
                            float volume_5_f = float.Parse(volume5);
                            float volume_6_f = float.Parse(volume6);
                            float volume_7_f = float.Parse(volume7);
                            float volume_8_f = float.Parse(volume8);

                            audioIn2_label.Text = volume7;

                            float audioInStandard = float.Parse(audioInTestStandard_textbox.Text);

                            if (Math.Abs(Math.Abs(audioInStandard) - Math.Abs(volume_7_f)) <= 3)
                            {
                                // ���λ
                                refreshTestResult_button_Click(null, null);
                                testResults["���Խ��"].AudioIn2Result = "PASS";
                                writeTestResult();
                            }
                            else
                            {
                                // ���λ
                                refreshTestResult_button_Click(null, null);
                                testResults["���Խ��"].AudioIn2Result = "FAIL";
                                writeTestResult();
                            }
                        }
                        else if (Int32.Parse(backCode) == -1)
                        {
                            output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL���޷���ȡ��·MIC��Ƶ����ֵ��backCode:[{backCode}]\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Audio IN 2 ����ʧ��ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n");
                        output_rich_textbox.AppendText($"Audio IN 2 ����ʧ��ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n�ɳ������������ָ�������\n");
                    }
                    finally
                    {
                        if (audioIn2_t != null)
                        {
                            audioIn2_t.Interrupt();
                            audioIn2_t = null;
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
            recordingGif_label.Image = Image.FromFile("./img/recordingFinish.png");
        }

        // Audio IN 2 ����
        private void audioIn2_test_button_Click_1(object sender, EventArgs e)
        {

            if (audioIn2_t != null)
            {
                MessageBox.Show("ǰһ��Audio IN 2���ڽ����С��������Ժ��ٽ��е����");
            }
            else
            {
                playPinkNoise();
                recordingGif_label.Visible = true;
                recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
                audioIn2_t = new Thread(audioIn2_func);
                audioIn2_t.IsBackground = true;
                audioIn2_t.Start();
            }
        }

        string fileName_testResult = "TestResult.txt";
        // ���ò��Խ��
        private void resetTestResult_button_Click(object sender, EventArgs e)
        {
            testResults.Clear();
            if (System.IO.File.Exists(fileName_testResult))
            {
                executeCMDCommand($"del {fileName_testResult}");
                network_test_label.Text = "��δ����";
                network2_test_label.Text = "��δ����";
                firmwareVerified_test_label.Text = "��δ����";
                redgreenLED_test_label.Text = "��δ����";
                resetButton_test_label.Text = "��δ����";
                threeCamera_test_label.Text = "��δ����";
                threeCamera2_test_label.Text = "��δ����";
                audioIn_test_label.Text = "��δ����";
                audioIn2_test_label.Text = "��δ����";
                arrayMic_test_label.Text = "��δ����";
                macAddress_test_label.Text = "��δ����";
                bareboardInfoFlash_test_label.Text = "��δ����";
                completeMachineInfoFlash_test_label.Text = "��δ����";

            }
            //MessageBox.Show("���Խ��������ɣ�", "��ʾ",  MessageBoxButtons.OK, MessageBoxIcon.Information);
            output_rich_textbox.AppendText("���Խ��������ɣ�\n");
        }

        // д����Խ��
        private void writeTestResult()
        {
            FileStream fs2 = new FileStream(fileName_testResult, FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs2, testResults);
            fs2.Close();
            refreshTestResult_button_Click(null, null);
        }

        // ˢ�²��Խ��
        private void refreshTestResult_button_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fs2 = new FileStream(fileName_testResult, FileMode.OpenOrCreate);
                if (fs2.Length == 0)
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    TestResult testResult = new TestResult();
                    testResult.NetworkTestResult = "��δ����";
                    testResult.Network2TestResult = "��δ����";
                    testResult.FirmwareVerifiedResult = "��δ����";
                    testResult.RedGreenLEDResult = "��δ����";
                    testResult.ResetButtonResult = "��δ����";
                    testResult.ThreeCameraResult = "��δ����";
                    testResult.ThreeCamera2Result = "��δ����";
                    testResult.AudioInResult = "��δ����";
                    testResult.AudioIn2Result = "��δ����";
                    testResult.ArrayMicResult = "��δ����";
                    testResult.MacAddressResult = "��δ����";
                    testResult.BareBoradMachineFlashResult = "��δ����";
                    testResult.CompleteMachineFlashResult = "��δ����";

                    testResults.Add("���Խ��", testResult);
                    binaryFormatter.Serialize(fs2, testResults);
                    fs2.Close();

                }
                else if (fs2.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    TestResult testResult = new TestResult();
                    testResults = bf.Deserialize(fs2) as Dictionary<string, TestResult>;
                    TestResult testresult_item = testResults["���Խ��"];
                    network_test_label.Text = testresult_item.NetworkTestResult;
                    network2_test_label.Text = testresult_item.Network2TestResult;
                    firmwareVerified_test_label.Text = testresult_item.FirmwareVerifiedResult;
                    redgreenLED_test_label.Text = testresult_item.RedGreenLEDResult;
                    resetButton_test_label.Text = testresult_item.ResetButtonResult;
                    threeCamera_test_label.Text = testresult_item.ThreeCameraResult;
                    threeCamera2_test_label.Text = testresult_item.ThreeCamera2Result;
                    audioIn_test_label.Text = testresult_item.AudioInResult;
                    audioIn2_test_label.Text = testresult_item.AudioIn2Result;
                    arrayMic_test_label.Text = testresult_item.ArrayMicResult;
                    macAddress_test_label.Text = testresult_item.MacAddressResult;
                    bareboardInfoFlash_test_label.Text = testresult_item.BareBoradMachineFlashResult;
                    completeMachineInfoFlash_test_label.Text = testresult_item.CompleteMachineFlashResult;
                }
                fs2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���쳣����������ǰ���Խ�����:\n�����ֶ�ɾ����Ŀ¼���ļ�����ͬʱ���ж��¼�Ʋ���\n��������������쳣�����´򿪹��ߣ�", "����ܰ��ʾ��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                output_rich_textbox.AppendText("���쳣����������ǰ���Խ�����:\n�����ֶ�ɾ����Ŀ¼���ļ�����ͬʱ���ж��¼�Ʋ���\n��������������쳣�����´򿪹��ߣ�\n");
                //Application.Exit();
            }
        }

        // ����ָʾ�Ʋ���PASS
        private void redGreenPASS_button_Click(object sender, EventArgs e)
        {
            // ���λ
            refreshTestResult_button_Click(null, null);
            testResults["���Խ��"].RedGreenLEDResult = "PASS";
            writeTestResult();
        }

        // ����ָʾ�Ʋ���FAIL
        private void redGreenFAIL_button_Click(object sender, EventArgs e)
        {
            // ���λ  
            refreshTestResult_button_Click(null, null);
            testResults["���Խ��"].RedGreenLEDResult = "FAIL";
            writeTestResult();
        }

        // ����SC12-E���Թ�װ���ڲ���
        private void poe1NetworkTest_button_Click(object sender, EventArgs e)
        {
            string sc12_E_ip = "219.198.235.13";
            output_rich_textbox.AppendText("��ִ�в���������SC12-E���Թ�װ���ڲ��ԡ���\n");
            string temp_check_ping_ip_exists = executeCMDCommand($"ping {sc12_E_ip} -n 1");
            output_rich_textbox.Text = "";
            //login_button_Click(null, null);
            if (temp_check_ping_ip_exists.Contains("TTL"))
            {
                // ���λ
                refreshTestResult_button_Click(null, null);
                testResults["���Խ��"].Network2TestResult = "PASS";
                writeTestResult();
                output_rich_textbox.AppendText("����PASS������SC12-E���Թ�װ�ɹ���\n");
            }
            else
            {
                // ���λ
                refreshTestResult_button_Click(null, null);
                testResults["���Խ��"].Network2TestResult = "FAIL";
                writeTestResult();
                MessageBox.Show("����SC12-E���Թ�װ���ڲ���ʧ�ܣ������������ӣ�");
                output_rich_textbox.AppendText("����SC12-E���Թ�װ���ڲ���ʧ�ܣ������������ӣ�\n");
            }
        }

        SXW0301_Production_line.LiveWindow liveWindow;
        Thread liveWindow_t;
        private void enterLiveCameraTest()
        {
            if (liveWindow == null)
            {
                liveWindow = new SXW0301_Production_line.LiveWindow();
                liveWindow.Show();
            }
            else if (liveWindow.IsDisposed)
            {
                liveWindow = new SXW0301_Production_line.LiveWindow();
                liveWindow.Activate();
                liveWindow.Show();
            }
            else
            {
                output_rich_textbox.AppendText("�Ѵ�Live����ͷ�������ظ���Ŷ\n");
            }
        }

        private void process5()
        {
            MethodInvoker MethInvo = new MethodInvoker(enterLiveCameraTest);
            BeginInvoke(MethInvo);
        }

        // ��Live����ͷ
        private void openLiveCamera_buttton_Click(object sender, EventArgs e)
        {
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в�������Live����ͷ����\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    if (liveWindow_t != null)
                    {
                        liveWindow_t.Interrupt();
                        liveWindow_t = null;
                    }
                    liveWindow_t = new Thread(process5);
                    liveWindow_t.IsBackground = false;
                    liveWindow_t.Start();
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
        }

        // ��Live����ͷPASS
        private void openLiveCameraPASS_buttton_Click(object sender, EventArgs e)
        {
            // ���λ
            refreshTestResult_button_Click(null, null);
            testResults["���Խ��"].ThreeCameraResult = "PASS";
            writeTestResult();
        }

        // ��Live����ͷFAIL
        private void openLiveCameraFAIL_buttton_Click(object sender, EventArgs e)
        {
            // ���λ
            refreshTestResult_button_Click(null, null);
            testResults["���Խ��"].ThreeCameraResult = "FAIL";
            writeTestResult();
        }

        SXW0301_Production_line.MergeWindow mergeWindow;
        Thread mergeWindow_t;
        private void enterMergeWindowTest()
        {
            if (mergeWindow == null)
            {
                mergeWindow = new SXW0301_Production_line.MergeWindow();
                mergeWindow.Show();
            }
            else if (mergeWindow.IsDisposed)
            {
                mergeWindow = new SXW0301_Production_line.MergeWindow();
                mergeWindow.Activate();
                mergeWindow.Show();
            }
            else
            {
                output_rich_textbox.AppendText("�Ѵ�Merge����ͷ�������ظ���Ŷ\n");
            }
        }

        private void process6()
        {
            MethodInvoker MethInvo = new MethodInvoker(enterMergeWindowTest);
            BeginInvoke(MethInvo);
        }

        // ��Merge����ͷ
        private void openMergeCamera_buttton_Click(object sender, EventArgs e)
        {
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в�������Merge����ͷ����\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    if (mergeWindow_t != null)
                    {
                        mergeWindow_t.Interrupt();
                        mergeWindow_t = null;
                    }
                    mergeWindow_t = new Thread(process6);
                    mergeWindow_t.IsBackground = false;
                    mergeWindow_t.Start();
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
        }

        // ��Merge����ͷPASS
        private void openMergeCameraPASS_buttton_Click(object sender, EventArgs e)
        {
            // ���λ
            refreshTestResult_button_Click(null, null);
            testResults["���Խ��"].ThreeCamera2Result = "PASS";
            writeTestResult();
        }

        // ��Merge����ͷFAIL
        private void openMergeCameraFAIL_buttton_Click(object sender, EventArgs e)
        {
            // ���λ
            refreshTestResult_button_Click(null, null);
            testResults["���Խ��"].ThreeCamera2Result = "FAIL";
            writeTestResult();
        }




        Thread writeInMac_t;
        private void writeInMacFunc()
        {
            output_rich_textbox.AppendText(session);
            for (int i = 1; i < 15; i += 3)
            {
                writeInMAC = writeInMAC.Insert(i + 1, ":");
            }
            string writeDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"EthernetService\\\",\\\"value\\\": {{\\\"Eth0MAC\\\": \\\"{writeInMAC}\\\" , \\\"Eth0Name\\\": \\\"eth0\\\"}}}}\"";
            output_string = executeCMDCommand(writeDeviceInfoCommand);
            MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
            string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

            // ������ɺ��ٻ�ȡһ��MAC�������һ���ģ���PASS
            getCurrentMacAddress_button_Click(null, null);
            string currentMAC_label_text = currentMac_label.Text;
            if (currentMAC_label_text == writeInMAC && Int32.Parse(backCode) == 0)
            {
                output_rich_textbox.AppendText($"��ǰд����Ϊ��PASS����ǰMAC��{currentMAC_label_text}\n");
                macAddress_test_label.Text = "PASS";

                // ���λ
                refreshTestResult_button_Click(null, null);
                testResults["���Խ��"].MacAddressResult = "PASS";
                writeTestResult();

                // �ж�����ǣ�219.198.235.11����Ҫ�������豸ǰ����ping������ͨ·�ɣ�������ǣ�����Ҫping 219.198.235.11 -t -S 219.198.235.17\
                if (host == "219.198.235.11")
                {
                    string localPCHost = null;
                    foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                        {
                            string inetName = netItem.Name;
                            string inetAddress = ipIntProp.Address.ToString();
                            string inetType = ipIntProp.Address.AddressFamily.ToString();
                            //output_rich_textbox.AppendText($"   �ӿ�����{inetName}��IP��{inetAddress}��IP���ͣ�{inetType}\n");
                            if (inetName == "��̫��" && inetType == "InterNetwork")
                            {
                                localPCHost = inetAddress;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(localPCHost))
                    {
                        int times = 0;
                        while (true)
                        {
                            times += 1;
                            string back_temp = executeCMDCommand($"ping {host} -t -S {localPCHost} -n 1");
                            Thread.Sleep(1000);
                            output_rich_textbox.AppendText($"����ʾ����{times}�롿�ؽ�·�������������Ժ󡭡�\n");
                            if (back_temp.Contains("TTL"))
                            {
                                break;
                            }
                            if (times >= 30)
                            {
                                output_rich_textbox.AppendText("30����·������û�и���û�гɹ���������������");
                                break;
                            }
                        }
                    }
                }

                device_disconnect_button_Click(null, null);
                device_connect_button_Click(null, null);
                writeInMac_button.Enabled = true;
            }
            else
            {
                output_rich_textbox.AppendText($"��ǰд����Ϊ��FAIL����ǰMAC��{currentMAC_label_text}\n");
                macAddress_test_label.Text = "FAIL";

                // ���λ
                refreshTestResult_button_Click(null, null);
                testResults["���Խ��"].MacAddressResult = "FAIL";
                writeTestResult();

                writeInMac_button.Enabled = true;
            }
            if (writeInMac_t != null)
            {
                writeInMac_t.Interrupt();
                writeInMac_t = null;
            }
            recordingGif_label.Image = Image.FromFile("./img/recordingFinish.png");
        }

        private string writeInMAC;

        // д��MAC��ַ���豸
        private void writeInMac_button_Click(object sender, EventArgs e)
        {
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в�����д��MAC��ַ���豸����\n");
                try
                {
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        // д��MAC��ַ���豸 -- �ȴ�����
                        writeInMAC = MAC;
                        if (string.IsNullOrEmpty(writeInMAC) || writeInMAC.Length != 12)
                        {
                            MessageBox.Show("��д����ȷ��MAC��ַ�ٽ���ˢ�룬ֻ�����ֻ�����ĸ\n", "��ܰ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            output_rich_textbox.AppendText("��д����ȷ��MAC��ַ�ٽ���ˢ�룬ֻ�����ֻ�����ĸ\n");
                            currentMac_label.Text = "д��ʧ��";
                            macAddress_test_label.Text = "д��ʧ��";
                        }
                        else
                        {
                            writeInMac_button.Enabled = false;
                            if (writeInMac_t != null)
                            {
                                MessageBox.Show("��ǰMAC����д�룬���Ժ�");
                            }
                            else
                            {
                                writeInMac_t = new Thread(writeInMacFunc);
                                writeInMac_t.IsBackground = true;
                                writeInMac_t.Start();
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
                    writeInMac_button.Enabled = true;
                    if (writeInMac_t != null)
                    {
                        writeInMac_t.Interrupt();
                        writeInMac_t = null;
                    }
                    output_rich_textbox.AppendText($"д��MAC��ַ���豸ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
                    currentMac_label.Text = "FAIL";
                }
                finally
                {

                }
            }
        }

        string duration;
        private void recordAudioThread()
        {
            try
            {
                // ��ʼ��Ƶ¼��
                output_rich_textbox.AppendText(session);
                string beginDeviceMICVolumeTestCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"startRecordAudio\\\",\\\"format\\\": 0,\\\"soundmode\\\": 8,\\\"samplerate\\\": 16000,\\\"periodsize\\\": 1000,\\\"duration\\\": {int.Parse(duration)}}}\"";
                output_string = executeCMDCommand(beginDeviceMICVolumeTestCommand);
                MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                if (Int32.Parse(backCode) == 0)
                {
                    output_rich_textbox.AppendText($"ִ�н��Ϊ��PASS���ѿ�ʼ��Ƶ¼�ƣ�backCode:[{backCode}]\n");
                }
                else if (Int32.Parse(backCode) == -1)
                {
                    output_rich_textbox.AppendText($"ִ�н��Ϊ��FAIL��δ��ʼ��Ƶ¼�ƣ�backCode:[{backCode}]\n");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"��ʼ��Ƶ¼��ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n��ɳ������������ָ�������\n");
                output_rich_textbox.AppendText($"��ʼ��Ƶ¼��ʧ�ܣ������Ƿ��Կ�������Ƶ����δ��ȷ�رգ�\n��ɳ������������ָ�������\n");
            }
            finally
            {
                Font font = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);
                output_rich_textbox.ForeColor = Color.Green;
                output_rich_textbox.SelectionFont = font;
                output_rich_textbox.AppendText("��Ƶ¼����ɣ�\n");
                beginAudioRecord_button.Enabled = true;
                // ¼�������ʾ
                recordingGif_label.Image = Image.FromFile("./img/recordingFinish.png");
                gain_array_mic_audio_level_button.Enabled = true;
                audioIn1_test_button.Enabled = true;
                audioIn2_test_button.Enabled = true;
                if (audioRecord_t != null)
                {
                    audioRecord_t.Interrupt();
                    audioRecord_t = null;
                }
            }
        }

        Thread audioRecord_t;

        // ��ʼ��Ƶ¼��
        private void beginAudioRecord_button_Click(object sender, EventArgs e)
        {
            //if (true)
            duration = recordTime_textbox.Text;
            recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
            if (String.IsNullOrEmpty(duration) || !new Regex("^[0-9]+$").IsMatch(duration) || duration.Equals("0"))
            {
                // ���߳�ȥ����������������ӣ�����ʱ���¼�ƣ���ȡ��¼�Ƶ���Ƶ
                // ��������¼���ټ��㣬����Ե��һ�Σ��Ϳ�ʼ¼�ƣ��ٻ�ȡ����,�޸�ÿһ�λ�ȡ������ģ���¼���ٶ�ȡ��¼����һ��������
                MessageBox.Show("��Ƶ¼��ʱ�䲻��Ϊ�գ�\n���������˷����ֵ����ݣ����������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                output_rich_textbox.AppendText("��Ƶ¼��ʱ�䲻��Ϊ�գ�\n���������˷����ֵ����ݣ����������룡\n");
                recordTime_textbox.Text = "";
            }
            else if (check_device_online())
            {
                beginAudioRecord_button.Enabled = false;
                output_rich_textbox.AppendText("��ִ�в�������ʼ��Ƶ¼�Ʋ��ԡ���\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    if (audioRecord_t != null)
                    {
                        MessageBox.Show("��ǰ��δ¼����ɣ����Եȣ�");
                        output_rich_textbox.AppendText("��ǰ��δ¼����ɣ����Եȣ�");
                    }
                    else
                    {
                        audioRecord_t = new Thread(recordAudioThread);
                        audioRecord_t.IsBackground = true;
                        audioRecord_t.Start();

                    }
                    Font font = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);
                    output_rich_textbox.ForeColor = Color.Red;
                    output_rich_textbox.SelectionFont = font;
                    output_rich_textbox.AppendText($"���Եȣ�����¼����Ƶ��,¼��ʱ�䡾{duration}���롭����\n");
                    recordingGif_label.Visible = true;

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
        }

        Thread extractAudioFile_t;

        // ����¼���ļ�
        private void extractRecordFile_button_Click(object sender, EventArgs e)
        {
            string savedFilePath;
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в���������¼���ļ�����\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    extractRecordFile_button.Enabled = false;
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "��ѡ�񱣴��ļ�·��";
                    //saveFileDialog.Filter = "��Ƶ�ļ�(*.mp3;*.mid;*.wav;)|*.mp3;*.mid;*.wav;";
                    saveFileDialog.Filter = "��Ƶ�ļ�(*.wav;)|*.wav;";

                    saveFileDialog.FileName = "audioRecord.wav";
                    saveFileDialog.OverwritePrompt = true;
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        recordingGif_label.Visible = true;
                        recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
                        LocalSavePath = saveFileDialog.FileName.ToString();
                        if (extractAudioFile_t != null)
                        {
                            MessageBox.Show("��ǰ��δ������ɣ����Եȣ�");
                            output_rich_textbox.AppendText("��ǰ��δ������ɣ����Եȣ�\n");
                        }
                        else
                        {
                            extractAudioFile_t = new Thread(DownloadFile);
                            extractAudioFile_t.IsBackground = true;
                            extractAudioFile_t.Start();
                        }
                    }
                    else
                    {
                        extractRecordFile_button.Enabled = true;
                        return;
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
        }

        FtpWebRequest reqFTP;
        Stream ftpStream;
        FileStream outputStream;
        FtpWebResponse response;
        string LocalSavePath;
        public void DownloadFile()
        {
            try
            {
                string ftpPath = "ftp://" + device_ip_textbox.Text + "/audioDumpFile.wav";
                outputStream = new FileStream(LocalSavePath, FileMode.Create);

                // ����uri����FtpWebRequest����   
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));

                // ָ��ִ��ʲô����  
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                // ָ�����ݴ�������  
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;

                // ftp�û���������  
                //reqFTP.Credentials = new NetworkCredential();

                response = (FtpWebResponse)reqFTP.GetResponse();

                // �����ص��ļ�д����
                ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;

                // �����С����Ϊ2kb  
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                // ÿ�ζ��ļ�����2kb  
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    // �����ݴ��ļ���д��   
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                //�ر���������ftp����
                ftpStream.Close();
                outputStream.Close();
                response.Close();

                MessageBox.Show($"¼���ļ�������ɣ�\n�ļ���ַ��{LocalSavePath}", "��ܰ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                output_rich_textbox.AppendText($"¼���ļ�������ɣ�\n�ļ���ַ��{LocalSavePath}\n");
                recordingGif_label.Image = Image.FromFile("./img/recordingFinish.png");
                extractAudioFile_t = null;
                extractRecordFile_button.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("δ�ҵ�ָ���ļ�������¼����Ƶ��", "��ܰ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                output_rich_textbox.AppendText("δ�ҵ�ָ���ļ�������¼����Ƶ��\n");
                if (ftpStream != null)
                {
                    ftpStream.Close();
                }
                if (outputStream != null)
                {
                    outputStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (extractAudioFile_t != null)
                {
                    extractAudioFile_t = null;
                }
                extractRecordFile_button.Enabled = true;
            }
        }

        // ��ȡ��ǰMAC��ַ
        private void getCurrentMacAddress_button_Click(object sender, EventArgs e)
        {
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в�������ȡ��ǰMAC��ַ����\n");
                try
                {
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        // ��ȡ��ǰMAC��ַ
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"EthernetService\\\"}}\"";
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"Eth0MAC\" : (.*)");
                        string currentMac = results_1[0].ToString().ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "").Replace("Eth0MAC:", "");
                        output_rich_textbox.AppendText("��ȡ��ǰMAC��ַ��" + currentMac + "\n");
                        currentMac_label.Text = currentMac;
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
                    output_rich_textbox.AppendText($"��ȡ��ǰMAC��ַʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
        }

        Thread resetDevice_t;

        private void resetDevice_func()
        {
            // ����Reset����
            string deleteResultFile = $"curl \"ftp://{host}\" -X \"DELE restore_result\"";
            executeCMDCommand("del restore_result");
            output_string = executeCMDCommand(deleteResultFile);
            string fetchDeviceInfoCommand = $"curl \"ftp://{host}/restore_result\" -o restore_result";
            int time_limit = 0;
            while (true)
            {
                time_limit += 1;
                Thread.Sleep(1000);
                output_string = executeCMDCommand(fetchDeviceInfoCommand);
                output_rich_textbox.AppendText($"���ڻ�ȡ����״̬�У���ǰ��{time_limit}���룡\n");
                if (executeCMDCommand("type restore_result").Contains("success"))
                {
                    resetButton_test_label.Text = "PASS";
                    resetTestClickResult_label.Text = "PASS";
                    // ���λ
                    refreshTestResult_button_Click(null, null);
                    testResults["���Խ��"].ResetButtonResult = "PASS";
                    writeTestResult();
                    output_rich_textbox.AppendText("��Reset���ԡ����Խ����PASS\n");
                    device_disconnect_button_Click(null, null);
                    device_connect_button_Click(null, null);
                    break;
                }
                if (time_limit >= 30)
                {
                    resetButton_test_label.Text = "FAIL";
                    resetTestClickResult_label.Text = "FAIL";
                    // ���λ
                    refreshTestResult_button_Click(null, null);
                    testResults["���Խ��"].ResetButtonResult = "FAIL";
                    writeTestResult();
                    output_rich_textbox.AppendText("��Reset���ԡ����Խ����FAIL\n");
                    MessageBox.Show("Reset����ʧ�ܻ���30s��δ��⵽Reset�����£���������ʼ���ԡ������ز⣡", "�����ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }
            beginResetTest_button.Enabled = true;
            if (resetDevice_t != null)
            {
                resetDevice_t.Interrupt();
                resetDevice_t = null;
            }
        }

        // ��ʼ����Reset����
        private void beginResetTest_button_Click(object sender, EventArgs e)
        {
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в�������ʼ����Reset����������30s��֮�ڰ��£������Զ��ж�Ϊʧ�ܡ���\n");
                resetTestClickResult_label.Text = "";
                try
                {
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        beginResetTest_button.Enabled = false;
                        if (resetDevice_t != null)
                        {
                            MessageBox.Show("��ǰ����Reset���������Ժ�");
                        }
                        else
                        {
                            resetDevice_t = new Thread(resetDevice_func);
                            resetDevice_t.IsBackground = true;
                            resetDevice_t.Start();
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
                    if (resetDevice_t != null)
                    {
                        resetDevice_t.Interrupt();
                        resetDevice_t = null;
                    }
                    beginResetTest_button.Enabled = true;
                    output_rich_textbox.AppendText($"����Reset����ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
        }

        SXW0301_Production_line.AgingTestPanel agingTestPanel;
        Thread agingThread;
        private void enterAgingMode()
        {
            int rate = 4096;
            string updateBitRateCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"Camera0Chn0\\\",\\\"value\\\": {{\\\"BitRate\\\": {rate}}}}}\"";
            output_string = executeCMDCommand(updateBitRateCommand);
            if (agingTestPanel == null)
            {
                agingTestPanel = new SXW0301_Production_line.AgingTestPanel();
                agingTestPanel.Show();
            }
            else if (agingTestPanel.IsDisposed)
            {
                agingTestPanel = new SXW0301_Production_line.AgingTestPanel();
                agingTestPanel.Activate();
                agingTestPanel.Show();

            }
            else
            {
                output_rich_textbox.AppendText("�ѽ����ϻ�ģʽ��壬�����ظ���Ŷ\n");
            }

        }
        private void process3()
        {
            MethodInvoker MethInvo = new MethodInvoker(enterAgingMode);
            BeginInvoke(MethInvo);
        }

        // �����ϻ�ģʽ���
        private void enterAgingMode_button_Click(object sender, EventArgs e)
        {
            if (agingThread != null)
            {
                agingThread.Interrupt();
                agingThread = null;
            }
            if (check_device_online())
            {
                output_rich_textbox.AppendText("��ִ�в����������ϻ�ģʽ��塭��\n");
                if (clientSocket != null && clientSocket.Connected)
                {
                    agingThread = new Thread(process3);
                    agingThread.IsBackground = false;
                    agingThread.Start();
                    //agingThread.Join();
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "�ѶϿ�";
                    output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
                }
                //enterAgingMode();
            }
        }

        Thread nextDeviceConnect_t;
        private void nextDeviceConnect_func()
        {
            try
            {
                output_rich_textbox.AppendText("��ִ�в�������ǰ�豸�ѶϿ����������һ̨�豸�󼴿��Զ����ӿ�ʼ����\n");
                device_disconnect_button_Click(null, null);
                resetTestResult_button_Click(null, null);
                currentMac_label.Text = "";
                currentVersion_label.Text = "";
                upgrade_progressbar.Value = 0;
                checked_firmware_textbox.Text = "";
                audioIn1_label.Text = "";
                audioIn2_label.Text = "";
                volume5_value_label.Text = "";
                volume1_value_label.Text = "";
                volume6_value_label.Text = "";
                volume2_value_label.Text = "";
                volume8_value_label.Text = "";
                volume4_value_label.Text = "";
                currentPCBA_textbox.Text = "";
                writeINPCBA_textbox.Text = "";
                currentSN_textbox.Text = "";
                writeInSN_textbox.Text = "";
                output_rich_textbox.Text = "";
                seewoProductPN_label.Text = "";
                seewoProductSN_label.Text = "";
                seewoWorkOrder_label.Text = "";
                seewoCustomerPN_label.Text = "";
                currentMac_label.Text = "";
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            
                
            

            device_connect_button_Click(null, null);
        }

        // ���������һ̨�豸���Ͽ���ǰ�豸ͬʱ������Խ��
        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (nextDeviceConnect_t != null)
            {
                MessageBox.Show("��ǰ����������һ̨�豸�����Ժ�");
            }
            else
            {
                completeMachine_textbox.Text = "";
                boardMachine_textbox.Text = "";
                nextDeviceConnect_t = new Thread(nextDeviceConnect_func);
                nextDeviceConnect_t.IsBackground = true;
                nextDeviceConnect_t.Start();
            }
            uiButton1.Enabled = false;
            recordingGif_label.Visible = true;
            recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
            
            
        }

        private void stopPinkNoise_button_Click(object sender, EventArgs e)
        {
            stopPinkNoise();
        }

        private void mothreBoardTest_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (bareBoardTest_checkBox.Checked)
            {
                bareBoardTest_checkBox.Checked = false;
            }
        }

        private void bareBoardTest_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (mothreBoardTest_checkBox.Checked)
            {
                mothreBoardTest_checkBox.Checked = false;
            }
        }

        // д��������Ϣfunc
        private void writeInCompleteMachineInfo_func()
        {
            output_rich_textbox.AppendText(session);
            string writeInCompleteMachineInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"SeewoProductPN\\\": \\\"{SeewoProductPN}\\\", \\\"SeewoProductSN\\\": \\\"{SeewoProductSN}\\\"}}}}\"";
            output_string = executeCMDCommand(writeInCompleteMachineInfoCommand);
            MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
            string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

            string[] infos = getCompleteAndBareBoardInfo_func();
            if (infos.Length > 0)
            {
                string result_temp_now = "";
                if ((infos[2] == SeewoProductPN) && (infos[3] == SeewoProductSN) && (Int32.Parse(backCode) == 0))
                {
                    output_rich_textbox.AppendText("��ǰд��������Ϣ���Ϊ��PASS\n");
                    result_temp_now = "PASS";
                    completeMachine_textbox.Text = "";
                }
                else
                {
                    output_rich_textbox.AppendText($"��ǰд��������Ϣ���Ϊ��FAIL\n");
                    result_temp_now = "FAIL";
                    completeMachine_textbox.Text = "��¼ʧ�ܣ���ȷ��������Ϣ�Ƿ�������";
                }
                // ���λ
                refreshTestResult_button_Click(null, null);
                testResults["���Խ��"].CompleteMachineFlashResult = result_temp_now;
                writeTestResult();
                
                //MessageBox.Show($"������������������Ϣ��¼��ɣ�\n��¼�������{result_temp_now}��", "��֪ͨ��", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // д��忨��Ϣfunc
        private void writeInBareboardMachineInfo_func()
        {
            output_rich_textbox.AppendText(session);
            string writeInBareBoradMachineInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"SeewoWorkOrder\\\": \\\"{SeewoWorkOrder}\\\", \\\"SeewoCustomerPN\\\": \\\"{SeewoCustomerPN}\\\"}}}}\"";
            output_string = executeCMDCommand(writeInBareBoradMachineInfoCommand);
            MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
            string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

            string[] infos = getCompleteAndBareBoardInfo_func();
            if (infos.Length > 0)
            {
                string result_temp_now = "";
                if ((infos[4] == SeewoWorkOrder) && (infos[1] == SeewoCustomerPN) && (Int32.Parse(backCode) == 0))
                {
                    output_rich_textbox.AppendText("��ǰд��忨��Ϣ���Ϊ��PASS\n");
                    result_temp_now = "PASS";
                    boardMachine_textbox.Text = "";
                }
                else
                {
                    output_rich_textbox.AppendText($"��ǰд��忨��Ϣ���Ϊ��FAIL\n");
                    result_temp_now = "FAIL";
                    boardMachine_textbox.Text = "��¼ʧ�ܣ���ȷ�ϰ忨��Ϣ�Ƿ�������";
                }
                // ���λ
                refreshTestResult_button_Click(null, null);
                testResults["���Խ��"].BareBoradMachineFlashResult = result_temp_now;
                writeTestResult();
                
                //MessageBox.Show($"�������������忨��Ϣ��¼��ɣ�\n��¼�������{result_temp_now}��", "��֪ͨ��", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ��ȡ����&�忨��Ϣfunc
        private string[] getCompleteAndBareBoardInfo_func()
        {
            string[] infos = new string[5];
            try
            {
                // ��ȡ������Ϣ��¼���̼�����������Enter
                string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                output_string = executeCMDCommand(fetchDeviceInfoCommand);
                MatchCollection results_2 = Regex.Matches(output_string, "\"PCBA\" : (.*)");
                string currentPCBA = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                output_rich_textbox.AppendText("��ȡ��ǰ�豸��PCBA�ţ�" + currentPCBA + "\n");

                MatchCollection results_3 = Regex.Matches(output_string, "\"SeewoCustomerPN\" : (.*)");
                string currentSeewoCustomerPN = results_3[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                output_rich_textbox.AppendText("��ȡ��ǰ�豸��SeewoCustomerPN�ţ�" + currentSeewoCustomerPN + "\n");

                MatchCollection results_4 = Regex.Matches(output_string, "\"SeewoProductPN\" : (.*)");
                string currentSeewoProductPN = results_4[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                output_rich_textbox.AppendText("��ȡ��ǰ�豸��SeewoProductPN�ţ�" + currentSeewoProductPN + "\n");

                MatchCollection results_5 = Regex.Matches(output_string, "\"SeewoProductSN\" : (.*)");
                string currentSeewoProductSN = results_5[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                output_rich_textbox.AppendText("��ȡ��ǰ�豸��SeewoProductSN�ţ�" + currentSeewoProductSN + "\n");

                MatchCollection results_6 = Regex.Matches(output_string, "\"SeewoWorkOrder\" : (.*)");
                string currentSeewoWorkOrder = results_6[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                output_rich_textbox.AppendText("��ȡ��ǰ�豸��SeewoWorkOrder�ţ�" + currentSeewoWorkOrder + "\n");

                try
                {
                    if (currentPCBA.IsNullOrEmpty())
                    {
                        currentPCBA = "null";
                    }
                    if (currentSeewoCustomerPN.IsNullOrEmpty())
                    {
                        currentSeewoCustomerPN = "null";
                    }
                    if (currentSeewoProductPN.IsNullOrEmpty())
                    {
                        currentSeewoProductPN = "null";
                    }
                    if (currentSeewoProductSN.IsNullOrEmpty())
                    {
                        currentSeewoProductSN = "null";
                    }
                    if (currentSeewoWorkOrder.IsNullOrEmpty())
                    {
                        currentSeewoWorkOrder = "null";
                    }
                    infos[0] = currentPCBA;
                    infos[1] = currentSeewoCustomerPN;
                    infos[2] = currentSeewoProductPN;
                    infos[3] = currentSeewoProductSN;
                    infos[4] = currentSeewoWorkOrder;

                    seewoProductPN_label.Text = currentSeewoProductPN;
                    seewoProductSN_label.Text = currentSeewoProductSN;
                    seewoWorkOrder_label.Text = currentSeewoWorkOrder;
                    seewoCustomerPN_label.Text = currentSeewoCustomerPN;
                    getCurrentMacAddress_button_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return infos;
        }

        // ������Ϣ
        string SeewoProductPN;
        string SeewoProductSN;

        // �忨��Ϣ
        string SeewoWorkOrder;
        string SeewoCustomerPN;
        string MAC;


        // ������Ϣ��¼���̼�����������Enter
        private void completeMachine_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (check_device_online())
                {
                    output_rich_textbox.AppendText("��ִ�в�����������Ϣ��¼���̼�����������Enter����\n");
                    try
                    {
                        if (clientSocket != null && clientSocket.Connected)
                        {
                            // д��������Ϣ��¼���̼�����������Enter
                            string completeMachineInfo = completeMachine_textbox.Text;
                            if (string.IsNullOrEmpty(completeMachineInfo) || completeMachineInfo.Length != 38)
                            {
                                completeMachine_textbox.Text = "";
                                output_rich_textbox.AppendText($"��д����ȷ��������Ϣ�ٽ���ˢ��,��ǰ�������Ϣ����{completeMachineInfo.Length.ToString()}\n");
                                completeMachine_textbox.Text = "��¼ʧ�ܣ���ȷ��������Ϣ�Ƿ�������";
                            }
                            else
                            {
                                // ��ʼ�����Ϣ��д��
                                string[] temp = completeMachineInfo.Split("@");
                                SeewoProductPN = temp[0];
                                SeewoProductSN = temp[1];
                                writeInCompleteMachineInfo_func();
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
                        output_rich_textbox.AppendText($"������Ϣ��¼���̼�����������Enterʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
                    }
                    finally
                    {

                    }
                }
            }
        }

        // �忨��Ϣ��¼���̼�����������Enter
        private void boardMachine_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (check_device_online())
                {
                    output_rich_textbox.AppendText("��ִ�в������忨��Ϣ��¼���̼�����������Enter����\n");
                    try
                    {
                        if (clientSocket != null && clientSocket.Connected)
                        {
                            // д��忨��Ϣ��¼���̼�����������Enter    
                            string bareboradMachineInfo = boardMachine_textbox.Text;
                            if (string.IsNullOrEmpty(bareboradMachineInfo) || bareboradMachineInfo.Length != 46)
                            {
                                boardMachine_textbox.Text = "";
                                output_rich_textbox.AppendText($"��д����ȷ�İ忨��Ϣ�ٽ���ˢ��,��ǰ�������Ϣ����{bareboradMachineInfo.Length.ToString()}\n");
                                boardMachine_textbox.Text = "��¼ʧ�ܣ���ȷ�ϰ忨��Ϣ�Ƿ�������";
                            }
                            else
                            {
                                // ��ʼ�����Ϣ��д��
                                string[] temp = bareboradMachineInfo.Split("$");
                                SeewoWorkOrder = temp[0];
                                SeewoCustomerPN = temp[1];
                                MAC = temp[2];
                                writeInBareboardMachineInfo_func();
                                recordingGif_label.Visible = true;
                                recordingGif_label.Image = Image.FromFile("./img/recordingGif.gif");
                                writeInMac_button_Click(null, null);
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
                        output_rich_textbox.AppendText($"�忨��Ϣ��¼���̼�����������Enterʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
                    }
                    finally
                    {

                    }
                }
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            completeMachine_textbox.Text = "";
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            boardMachine_textbox.Text = "";
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}