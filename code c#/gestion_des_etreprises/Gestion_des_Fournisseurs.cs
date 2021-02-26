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
    public partial class Gestion_des_Fournisseurs : Form
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        private int IndexFournisseur = -1;
        public Gestion_des_Fournisseurs()
        {
            InitializeComponent();
        }
        public void efacer_champ()
        {
            textBox1.Text = textBox2.Text = "";
            richTextBox1.Text = "";
        }
        public void afficher()
        {
            var forn = from f in data.Fournisseur select new { Nomber_Fournisseur = f.NumFournisseur, Nom_Fournisseur = f.nameFournisseur, Telephone_Fournisseur = f.TelFournisseur, Address_Fournisseur = f.AddressFournisseur  };
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = forn;
            dataGridView1.ClearSelection();
            this.IndexFournisseur = -1;
            efacer_champ();
        }
        private void Gestion_des_Fournisseurs_Load(object sender, EventArgs e)
        {
            textBox3.Enabled = textBox4.Enabled = textBox5.Enabled = textBox6.Enabled = false;
            textBox3.Text = "0 Produits";
             textBox4.Text = textBox5.Text = textBox6.Text = "0 DH";
            afficher();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            efacer_champ();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text.Length <= 100 && (textBox2.Text.Length == 10 || textBox2.Text.Length == 0) && richTextBox1.Text.Length <= 100)
            {
                try
                {
                    Fournisseur f = new Fournisseur();
                    f.AddressFournisseur = richTextBox1.Text;
                    f.nameFournisseur = textBox1.Text;
                    f.TelFournisseur = textBox2.Text;
                    data.Fournisseur.InsertOnSubmit(f);
                    data.SubmitChanges();
                    afficher();

                    MessageBox.Show(" Fournisseur ajouter successful");

                }
                catch
                {
                    MessageBox.Show("error systeme !!");
                }

            }
            else
            {
                if (textBox1.Text.Length > 99)
                    MessageBox.Show("incorrect longueur de nom Fournisseur supérieur a 50 caractère !!");
                else if (textBox2.Text.Length != 10 || textBox2.Text.Length != 0)
                    MessageBox.Show("incorrect longueur de Nomber Telephone Fournisseur !!");
                else if (richTextBox1.Text.Length > 99)
                    MessageBox.Show("incorrect longueur de Address Fournisseur supérieur a 100 caractère !!");
                else if (textBox1.Text == "")
                    MessageBox.Show("champ est vide !!");
                else
                    MessageBox.Show("error systeme !!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IndexFournisseur != -1)
            {
                try
                {
                    if (MessageBox.Show("vous le voulez bien Supprimer", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var f = (from fo in data.Fournisseur where fo.NumFournisseur == this.IndexFournisseur select fo).Single();
                        data.Fournisseur.DeleteOnSubmit(f);
                        data.SubmitChanges();
                        afficher();
                        MessageBox.Show(" Fournisseur Supprimer successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
                catch
                {
                    MessageBox.Show("error systeme !!");
                }

            }
            else
                MessageBox.Show("incorrect Supprimer Num Fournisseur non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (IndexFournisseur != -1 && textBox1.Text != "" && textBox1.Text.Length <= 50 && textBox2.Text.Length <= 10 && richTextBox1.Text.Length <= 100)
            {
                try
                {
                    if (MessageBox.Show("vous le voulez bien Modifier", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var f = (from fo in data.Fournisseur where fo.NumFournisseur == this.IndexFournisseur select fo).Single();
                        f.AddressFournisseur = richTextBox1.Text;
                        f.nameFournisseur = textBox1.Text;
                        f.TelFournisseur = textBox2.Text;
                        data.SubmitChanges();
                        afficher();
                        MessageBox.Show("produit Modifier successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
                catch
                {
                    MessageBox.Show("error systeme !!");
                }

            }
            else
            {
                if (IndexFournisseur == -1)
                    MessageBox.Show("incorrect Modifier Num Fournisseur non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (textBox1.Text.Length > 50)
                    MessageBox.Show("longueur de nom Fournisseur supérieur a 50 caractère");
                else if (textBox2.Text.Length > 10)
                    MessageBox.Show("longueur de Nomber Telephone Fournisseur supérieur a 10 Number");
                else if (richTextBox1.Text.Length > 100)
                    MessageBox.Show("longueur de Address Fournisseur supérieur a 100 caractère");
                else if (textBox1.Text == "")
                    MessageBox.Show("champ est vide !!");
                else
                    MessageBox.Show("error systeme !!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && e.RowIndex > -1 && e.RowIndex < dataGridView1.Rows.Count)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                richTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                this.IndexFournisseur = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());



                
                var Quantité_totle =(from a in data.Produit where a.NumFournisseur == this.IndexFournisseur select a.quantité).Sum().ToString();
                if(Quantité_totle.ToString()!="")
                textBox3.Text = Quantité_totle.ToString()+ " Produits";
                else
                textBox3.Text = "0 Produits";


                var prix_totle = (from a in data.Produit where a.NumFournisseur == this.IndexFournisseur select a.prix_Fournisseur * a.quantité).Sum().ToString();
                var prix_payment = (from a in data.Produit where a.NumFournisseur == this.IndexFournisseur select a.payment).Sum().ToString();

                if (prix_totle.ToString() == "")
                    prix_totle = "0";


                if (prix_payment.ToString() == "")
                    prix_payment = "0";


                textBox4.Text = prix_totle.ToString()+" Dhs";
                textBox5.Text = ( float.Parse(prix_totle.ToString()) - float.Parse(prix_payment.ToString()) ).ToString() + " Dhs";
                textBox6.Text = prix_payment.ToString() + " Dhs";





            }
            else
                this.IndexFournisseur = -1;
        }
    }
}
