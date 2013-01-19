using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace POS_Annotator
{
    public partial class Form1 : Form
    {
        public static string tokenFile = "";
        private static string tagsetFile = "";
        public static string processedFile = "";
        private String[] tokens;
        private String[] listitem;
        private String[] tagset;
        

        public Form1()
        {
            InitializeComponent();
            dataGridView1.Visible = false;
            listView1.Visible = false;
            label1.Visible = false;
            this.dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            this.dataGridView1.CellLeave += dataGridView1_CellLeave;
            this.FormClosing += Form1_FormClosing;

        }

        private void dataGridView1_CellFormatting(object sender, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("POS")) 
                //if a column in the POS table is select, higlight the token cell as well
            {
                int ri = e.RowIndex;

                if (dataGridView1[1, ri].Selected == true)
                {
                    dataGridView1[0, ri].Style.BackColor = Color.LightGray;
                }
            }
        }

        private void dataGridView1_CellLeave(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("POS"))
            {
                int ri = e.RowIndex;
                dataGridView1[0, ri].Style.BackColor = Color.White;
            }
        }
        
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openTagsetFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "LIST files|*.list";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                tagsetFile = openFileDialog1.FileName;

                listView1.Clear();

                StreamReader tagsr = new StreamReader(tagsetFile);
                string tagsArray = tagsr.ReadToEnd();

                tagset = Regex.Split(tagsArray, @"\r?\n");

                foreach (string element in tagset)
                {
                    POS.Items.Add(element);
                    listView1.Items.Add(element);                    
                }

                listView1.Visible = true;
                label1.Visible = true;
            }

            else
            {

            }

        }

        private void openVerticalFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "VRT files|*.vrt";
            DialogResult dr = openFileDialog1.ShowDialog();

            if (listView1.Items.Count != 0)
            {
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    tokenFile = openFileDialog1.FileName;
                    dataGridView1.Visible = true;
                    dataGridView1.RowCount = 0; //clear vertical file data

                    StreamReader tokensr = new StreamReader(tokenFile);

                    string tokensArray = tokensr.ReadToEnd();
                    tokens = Regex.Split(tokensArray, @"\r?\n");

                    foreach (string element in tokens)
                    {
                        if (string.IsNullOrWhiteSpace(element) == false)
                        //ignore empty lines
                        {

                            if (Regex.IsMatch(element, @"\t") == true) //if a token already has a tag
                            {
                                listitem = Regex.Split(element, @"\t"); //separate them

                                if (listitem.Length == 2 && POS.Items.Contains(listitem[1].ToString())) 
                                    //check if it's properly formatted (i.e. one token, one tag) and that the tag is a valid one
                                {
                                    dataGridView1.Rows.Add(listitem);
                                }
                                else //if not, only add what we assume is the token
                                {
                                    dataGridView1.Rows.Add(listitem[0]);
                                }
                            }
                            else
                            {
                                dataGridView1.Rows.Add(element);
                            }
                        }
                    }
                    int init_cl = dataGridView1.CurrentCellAddress.X;
                    int init_rw = dataGridView1.CurrentCellAddress.Y;

                    if (init_cl == 0) // default position is 0,0, so move the focus to the next editable cell
                    {
                        while (dataGridView1[1, init_rw].Value != null && init_rw < dataGridView1.Rows.Count)
                        {
                            init_rw++;
                        }

                        dataGridView1.CurrentCell = dataGridView1[1, init_rw];
                        dataGridView1.CurrentCell.Selected = true;
                        dataGridView1.BeginEdit(true);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a tagset file first");
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int cur_cl = dataGridView1.CurrentCellAddress.X; //get the address of the current cell
                int cur_rw = dataGridView1.CurrentCellAddress.Y;
                int offsetcur = 1; //assuming that the next cell to edit is the one directly below ...

                if (cur_cl == 1) //only the POS column should be editable
                {
                    dataGridView1.CurrentCell.Value = listView1.SelectedItems[0].Text;
                }

                while (dataGridView1[cur_cl, cur_rw + offsetcur].Value != null && offsetcur < dataGridView1.Rows.Count)
                    //...but if the cell below already has a tag, check the next one until you find one that is empty
                {
                    offsetcur++;
                }

                dataGridView1.CurrentCell = null; //unselect the cell for the purposes of formatting
                dataGridView1.CurrentCell = dataGridView1[cur_cl, cur_rw + offsetcur]; //move to the empty cell
                dataGridView1.CurrentCell.Selected = true;  //select it
                dataGridView1.BeginEdit(true); //start editing
            }
            catch
            {
                //
            }
        }       

        private void saveFile()
        {
            var processed = File.Open(processedFile, FileMode.Create);           
            int rc = dataGridView1.Rows.Count;            

            StreamWriter processedw = new StreamWriter(processed);
            using (processedw)
            {
                int z = 0;

                while (z < rc)
                {
                    if (dataGridView1.Rows[z].Cells[1].Value != null)
                    {
                        processedw.WriteLine(dataGridView1.Rows[z].Cells[0].Value
                            + "\t"
                            + dataGridView1.Rows[z].Cells[1].Value);
                        z++;
                    }
                    else
                    {
                        processedw.WriteLine(dataGridView1.Rows[z].Cells[0].Value);
                        z++;
                    }

                }
            }
        }

        private void saveAnnotatedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult sr = saveFileDialog1.ShowDialog();
            if (sr == System.Windows.Forms.DialogResult.OK)
            {
                processedFile = saveFileDialog1.FileName;
                saveFile();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tokenFile != null)
            {
                processedFile = tokenFile + ".processed";
                saveFile();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox1 box = new AboutBox1())
            {
                box.ShowDialog(this);
            }
        }

    }
}