using System.Threading;

namespace Impure.Object_Classes
{
    public class HotKeys
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetKeyState(int vKey);

        public void KeyLoop()
        {
            while (true)
            {
                int Key = -1;
                Key = GetKeyState(45);//Menu key 90 = Z // 45 = Insert
                if ((Key & 0x8000) != 0)
                {
                    MainWindow.self.Visibility = MainWindow.self.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
                    Thread.Sleep(500);
                }
                Thread.Sleep(1);
            }
        }
    }
}
