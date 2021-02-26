using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gestion_des_etreprises
{
    public partial class Gestion_De_Réparation : Form
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        private int index = -1;
        public Gestion_De_Réparation()
        {
            InitializeComponent();
  
        }

        private void Gestion_De_Réparation_Load(object sender, EventArgs e)
        {
            textBox3.Enabled = textBox2.Enabled = false;
            checkBox1.Checked = true;
            dateTimePicker2.Enabled = false;
            numericUpDown1.Maximum = numericUpDown4.Maximum= 10000;
            numericUpDown1.Minimum = 0;
            dateTimePicker1.MaxDate = dateTimePicker2.MaxDate = DateTime.Today;
            numericUpDown4.Maximum = 100000;
            afficher();
            radioButton1.Checked = true;

        }
        public void afficher()
        {

            var rep = from r in data.Reparation where r.Date_Reparation == dateTimePicker2.Value select new { r.Num_Reparation, r.Titre_Reparation, r.Prix, r.Telephone_Client, r.Commenter , Solde_De_Crédit = r.payment  };
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = rep;
            efacer_champ();

            if(dataGridView1.Rows.Count==0)
            {
                textBox2.Text = "0  Réparations";
                textBox3.Text = "0 Dhs";
            }
            else
            {
                textBox2.Text = (from p in data.Reparation where p.Date_Reparation == dateTimePicker2.Value select p).Count().ToString() + " Produits";
                textBox3.Text = (from p in data.Reparation where p.Date_Reparation == dateTimePicker2.Value select p.Prix).Sum().ToString() + " Dhs";

            }

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 300;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 120;
            radioButton1.Checked = true;

        }
        public void efacer_champ()
        {
            textBox1.Text = textBox4.Text="";
            numericUpDown1.Value  = 0;
            dateTimePicker1.Value = dateTimePicker2.Value;
            this.index = -1;
            dataGridView1.ClearSelection();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                dateTimePicker2.Enabled = false;
                dateTimePicker2.Value = DateTime.Today;
            }
            else
                dateTimePicker2.Enabled = true;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            afficher();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Rows.Count > 0 && e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {


                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                numericUpDown1.Value = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                dateTimePicker1.Value = dateTimePicker2.Value;
                richTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString()) == 0)
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;

                numericUpDown4.Value = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                this.index = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            else
                this.index = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            efacer_champ();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="" && textBox1.Text.Length< 100  && numericUpDown1.Value< 10000 && numericUpDown4.Value < 10000 && (textBox4.Text.Length==0 || textBox4.Text.Length ==10) )
            {
                try
                {
                   
                        var a = new Reparation();
                    a.Commenter = richTextBox1.Text;
                    a.Date_Reparation = dateTimePicker1.Value;
                    a.Prix = int.Parse(numericUpDown1.Value.ToString());
                    a.Telephone_Client = textBox4.Text;
                    a.Titre_Reparation = textBox1.Text;
                    a.payment = int.Parse(numericUpDown4.Value.ToString());
                    data.Reparation.InsertOnSubmit(a);
                    data.SubmitChanges();

                        afficher();
                        efacer_champ();
                        MessageBox.Show(" Reparation ajouter successful");

                

                }
                catch
                {
                    MessageBox.Show("error systeme ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                if(textBox1.Text == "")
                    MessageBox.Show("incorrect  titre Reparation vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if(textBox1.Text.Length>=100)
                    MessageBox.Show("incorrect  titre Reparation supérieure  a 100 caractère !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (numericUpDown1.Value >= 10000 || numericUpDown4.Value >= 10000)
                    MessageBox.Show("impossible   prix Reparation supérieure  a 10000 Dh !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (textBox4.Text.Length!=10 || textBox4.Text.Length!=0)
                    MessageBox.Show("incorrect nomber de telephone  !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                    MessageBox.Show("incorrect Informations Réparation  !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.index!=-1 &&  textBox1.Text != "" && textBox1.Text.Length < 100  && numericUpDown1.Value < 10000 && (textBox4.Text.Length == 0 || textBox4.Text.Length == 10) && numericUpDown4.Value<10000)
            {
                try
                {
                    if (MessageBox.Show("vous le voulez bien Modifier", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var a = (from r in data.Reparation where r.Num_Reparation ==this.index  select r).Single();

                        a.Commenter = richTextBox1.Text;
                        a.Date_Reparation = dateTimePicker1.Value;
                        a.Prix = int.Parse(numericUpDown1.Value.ToString());
                        a.Telephone_Client = textBox4.Text;
                        a.Titre_Reparation = textBox1.Text;
                        a.payment = int.Parse(numericUpDown4.Value.ToString());
                        data.SubmitChanges();

                        afficher();
                        efacer_champ();
                        MessageBox.Show(" Reparation Modifier successful");
                    }


                }
                catch
                {
                    MessageBox.Show("error systeme ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                if(this.index ==-1)
                    MessageBox.Show("incorrect Modifier Num Reparation non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (textBox1.Text == "")
                    MessageBox.Show("incorrect  titre Reparation vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (textBox1.Text.Length >= 100)
                    MessageBox.Show("incorrect  titre Reparation supérieure  a 100 caractère !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (numericUpDown1.Value >= 10000 || numericUpDown4.Value >= 10000)
                    MessageBox.Show("impossible   prix Reparation supérieure  a 10000 Dh !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (textBox4.Text.Length != 10 || textBox4.Text.Length != 0)
                    MessageBox.Show("incorrect nomber de telephone  !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                    MessageBox.Show("incorrect Informations Réparation  !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.CurrentRow.Index >= 0 && dataGridView1.CurrentRow.Index < dataGridView1.Rows.Count && this.index != -1)
            {
                try
                {

                    if (MessageBox.Show("vous le voulez bien Supprimé", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var a = (from r in data.Reparation where r.Num_Reparation == this.index select r).Single();
                        data.Reparation.DeleteOnSubmit(a);
                        data.SubmitChanges();
                        afficher();
                        efacer_champ();
                        MessageBox.Show("produit Réparation successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                }
                catch
                {
                    MessageBox.Show("error systeme ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("incorrect Supprimer Num Réparation non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void Gestion_De_Réparation_Deactivate(object sender, EventArgs e)
        {
            Form2 formGlobal = (Form2)Application.OpenForms["Form2"];
            formGlobal.afficher();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = false;
            numericUpDown4.Value = 0;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown4.Enabled = true;
        }
    }
} 
