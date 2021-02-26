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
    public partial class Gestion_des_Vendeurs : Form
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        private int IndexVendeurs = -1;
        public void afficher()
        {
            var ven = from v in data.Vendeur select new { Nombre_Vendeur = v.NumVendeur, Nom_Vendeur = v.nameVendeur };
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ven;
            dataGridView1.ClearSelection();
            this.IndexVendeurs = -1;
            textBox1.Text = "";
        }
        public Gestion_des_Vendeurs()
        {
            InitializeComponent();
        }

        private void Gestion_des_Vendeurs_Load(object sender, EventArgs e)
        {
            afficher();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text.Length <= 100)
            {
                try
                {
                    Vendeur v = new Vendeur();
                    v.nameVendeur = textBox1.Text;
                    data.Vendeur.InsertOnSubmit(v);
                    data.SubmitChanges();
                    afficher();
                    MessageBox.Show(" Vendeur ajouter successful");

                }
                catch
                {
                    MessageBox.Show("error systeme !!");
                }

            }
            else
            {
                if (textBox1.Text.Length > 50)
                    MessageBox.Show("longueur de nom Vendeur supérieur a 100 caractère !!");
                else
                    MessageBox.Show("champ est vide !!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.IndexVendeurs != -1)
            {
                try
                {
                    if (MessageBox.Show("vous le voulez bien Vendeurs", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var ven = (from v in data.Vendeur where v.NumVendeur == this.IndexVendeurs select v).Single();
                        data.Vendeur.DeleteOnSubmit(ven);
                        data.SubmitChanges();
                        afficher();
                        MessageBox.Show(" Vendeurs Supprimer successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            if (this.IndexVendeurs != -1 && textBox1.Text != "" && textBox1.Text.Length <= 100)
            {
                try
                {
                    if (MessageBox.Show("vous le voulez bien Modifier", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var ven = (from v in data.Vendeur where v.NumVendeur == this.IndexVendeurs select v).Single();
                        ven.nameVendeur = textBox1.Text;
                        data.SubmitChanges();
                        afficher();
                        MessageBox.Show(" Vendeurs Modifier successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
                catch
                {
                    MessageBox.Show("error systeme !!");
                }

            }
            else
            {
                if (textBox1.Text.Length > 50)
                    MessageBox.Show("longueur de nom Vendeur supérieur a 50 caractère !!");
                else if (this.IndexVendeurs == -1)
                    MessageBox.Show("incorrect Supprimer Num Vendeur non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                    MessageBox.Show("champ est vide !!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && e.RowIndex > -1 && e.RowIndex < dataGridView1.Rows.Count)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                this.IndexVendeurs = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            else
                this.IndexVendeurs = -1;
        }
    }
}
