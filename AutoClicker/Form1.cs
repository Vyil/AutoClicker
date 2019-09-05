using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer timer;

        const int MOUSEEVENTF_LEFTDOWN = 2;
        const int MOUSEEVENTF_LEFTUP = 4;
        const int MOUSEEVENTF_RIGHTDOWN = 8;
        const int MOUSEEVENTF_RIGHTUP = 16;
        const int MOUSEEVENTF_MIDDLEUP = 32;
        const int MOUSEEVENTF_MIDDLEDOWN = 64;
        //input type constant
        const int INPUT_MOUSE = 0;

        Random random = new Random();
        private int delay = 0;
        private int clicks = 0;

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        public struct INPUT
        {
            public uint type;
            public MOUSEINPUT mi;
        };

        public Form1()
        {
            InitializeComponent();
            stopButton.Enabled = false;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (stopButton.Enabled)
            {
                startButton.Enabled = true;
                stopButton.Enabled = false;
                statusText.Text = "Disabled";
                timer.Stop();
                timer.Dispose();
            }
            clickCounter.Text = clicks.ToString();
            
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            start(converToMS(Decimal.ToInt32(secondsValue.Value), Decimal.ToInt32(minutesValue.Value)));
        }
        
        public void start(int time)
        {
            
            if (checkBoxDelay.Checked)
            {
                int delayGiven = Decimal.ToInt32(numericDelay.Value);
                delay = random.Next(0, delayGiven);
            }

            if (time == 0)
            {
                startButton.Enabled = true;
                noTimeLabel.Text = "Can not start without a set time";
                return;
            }
            timer = new System.Timers.Timer(time + delay);
            timer.Elapsed += clickatcur;
            timer.Enabled = true;
            startButton.Enabled = false;
            stopButton.Enabled = true;
            statusText.Text = "Enabled";
            clicks = 0;
            clickCounter.Text = "Clicking";
            noTimeLabel.Text = "";
        }

        public int converToMS(int seconds, int minutes)
        {

            if (seconds == 0 && minutes == 0)
            {
                return 0;
            }
            else if (seconds == 0 && minutes >= 0)
            {
                return minutes * 60000;
            }
            else if (seconds >= 0 && minutes == 0)
            {

                return seconds * 1000;
            }
            else
            {
                int secondsMS = seconds * 1000;
                int minutesMS = minutes * 60000;
                int result = secondsMS + minutesMS;
                return result;
            }
        }

        public void clickatcur(Object source, ElapsedEventArgs e)
        {
            //Get new delay after each loop
            if(checkBoxDelay.Enabled)
            {
                int delayGiven = Decimal.ToInt32(numericDelay.Value);
                delay = random.Next(0, delayGiven);
            }            

            INPUT i = new INPUT();
            i.type = INPUT_MOUSE;
            i.mi.dx = 0;
            i.mi.dy = 0;
            i.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
            i.mi.time = 0;
            SendInput(1, ref i, Marshal.SizeOf(i));
            i.mi.dwFlags = MOUSEEVENTF_LEFTUP;
            SendInput(1, ref i, Marshal.SizeOf(i));

            clicks++;
        }

    }
}
