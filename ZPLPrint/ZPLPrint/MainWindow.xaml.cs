using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZPLPrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string ZPLString = "^XA^SEE:UHANGUL.DAT^FS^CW1,E:KFONT3.FNT^CI26^FS^LH0,0^CW1,B:ANMDJ.TTF^CI17^FO8,8^GB543,384,1^FS^FO8,55^GB543,0,1^FS^FO9,140^GB543,0,1^FS^FO8,359^GB543,0,1^FS^FO8,320^GB543,0,1^FS^FO130,8^GB0,315,1^FS^FO350,8^GB0,132,1^FS^FO388,150^GE160,160,1^FS^FO395,200^GB148,0,1^FS^FO388,240^GB160,0,1^FS^FO30,14^A150,40^F8^FD차종 ^FS^FO200,14^A150,40^F8^FDALC^FS^FO420,14^A150,40^F8^FDSEQ^FS^FO30,190^A0N150,40^F8^FDALC^FS^FO20,240^A0N150,40^F8^FDCODE^FS^FO200,370^A150,20^F8^FDInspected by N-Vision System^FS^FO10,70^A0N190,80^F8^FD_car_^FS^FO165,70^A0N150,80^F8^FD_productName_^FS^FO145,155^A100,20^F8^FD_ALC_^FS^BY3,3,80^FO138,190^B3N,60,N,N,N^FD_barcode_^FS^FO145,290^A100,20^F8^FDLOT:_inspectionSeq_^FS^FO440,165^A150,40^F8^FDOK^FS^FO398,210^A150,30^F8^FD_inspectionDate_^FS^FO410,250^A150,35^F8^FD_inspector_^FS^FO150,330^A150,30^F8^FD_manufacturer_^FS^XZ";

        public string product;
        private readonly PasswordControl PasswordControl = new PasswordControl();
        Window window;
        public MainWindow()
        {
            InitializeComponent();
            inspectionDate.Text = System.DateTime.Now.ToString("yyyyMMdd");
            PasswordControl.Succeeded += OnZPLPrint;
            PasswordControl.Cancelled += OnPasswordClosed;
        }

        private void ZPLPrint_Click(object sender, RoutedEventArgs e)
        {
            window = new Window();
            window.Title = "Password";
            window.Content = PasswordControl;
            window.Topmost = true;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Height = 100;
            window.Width = 400;
            window.ShowDialog();

        }

        private void productName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                product = ((ComboBoxItem)productName.SelectedItem).Content.ToString();
                string PNO = product.Substring(0, 2);
                string alc = product.Substring(2, 1);
                string code = getProductBarcode(PNO, alc);
                ALC.Text = code;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("제품 선택 중 에러가 발생했습니다. 다시 한번 선택해 주세요.");
            }
        }

        private string getProductBarcode(string pno, string alc)
        {
            string barcode = "84260-";

            switch (pno)
            {
                case "F1":
                    barcode += "S8000";
                    break;
                case "F2":
                    barcode += "S8010";
                    break;
                case "F3":
                    barcode += "S8020";
                    break;
                case "F4":
                    barcode += "S8030";
                    break;
                case "F5":
                    barcode += "S8040";
                    break;
                case "F6":
                    barcode += "S8050";
                    break;
                case "F7":
                    barcode += "S8060";
                    break;
                case "F8":
                    barcode += "S8070";
                    break;
                case "F9":
                    barcode += "S8080";
                    break;
                case "FA":
                    barcode += "S8090";
                    break;
                case "FD":
                    barcode += "S8700";
                    break;
                case "FE":
                    barcode += "S8710";
                    break;
                case "FF":
                    barcode += "S8720";
                    break;
                case "FG":
                    barcode += "S8730";
                    break;
                default:
                    break;
            }

            switch (alc)
            {
                case "N":
                    barcode += "NNB";
                    break;
                case "R":
                    barcode += "RBD";
                    break;
                case "W":
                    barcode += "WDN";
                    break;
                default:
                    break;
            }

            return barcode;
        }

        private void OnZPLPrint(object sender, EventArgs e)
        {
            window.Close();
            ZPLString = ZPLString.Replace("_car_", car.Text.ToString());
            ZPLString = ZPLString.Replace("_productName_", product);
            ZPLString = ZPLString.Replace("_ALC_", ALC.Text.ToString());
            ZPLString = ZPLString.Replace("_barcode_", product);
            ZPLString = ZPLString.Replace("_inspectionSeq_", LOT.Text.ToString());
            ZPLString = ZPLString.Replace("_inspectionDate_", inspectionDate.Text.ToString());
            ZPLString = ZPLString.Replace("_inspector_", inspector.Text.ToString());
            ZPLString = ZPLString.Replace("_manufacturer_", manufacturer.Text.ToString());

            try
            {
                TcpClient client = new TcpClient(printIP.Text, 9100);
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(ZPLString);

                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Flush();

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("프린트 에러");
            }
        }

        private void OnPasswordClosed(object sender, EventArgs e)
        {
            window.Close();
        }

    }
}
