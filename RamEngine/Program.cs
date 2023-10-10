using System.Windows.Forms;

namespace RamEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BytecodeEngine game = new BytecodeEngine();
        }
    }
}
