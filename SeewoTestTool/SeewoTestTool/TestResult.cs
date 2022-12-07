[Serializable]

// 定义TestResult类来存储设备各项功能测试结果
public class TestResult
{
    private string network_test_result;
    private string network2_test_result;
    private string firmwareVerified_test_result;
    private string redgreenLED_test_result;
    private string resetButton_test_result;
    private string threeCamera_test_result;
    private string threeCamera2_test_result;
    private string audioIn_test_result;
    private string audioIn2_test_result;
    private string arrayMic_test_result;
    private string macAddress_test_result;

    // get set简化写法，直接通过xxx.xxx返回或设置值
    public string NetworkTestResult
    {
        get { return network_test_result; }
        set { network_test_result = value; }
    }

    public string Network2TestResult
    {
        get { return network2_test_result; }
        set { network2_test_result = value; }
    }

    public string FirmwareVerifiedResult
    {
        get { return firmwareVerified_test_result; }
        set { firmwareVerified_test_result = value; }
    }

    public string RedGreenLEDResult
    {
        get { return redgreenLED_test_result; }
        set { redgreenLED_test_result = value; }
    }

    public string ResetButtonResult
    {
        get { return resetButton_test_result; }
        set { resetButton_test_result = value; }
    }

    public string ThreeCameraResult
    {
        get { return threeCamera_test_result; }
        set { threeCamera_test_result = value; }
    }

    public string ThreeCamera2Result    
    {
        get { return threeCamera2_test_result; }
        set { threeCamera2_test_result = value; }
    }

    public string AudioInResult
    {
        get { return audioIn_test_result; }
        set { audioIn_test_result = value; }
    }
    public string AudioIn2Result
    {
        get { return audioIn2_test_result; }
        set { audioIn2_test_result = value; }
    }

    public string ArrayMicResult
    {
        get { return arrayMic_test_result; }
        set { arrayMic_test_result = value; }
    }

    public string MacAddressResult
    {
        get { return macAddress_test_result; }
        set { macAddress_test_result = value; }
    }

}
