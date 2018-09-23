using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using RSAExample;

namespace GUISimpleTCPClient
{

    public partial class GUISimpleTCPClient : Form
    {

        TcpClient client = null;
        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;
        public byte[] key;
        Symmetric_Cryptography sy_C = new Symmetric_Cryptography();
        AsymmetricEncryptionUtility Asy_e = new AsymmetricEncryptionUtility();
        RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();


        public GUISimpleTCPClient()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient(ipAddress.Text, Convert.ToInt32(port.Text));
                listBox1.Items.Add("Connected to Server");
                stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);
                string msg = reader.ReadLine();
                listBox1.Items.Add("Recieved form server:");
                listBox1.Items.Add(msg);
                button1.Enabled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void send_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = M.Text;
                writer.WriteLine(msg);
                writer.Flush();
                msg = reader.ReadLine();
                if (msg.Length != 0)
                    listBox1.Items.Add(msg);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string NameOfFile = openFileDialog1.FileName;

                string fileName = NameOfFile;

                FileInfo f = new FileInfo(fileName);
                long s1 = f.Length;
                if (s1 > 50000000)
                {
                    MessageBox.Show("Selected file is too large ! : " + Convert.ToString(s1));
                    MessageBox.Show("Application is aborting! . . .");
                    Application.Exit();
                }
                try
                {
                    string[] file = NameOfFile.Split('\\');
                    MessageBox.Show(file[file.Length - 1]);
                    byte[] FileRAW = File.ReadAllBytes(NameOfFile);

                    string file_Encrypted = sy_C.encryptFile(FileRAW);
                    string s_key = Convert.ToBase64String(sy_C.sessionkey);
                    string iv = Convert.ToBase64String(sy_C.sessioniv);
                    s_key = s_key.ToString();
                    iv = iv.ToString();

                    byte[] encrypted_sessionkey = Asy_e.EncryptData(s_key, File.ReadAllText("PublicKey.xml"));
                    byte[] encrypted_iv = Asy_e.EncryptData(iv, File.ReadAllText("PublicKey.xml"));
                    s_key = Convert.ToBase64String(encrypted_sessionkey);
                    iv = Convert.ToBase64String(encrypted_iv);
                    writer.WriteLine(file[file.Length - 1]);
                    writer.WriteLine(s_key);
                    writer.WriteLine(iv);
                    writer.WriteLine(file_Encrypted);
                    writer.Flush();
                    string message = reader.ReadLine();
                    MessageBox.Show(message);

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void port_TextChanged(object sender, EventArgs e)
        {

        }

        private void ipAddress_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
