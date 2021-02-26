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
    public partial class Rechercher_Des_Achats : Form
    {
        private int NumAchte = -1;
        DataClasses1DataContext data = new DataClasses1DataContext();
        
        public void afficher_par_mounth(int cred)
        {

            string mounth = dateTimePicker1.Value.Month.ToString();
            if (int.Parse(dateTimePicker1.Value.Month.ToString()) < 10)
                mounth = "0" + dateTimePicker1.Value.Month.ToString();
            
            var data_today = from a in data.Achate
                             join p in data.Produit on a.NumProduit equals p.NumProduit
                             join v in data.Vendeur on a.NumVendeur equals v.NumVendeur
                             where a.dateAchate.ToString().Contains("-" + mounth + "-") && a.payment> cred
                             select new { num= a.NumAchate, Nom_Produit= p.nameProduit, Quantité_Produit= a.quantitéProduit, Prix_Fournisseur= p.prix_Fournisseur, Prix_Achate= a.prixAchate, Nom_Vendeur= v.nameVendeur, Solde_De_Crédit=a.payment };
            var today_Quantité_Produit = (from a in data.Achate where a.dateAchate.ToString().Contains("-" + mounth + "-") && a.payment > cred select a.quantitéProduit).Sum();
            var today_totale_prix = (from a in data.Achate where a.dateAchate.ToString().Contains("-" + mounth + "-") && a.payment > cred select a.prixAchate * a.quantitéProduit).Sum();
            var today_totale_crédit = (from a in data.Achate where a.dateAchate.ToString().Contains("-" + mounth + "-") && a.payment > cred select a.payment).Sum();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = data_today;
            textBox1.Text = today_Quantité_Produit.ToString() + " Produit";
            textBox2.Text = today_totale_prix.ToString() + " DHs";
            textBox3.Text= today_totale_crédit.ToString() + " DHs";
            dataGridView1.ClearSelection();


            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[2].Width = 170;
            dataGridView1.Columns[3].Width = 170;
            dataGridView1.Columns[4].Width = 120;
            dataGridView1.Columns[1].Width = 400;
        }
        public void afficher_par_day(int cred)
        {


            var data_today = from a in data.Achate
                             join p in data.Produit on a.NumProduit equals p.NumProduit
                             join v in data.Vendeur on a.NumVendeur equals v.NumVendeur
                             where a.dateAchate == dateTimePicker1.Value && a.payment > cred
                             select new { num = a.NumAchate, Nom_Produit = p.nameProduit, Quantité_Produit = a.quantitéProduit, Prix_Fournisseur = p.prix_Fournisseur, Prix_Achate = a.prixAchate, Nom_Vendeur = v.nameVendeur , Solde_De_Crédit = a.payment };
            var today_Quantité_Produit = (from a in data.Achate where a.dateAchate == dateTimePicker1.Value && a.payment > cred select a.quantitéProduit).Sum();
            var today_totale_prix = (from a in data.Achate where a.dateAchate == dateTimePicker1.Value && a.payment > cred select a.prixAchate * a.quantitéProduit).Sum();
            var today_totale_crédit = (from a in data.Achate where a.dateAchate == dateTimePicker1.Value && a.payment > cred select a.payment).Sum();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = data_today;
            if (today_Quantité_Produit.ToString() == "")
                today_Quantité_Produit = 0;
            if (today_totale_prix.ToString() == "")
                today_totale_prix = 0;
            textBox1.Text = today_Quantité_Produit.ToString() + " Produit";
            textBox2.Text = today_totale_prix.ToString() + " DHs";
            textBox3.Text = today_totale_crédit.ToString() + " DHs";
            dataGridView1.ClearSelection();
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[2].Width = 170;
            dataGridView1.Columns[3].Width = 170;
            dataGridView1.Columns[4].Width = 120;
            dataGridView1.Columns[1].Width = 400;

        }
        public Rechercher_Des_Achats()
        {
            InitializeComponent();
        }

        private void Rechercher_Des_Achats_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = DateTime.Today;
            radioButton1.Checked = true;
            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = false;
            textBox1.Text = " 0 Produit";
            textBox2.Text = " 0 DHs";
            textBox3.Text = " 0 DHs";
            if (checkBox1.Checked==true)
                afficher_par_day(0);
            else
                afficher_par_day(-1);
            button2.Enabled = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {

                if (checkBox1.Checked == true)
                    afficher_par_day(0);
                else
                    afficher_par_day(-1);
            }

            else
            {
                if (checkBox1.Checked == true)
                    afficher_par_mounth(0);
                else
                    afficher_par_mounth(-1);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                if (checkBox1.Checked == true)
                    afficher_par_mounth(0);
                else
                    afficher_par_mounth(-1);
                button1.Enabled = false;
                button2.Enabled = true;
            }

            else
            {
                if (checkBox1.Checked == true)
                    afficher_par_day(0);
                else
                    afficher_par_day(-1);

                button2.Enabled = false;
                button1.Enabled = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                this.NumAchte = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            else
                this.NumAchte = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.NumAchte != -1)
            {
                MessageBox.Show(this.NumAchte.ToString());
                dataGridView1.ClearSelection();
                this.NumAchte = -1;
            }
            else
                MessageBox.Show("incorrect Supprimer Num achate non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
            
                if (checkBox1.Checked == true)
                    afficher_par_mounth(0);
                else
                    afficher_par_mounth(-1);
            }

            else
            {
                if (checkBox1.Checked == true)
                    afficher_par_day(0);
                else
                    afficher_par_day(-1);

            }
        }

       
    }
}
