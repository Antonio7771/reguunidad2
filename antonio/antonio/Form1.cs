using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Net.Sockets;
using System.IO;


namespace antonio
{
    public partial class Form1 : Form
    {
        static private NetworkStream stream;
        static private StreamWriter streamw;
        static private StreamReader streamr;
        static private TcpClient client = new TcpClient();
        static private string nickname = "unknown";

        private delegate void DAddItem(String s);

        private void AddItem(String s)
        {
            listBox1.Items.Add(s);
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            streamw.WriteLine(textBox1.Text);
            streamw.Flush();
            textBox1.Clear();
        }

        void Listen()
        {
            while (client.Connected)
            {
                try
                {
                    this.Invoke(new DAddItem(AddItem), streamr.ReadLine());

                }
                catch
                {
                    MessageBox.Show("No se ha podido conectar al servidor");
                    Application.Exit();
                }
            }
        }

        void Conectar()
        {
            try
            {
                client.Connect("127.0.0.1", 8000);
                if (client.Connected)
                {
                    Thread t = new Thread(Listen);

                    stream = client.GetStream();
                    streamw = new StreamWriter(stream);
                    streamr = new StreamReader(stream);

                    streamw.WriteLine(nickname);
                    streamw.Flush();

                    t.Start();
                }
                else
                {
                    MessageBox.Show("Servidor no Disponible");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Servidor no Disponible");
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            listBox1.Enabled = true;
            textBox1.Enabled = true;

            textBox2.Enabled = false;
            label1.Enabled = false;
            button2.Enabled = false;

            nickname = textBox2.Text;
            Conectar();
        }
    }
}
