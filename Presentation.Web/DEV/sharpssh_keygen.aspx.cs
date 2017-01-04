using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using System.Net;
using System.IO;
using PAObjectsLib;
using Vim25Api;
using System.Reflection;
using NCC.ClearView.Application.Core.ClearViewWS;
using Tamir.SharpSsh;
using Tamir.Streams;
using Tamir.SharpSsh.jsch;

namespace NCC.ClearView.Presentation.Web
{
    public partial class sharpssh_keygen : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intCount = 0;
        protected int[] arProcessing = new int[5] { 8, 45, 47, 92, 124 };   // 8 = backspace, 92 = \, 124 = |, 47 = /, 45 = -
        private string strSSH_Carriage = "\r";

        protected void Page_Load(object sender, EventArgs e)
        {
            RunExample(new string[] { "rsa", "keyfile", "comment" });
        }

        public void RunExample(params string[] arg)
        {
            if (arg.Length < 3)
            {
                Response.Write(
                    "usage: java KeyGen rsa output_keyfile comment\n" +
                    "       java KeyGen dsa  output_keyfile comment");
                return;
            }

            try
            {
                //Get sig type ('rsa' or 'dsa')
                String _type = arg[0];
                int type = 0;
                if (_type.Equals("rsa")) { type = KeyPair.RSA; }
                else if (_type.Equals("dsa")) { type = KeyPair.DSA; }
                else
                {
                    Response.Write(
                        "usage: java KeyGen rsa output_keyfile comment\n" +
                        "       java KeyGen dsa  output_keyfile comment");
                    return;
                }
                //Output file name
                String filename = arg[1];
                //Signature comment
                String comment = arg[2];

                //Create a new JSch instance
                JSch jsch = new JSch();

                //Prompt the user for a passphrase for the private key file
                Variables oVar = new Variables((int)CurrentEnvironment.PNCNT_PROD);
                String passphrase = oVar.ADPassword();


                //Generate the new key pair
                KeyPair kpair = KeyPair.genKeyPair(jsch, type);
                //Set a passphrase
                kpair.setPassphrase(passphrase);
                //Write the private key to "filename"
                using (FileStream fs = new FileStream(Server.MapPath("~/DEV/" + filename), FileMode.OpenOrCreate, FileAccess.Write))
                    kpair.writePrivateKey(fs);
                //Write the public key to "filename.pub"
                using (FileStream fs = new FileStream(Server.MapPath("~/DEV/" + filename + ".pub"), FileMode.OpenOrCreate, FileAccess.Write))
                    kpair.writePublicKey(fs, comment);
                //Print the key fingerprint
                Response.Write("Finger print: " + kpair.getFingerPrint());
                //Free resources
                kpair.dispose();
            }
            catch (Exception e)
            {
                Response.Write(e);
            }
            return;
        }
    }
}
