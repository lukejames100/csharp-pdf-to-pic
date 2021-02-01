using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using O2S.Components.PDFRender4NET;

namespace csharp_pdf_to_pic
{

    public enum Definition
    {
        One=1, Two=2,Three=3,Four=4,Five=5,Six=6,Seven=7,Eight=8,Nine=9,Ten=10
    }
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.InitialDirectory = "c://";
            op.Filter = "PDF文件|*.pdf";
            op.RestoreDirectory = true;
            op.FilterIndex = 1;
            if (op.ShowDialog() == DialogResult.OK)
            {
                string fname = op.FileName;
                textBox1.Text = fname;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PDFTranImgHelp.ConvertPDF2Image(textBox1.Text, textBox2.Text+"\\", "pdfimage", 1, 1, ImageFormat.Jpeg, Definition.Five);

            //打开对应目录
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = textBox2.Text;
            System.Diagnostics.Process.Start(psi);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dl = new FolderBrowserDialog();
            dl.Description = "请选择文件路径";
            DialogResult result = dl.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                MessageBox.Show("重新再选");
            }
            string fp = dl.SelectedPath.Trim();
            DirectoryInfo thefold = new DirectoryInfo(fp);
            if (thefold.Exists)
            {
                textBox2.Text = fp;
            }
        }
    }

    public class PDFTranImgHelp
    {
        public static void ConvertPDF2Image(string pdfInputPath, string imagesOutputPath, string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition)
        {
            PDFFile pdffile = PDFFile.Open(pdfInputPath);
            if (!Directory.Exists(imagesOutputPath))
            {
                Directory.CreateDirectory(imagesOutputPath);
            }
            if (startPageNum <= 0)
            {
                startPageNum = 1;
            }
            if (endPageNum < pdffile.PageCount)
            {
                endPageNum = pdffile.PageCount;
            }
            if (startPageNum > endPageNum)
            {
                int temppagenum = startPageNum;
                startPageNum = endPageNum;
                endPageNum = temppagenum;
            }
            for (int i = startPageNum; i <= endPageNum; i++)
            {
                Bitmap pageimage = pdffile.GetPageImage(i - 1, 56 * (int)definition);
                pageimage.Save(imagesOutputPath + imageName + i.ToString() + "." + imageFormat.ToString(), imageFormat);
                pageimage.Dispose();
            }
            pdffile.Dispose();
        }
    }
}
