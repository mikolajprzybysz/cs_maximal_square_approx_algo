using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using algo_approx;
using algo_optimal;
using System.Threading;
using System.IO;
using System.Diagnostics;
namespace ApproximationAlgorithm
{
    public partial class ApproximationAlgorithmForm : Form
    {
        //ListBox.ObjectCollection RectangleangleList;
        long time;
        int squareSize;
        Zdanowicz zd;
        bwApprox bw;
        Przybyszm pm;
        optimal op;

        public ApproximationAlgorithmForm()
        {
            InitializeComponent();            
        }

        private void addRectangleButton_Click(object sender, EventArgs e)
        {
            if ( widthMaskedTextBox.Text != "" && heightMaskedTextBox.Text != "")
                RectangleListBox.Items.Add(new Rectangle(int.Parse(widthMaskedTextBox.Text), int.Parse(heightMaskedTextBox.Text)));
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApproximationAlgorithmForm.ActiveForm.Dispose();
        }

        private void zdanowiczLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripProgressBar1.Value = 0;
            if (canCalculate())
            {
                zd = new Zdanowicz(RectangleListBox.Items);
                this.Enabled = false;
                backgroundWorker1.RunWorkerAsync(zd);
            }

        }

        private void przybyszMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripProgressBar1.Value = 0;
            if (canCalculate())
            {
                pm = new Przybyszm(RectangleListBox.Items);
                this.Enabled = false;
                backgroundWorker1.RunWorkerAsync(pm);
            }
        }

        private void wachBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripProgressBar1.Value = 0;
            if (canCalculate())
            {
                bw = new bwApprox(RectangleListBox.Items);
                this.Enabled = false;
                backgroundWorker1.RunWorkerAsync(bw);
            }
        }

        private void optimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripProgressBar1.Value = 0;
            if (canCalculate())
            {
                op = new optimal(RectangleListBox.Items);
                this.Enabled = false;
                backgroundWorker1.RunWorkerAsync(op);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RectangleListBox.Items.Clear();
        }

        private void menuGenerateRandom_Click(object sender, EventArgs e)
        {
            int n = int.Parse(this.txtAmmount.Text);
            int maxW = int.Parse(this.txtMaxWidth.Text);
            int maxH = int.Parse(this.txtMaxHeight.Text);

            this.RectangleListBox.Items.Clear();
            Random r = new Random();
            for (int x = 0; x < n; x++)
            {
                this.RectangleListBox.Items.Add(new Rectangle(r.Next(1,maxW),r.Next(1,maxH)));
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void RectangleangleListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            usedRectangleViewer.setList(new List<Rectangle> { (Rectangle)RectangleListBox.SelectedItem });
            usedRectangleViewer.update();
        }

        private void RectangleangleListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                RectangleListBox.Items.Remove(RectangleListBox.SelectedItem);
            }
        }

        private void RectangleangleListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RectangleEditor recEdit = new RectangleEditor((Rectangle)RectangleListBox.SelectedItem, this);
            recEdit.Show(this);
        }

        public void editRectangle(int width, int height)
        {
            Rectangle rec = (Rectangle)RectangleListBox.SelectedItem;
            rec.setWidth(width);
            rec.setHeight(height);
            List<Rectangle> tmpList = new List<Rectangle>();
            foreach (Rectangle r in RectangleListBox.Items)
            {
                if (r == RectangleListBox.SelectedItem)
                    tmpList.Add(rec);
                else
                    tmpList.Add(r);
            }
            RectangleListBox.Items.Clear();
            foreach (Rectangle r in tmpList)
                RectangleListBox.Items.Add(r);
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openInputFile.Filter = "Text file(*.txt)|*.txt";
            this.openInputFile.ShowDialog(this);
            this.RectangleListBox.Items.Clear();

            if (this.openInputFile.FileName.Length > 0)
            {
                TextReader tr = new StreamReader(this.openInputFile.FileName);

                string[] input = tr.ReadLine().Split(' ');
                int inputSize = int.Parse(input[0]);

                if (input.Length < (inputSize * 2) + 1)
                {
                    MessageBox.Show("Incorrect input file");
                    return;

                }
                for (int x = 1; x < inputSize * 2; x += 2)
                {

                    //if (rec.Length != 2 ||  rec[0]=="" || rec[1] =="")
                    //{
                    //    MessageBox.Show("Error during import");
                    //    return;
                    //}
                    this.RectangleListBox.Items.Add(new Rectangle(int.Parse(input[x]), int.Parse(input[x + 1])));
                }
                tr.Close();
                MessageBox.Show("Import successfull, " + inputSize + " rectangles imported.");
            }
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveInputFile.Filter = "Text file(*.txt)|*.txt";
            this.saveInputFile.ShowDialog(this);

            StreamWriter fileWriter = null;
            if (saveInputFile.FileName.Length > 0)
            {
                string filePath = Path.GetFullPath(saveInputFile.FileName);
                if (File.Exists(filePath))
                    fileWriter = File.AppendText(filePath);
                else
                    fileWriter = File.CreateText(filePath);
                try
                {
                    fileWriter.Write(this.RectangleListBox.Items.Count);
                    foreach (Rectangle r in this.RectangleListBox.Items)
                        fileWriter.Write(" " + r.getWidth().ToString()
                            + " " + r.getHeight().ToString());
                }
                finally
                {
                    fileWriter.Close();
                }
                MessageBox.Show("Input rectangle save to file: \n" + filePath);
            }
        }

        private bool canCalculate()
        {
           
                if (RectangleListBox != null && RectangleListBox.Items.Count != 0)
                    return true;
                else
                {
                    MessageBox.Show("There are no rectangles", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
       
        }

        public void setProgressBarValue(int value)
        {
            try
            {
                if (value > 100)
                    value = 100;
                else if (value < 0)
                    value = 0;
                this.Invoke(new ThreadStart(delegate()
                    {
                        this.toolStripProgressBar1.Maximum = 100;
                        this.toolStripProgressBar1.Value = value;
                    }));
            }
            catch (Exception e)
            {
                Console.WriteLine("setProgressBarValue " + e.Message);
            }
        }

        public void setProgressForOptimal(int value, int max)
        {
            try
            {
                this.Invoke(new ThreadStart(delegate()
                {
                    this.toolStripProgressBar1.Maximum = max;
                    this.toolStripProgressBar1.Value = value;
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine("set progres : " + e.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //if (canCalculate())
                //{
                time = 0;
                if (e.Argument is Zdanowicz)
                {
                    e.Result = zd.Run(ref time, this);
                    squareSize = zd.side;
                }
                else if (e.Argument is bwApprox)
                {
                    e.Result = bw.Run(ref time, this);
                    squareSize = bw.squareSize;
                }
                else if (e.Argument is Przybyszm)
                {
                    e.Result = pm.Run(ref time, this);
                    squareSize = pm.side;
                }
                else if (e.Argument is optimal)
                {
                    e.Result = op.Run(ref time, this);
                    squareSize = op.side;
                }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("backgroundWorker1_DoWork : " + ex.Message);
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                List<Rectangle> solution = e.Result as List<Rectangle>;
                RectangleViewer.setList(solution);
                this.Enabled = true;
                this.toolStripProgressBar1.Value = 100;
                this.toolStripProgressBar1.Maximum = 100;
                statistics(squareSize);
                if (e.Result != null)
                    setUsedRectangles();
            }
            catch (Exception ex)
            {
                Console.WriteLine("backgroundWorker1_RunWorkerCompleted" + ex.Message);
            }
        }

        private void setUsedRectangles()
        {
            try
            {
                List<Rectangle> used = RectangleViewer.listRectangles;
                usedRectanglesListBox.Items.Clear();
                foreach (Rectangle u in used)
                {
                    usedRectanglesListBox.Items.Add(u);
                }
            }
            catch (Exception e)
            { Console.WriteLine("setUsedRectangles" + e.Message); }
        }

        private void usedRectanglesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            usedRectangleViewer.setList(new List<Rectangle> { (Rectangle)usedRectanglesListBox.SelectedItem });
            usedRectangleViewer.update();
        }

        private void statistics(int squareSize)
        {
            if (RectangleViewer.listRectangles != null)
            {
                this.txtTime.Text = time.ToString() + " msec.";
                this.txtUsed.Text = (RectangleViewer.listRectangles.Count * 100 / RectangleListBox.Items.Count).ToString() + "%";
                this.txtSize.Text = squareSize.ToString();
                RectangleViewer.update();
            }
            else
                MessageBox.Show("There is no solution", "Solution", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
