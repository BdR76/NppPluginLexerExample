using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            String ver = GetVersion();
            lblTitle.Text += ver;
        }

        private string GetVersion()
        {
            // version for example "1.3.0.0"
            String ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // if 4 version digits, remove last two ".0" if any, example  "1.3.0.0" ->  "1.3" or  "2.0.0.0" ->  "2.0"
            while ((ver.Length > 4) && (ver.Substring(ver.Length - 2, 2) == ".0"))
            {
                ver = ver.Substring(0, ver.Length - 2);
            }
            return ver;
        }

        private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lbl = (sender as LinkLabel);
            string url = lbl.Text;

            if (url[0] == '@')
            {
                url = "https://github.com/" + url.Substring(1, url.Length - 1);
            };

            // Change the color of the link text by setting LinkVisited to true.
            lbl.LinkVisited = true;

            // Call the Process.Start method to open the default browser with a URL:
            System.Diagnostics.Process.Start(url);
        }
    }
}
