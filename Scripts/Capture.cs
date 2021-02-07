using UnityEngine;

public class Capture : MonoBehaviour
{
    public int SuperSize = 1;
    public string FileName;

    void Start()
    {
        DontDestroyOnLoad(gameObject);//从初始关卡打开，时刻可以截图
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))//按下A键截屏
        {
            //unity 自带截屏，只能是截全屏
            ScreenCapture.CaptureScreenshot(FileName + System.DateTime.Now.ToString("dd.mm.hh.mm.ss") + ".png", SuperSize);
            //保存图片到项目文件夹. SuperSize就是当前分辨率的几倍.
        }
    }
}