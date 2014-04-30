using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.Diagnostics;
using Microsoft.Win32;
using System.Reflection;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        string username = "";
        string password = "";

        Assembly _assembly;
        Stream _imageStream;

        public Form1()
        {
            InitializeComponent();
            try {
            TextReader tr = new StreamReader("settings.cnf");
            String passline = tr.ReadLine();
            //if ()
            string[] words = passline.Split('|');
            username = words[0];
            password = words[1];
            tr.Close();
            uname.Text = username;
            pwd.Text = password;
            } catch (FileNotFoundException) {
                
            }
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> postParameters = new Dictionary<string, string>();
            HttpStatusCode wRespStatusCode = new HttpStatusCode();

            

            if (uname.Text != "" && pwd.Text != "")
            {
                postParameters.Add("username", uname.Text);
                postParameters.Add("password", pwd.Text);
                postParameters.Add("strAGB", "AGB");
                postParameters.Add("strHinweis", "Zahlungsbedingungen");

                postParameters.Add("Login", "");

                string postData = "";

                String url = "https://hotspot.t-mobile.net/wlan/index.do";

                foreach (string key in postParameters.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(postParameters[key]) + "&";
                }

                byte[] data = Encoding.ASCII.GetBytes(postData);

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url + "?" + postData);
                myHttpWebRequest.Method = "GET";

                Console.WriteLine(postData);

                
                
                try
                {
                    // execute the request
                    HttpWebResponse response = (HttpWebResponse)
                        myHttpWebRequest.GetResponse();
                    // we will read data via the response stream
                    wRespStatusCode = response.StatusCode;
                    label1.Text = wRespStatusCode.ToString();
                    StringBuilder sb = new StringBuilder();
                    Stream s = response.GetResponseStream();
                    byte [] buf = new byte[8192];
                    string tempString = null;
                    int count = 0;
                    do
                    {
                        count = s.Read(buf, 0, buf.Length);
                        if (count != 0)
                        {
                            tempString = Encoding.ASCII.GetString(buf, 0, count);
                            sb.Append(tempString);
                        }
                    } while (count != 0);

                }
                catch (WebException)
                {
                    label1.Text = "error";
                }
            }
            else
            {
                label1.Text = "Please enter credentials";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (uname.Text != "" && pwd.Text != "")
            {
                TextWriter tw = new StreamWriter("settings.cnf");

                // write a line of text to the file
                tw.WriteLine(uname.Text + "|" + pwd.Text);

                // close the stream
                tw.Close();

                try
                {
                    _assembly = Assembly.GetExecutingAssembly();
                    _imageStream = _assembly.GetManifestResourceStream("HotspotLogin.haken.jpg");
                    pictureBox1.Image = new Bitmap(_imageStream);
                }
                catch
                {
                    MessageBox.Show("Error accessing resources!");
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = GetDefaultBrowserPath();
            p.StartInfo.Arguments = "http://www.pornopurzel.de";
            p.Start();
        }


        private string GetDefaultBrowserPath()
        {
            string key = @"HTTP\shell\open\command";
            using (RegistryKey registrykey = Registry.ClassesRoot.OpenSubKey(key, false))
            {
                return ((string)registrykey.GetValue(null, null)).Split('"')[1];
            }
        }
     
    }
}
