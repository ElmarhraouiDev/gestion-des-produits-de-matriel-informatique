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
    public partial class Gestion_De_Produit : Form
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        private int indexProduit = -1;
        public void afficher()
        {
            comboBox3.Items.Clear();
            comboBox1.Items.Clear();
            var typePr = (from t in data.Produit select t.TypeProduit).Distinct();
            foreach (var t in typePr)
            {
                comboBox1.Items.Add(t.ToString());
            }
            comboBox3.Items.Add("tous les  catégories");
            foreach (var i in typePr)
            {
                comboBox3.Items.Add(i.ToString());
            }

            if (comboBox1.Text == "")
            {
                comboBox3.Text = "tous les  catégories";
            }
            else
            {
                var prod = from p in data.Produit join f in data.Fournisseur on p.NumFournisseur equals f.NumFournisseur where p.TypeProduit == comboBox1.Text.ToString() select new { Num = p.NumProduit, Nom_Produit = p.nameProduit, Categorie_Produit = p.TypeProduit, p.quantité,Prix_Fournisseur = p.prix_Fournisseur, Prix_achat = p.prix_achat, Date_Commande = p.DateCommande, Nom_Fournisseur = f.nameFournisseur , Solde_De_Crédit=p.payment };
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = prod;
                if (dataGridView1.Rows.Count == 0)
                {
                    textBox2.Text = "0  Produit";
                    textBox3.Text = "0 Dhs";
                }
                else
                {
                    textBox3.Text = (from p in data.Produit where p.TypeProduit == comboBox1.Text.ToString() select p.prix_Fournisseur * p.quantité).Sum().ToString() + " Dhs";
                    textBox2.Text = (from p in data.Produit where p.TypeProduit == comboBox1.Text.ToString() select p.quantité).Sum().ToString() + " Produits";

                }
                comboBox3.Text = comboBox1.Text;


            }



            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 300;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 100;

            this.indexProduit = -1;
            efacer_champ();
            
        }
        public void efacer_champ()
        {
            textBox1.Text = "";
            comboBox1.Text = comboBox2.Text = "";
            numericUpDown1.Value = numericUpDown3.Value = 0;
            textBox4.Text = "0";
            dateTimePicker1.Value = DateTime.Today;
            dataGridView1.ClearSelection();

            radioButton1.Checked = true;
        }
        public Gestion_De_Produit()
        {
            InitializeComponent();
            
        }

        private void Gestion_De_Produit_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = DateTime.Today;
            numericUpDown1.Maximum = numericUpDown3.Maximum = numericUpDown4.Maximum = 100000;
            textBox4.Text = "0";
            textBox2.Enabled = textBox3.Enabled = false;
            var forn = (from f in data.Fournisseur select f.nameFournisseur);
            foreach (var i in forn)
            {
                comboBox2.Items.Add(i.ToString());
            }
            afficher();
            radioButton1.Checked = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            efacer_champ();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text[0].ToString()!="," && numericUpDown1.Value < 10000 && textBox4.Text!="" && numericUpDown3.Value < 10000 && textBox1.Text != "" && textBox1.Text.Length <= 50 && comboBox1.Text != "" && comboBox1.Text.Length <= 50 && comboBox2.Text != "" && numericUpDown1.Value > 0 && float.Parse(textBox4.Text.ToString()) > 0 && float.Parse(numericUpDown3.Value.ToString()) > float.Parse(textBox4.Text.ToString()) && numericUpDown4.Value < 10000 && ( (float.Parse(numericUpDown1.Value.ToString())* float.Parse(textBox4.Text.ToString()))- float.Parse(numericUpDown4.Value.ToString()) )>=0)
            {
                try
                {
                    var pDouble = from p in data.Produit where p.nameProduit == textBox1.Text select p;

                    if (pDouble.Count() == 0)
                    {
                        Produit p = new Produit();
                        p.nameProduit = textBox1.Text;
                        p.TypeProduit = comboBox1.Text;
                        var NumFournisseur = from f in data.Fournisseur where f.nameFournisseur == comboBox2.Text select f.NumFournisseur;
                        p.NumFournisseur = int.Parse(NumFournisseur.Single().ToString());
                        p.quantité = int.Parse(numericUpDown1.Value.ToString());
                        p.prix_Fournisseur = float.Parse(textBox4.Text.ToString());
                        p.prix_achat = int.Parse(numericUpDown3.Value.ToString());
                        p.DateCommande = dateTimePicker1.Value;
                        p.payment= int.Parse(numericUpDown4.Value.ToString());
                        data.Produit.InsertOnSubmit(p);
                        data.SubmitChanges();
                        afficher();
                        MessageBox.Show(" Produit ajouter successful");

                    }
                    else
                        MessageBox.Show("Produit deja ajoute !!");
                }
                catch
                {
                    MessageBox.Show("error systeme !!");
                }

            }
            else
            {
                if (textBox1.Text == "")
                    MessageBox.Show("champ de Nom Produit est vide !!");
                else if (textBox1.Text.Length > 50)
                    MessageBox.Show("longueur de nom Produit supérieur a 50 caractère !!");
                else if (comboBox1.Text.Length > 50)
                    MessageBox.Show("longueur de Type Produit supérieur a 50 caractère !!");
                else if (comboBox1.Text == "")
                    MessageBox.Show("champ de Type Produit est vide !!");
                else if (textBox4.Text[0].ToString() == ",")
                    MessageBox.Show("incorrect Prix Fournisseur  !!");
                else if (comboBox2.Text == "")
                    MessageBox.Show("champ de Nom  Fournisseur est vide !!");
                else if (numericUpDown1.Value == 0)
                    MessageBox.Show("incorrect Quantité 0 !!");
                else if (textBox4.Text == "")
                    MessageBox.Show("incorrect Prix Fournisseur vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (numericUpDown3.Value == 0)
                    MessageBox.Show("incorrect Prix achat 0 !!");
                else if (float.Parse(textBox4.Text.ToString()) >= float.Parse(numericUpDown3.Value.ToString()))
                    MessageBox.Show("impossible Prix de Fournisseur supérieur ou égal a  Prix achat !!");
                else if (((float.Parse(numericUpDown1.Value.ToString())) * float.Parse(textBox4.Text.ToString())) - float.Parse(numericUpDown4.Value.ToString()) < 0)
                    MessageBox.Show("impossible Solde De Crédit  supérieur a Montant Total  !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("informations incorrectes");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.indexProduit != -1 &&  textBox4.Text[0].ToString() != "," &&  numericUpDown1.Value < 10000 && textBox4.Text != "" && numericUpDown3.Value < 10000 && textBox1.Text != "" && textBox1.Text.Length <= 50 && comboBox1.Text != "" && comboBox1.Text.Length <= 50 && comboBox2.Text != "" && numericUpDown1.Value > 0 && float.Parse(textBox4.Text.ToString()) > 0 && float.Parse(numericUpDown3.Value.ToString()) > float.Parse(textBox4.Text.ToString()) && numericUpDown4.Value < 10000 && ((float.Parse(numericUpDown1.Value.ToString()) * float.Parse(textBox4.Text.ToString())) - float.Parse(numericUpDown4.Value.ToString())) >= 0)
            {
                try
                {

                    var doubleName = from p in data.Produit where p.nameProduit == textBox1.Text select p;
                    var namPR = from p in data.Produit where p.NumProduit == this.indexProduit select p;
                    if (doubleName.Count() == 0 || textBox1.Text == namPR.Single().nameProduit)
                    {
                        if (MessageBox.Show("vous le voulez bien Modifier", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var p = (from pr in data.Produit where pr.NumProduit == this.indexProduit select pr).Single();
                            p.nameProduit = textBox1.Text;
                            p.TypeProduit = comboBox1.Text;
                            var NumFournisseur = from f in data.Fournisseur where f.nameFournisseur == comboBox2.Text select f.NumFournisseur;
                            p.NumFournisseur = int.Parse(NumFournisseur.Single().ToString());
                            p.quantité = int.Parse(numericUpDown1.Value.ToString());
                            p.prix_Fournisseur = float.Parse(textBox4.Text.ToString());
                            p.prix_achat = int.Parse(numericUpDown3.Value.ToString());
                            p.DateCommande = dateTimePicker1.Value;
                            p.payment = int.Parse(numericUpDown4.Value.ToString());
                            data.SubmitChanges();
                            afficher();
                            MessageBox.Show(" Produit Modifier successful");
                        }

                    }
                    else
                        MessageBox.Show("Produit deja exsist  !!");
                }
                catch
                {
                    MessageBox.Show("error systeme !!");
                }

            }
            else
            {
                if (this.indexProduit == -1)
                    MessageBox.Show("incorrect Supprimer Num Fournisseur non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox1.Text == "")
                    MessageBox.Show("champ de Nom Produit est vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox1.Text.Length > 50)
                    MessageBox.Show("longueur de nom Produit supérieur a 50 caractère !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (comboBox1.Text.Length > 50)
                    MessageBox.Show("longueur de Type Produit supérieur a 50 caractère !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (comboBox1.Text == "")
                    MessageBox.Show("champ de Type Produit est vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (comboBox2.Text == "")
                    MessageBox.Show("champ de Nom  Fournisseur est vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox4.Text[0].ToString() == ",")
                    MessageBox.Show("incorrect Prix Fournisseur  !!");
                else if (numericUpDown1.Value == 0)
                    MessageBox.Show("incorrect Quantité 0 !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox4.Text == "")
                    MessageBox.Show("incorrect Prix Fournisseur vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (numericUpDown3.Value == 0)
                    MessageBox.Show("incorrect Prix achat 0 !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (float.Parse(textBox4.Text.ToString()) >= float.Parse(numericUpDown3.Value.ToString()))
                    MessageBox.Show("impossible Prix de Fournisseur supérieur ou égal a  Prix achat !!");
                else if (((float.Parse(numericUpDown1.Value.ToString())) * float.Parse(textBox4.Text.ToString())) - float.Parse(numericUpDown4.Value.ToString()) < 0)
                    MessageBox.Show("impossible Solde De Crédit  supérieur a Montant Total  !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("informations incorrectes");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.indexProduit != -1)
            {
                try
                {
                    if (MessageBox.Show("vous le voulez bien Supprimer", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var p = (from pr in data.Produit where pr.NumProduit == this.indexProduit select pr).Single();
                        data.Produit.DeleteOnSubmit(p);
                        data.SubmitChanges();
                        afficher();
                        MessageBox.Show(" Produit Supprimer successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
                catch
                {
                    MessageBox.Show("error systeme !!");
                }

            }
            else
                MessageBox.Show("incorrect Supprimer Num Produit non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && e.RowIndex > -1 && e.RowIndex < dataGridView1.Rows.Count)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                numericUpDown1.Value = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                numericUpDown3.Value = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                dateTimePicker1.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                comboBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                this.indexProduit = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            else
                this.indexProduit = -1;
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != "tous les  catégories")
            {
                var prod = from p in data.Produit join f in data.Fournisseur on p.NumFournisseur equals f.NumFournisseur where p.TypeProduit == comboBox3.Text.ToString() select new { Num = p.NumProduit, Nom_Produit = p.nameProduit, Categorie_Produit = p.TypeProduit, p.quantité, Prix_Fournisseur = p.prix_Fournisseur, Prix_achat = p.prix_achat, Date_Commande = p.DateCommande, Nom_Fournisseur = f.nameFournisseur, Solde_De_Crédit = p.payment };
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = prod;
                textBox2.Text = (from p in data.Produit where p.TypeProduit == comboBox3.Text.ToString() select p.quantité).Sum().ToString() + " Produits";
                textBox3.Text = (from p in data.Produit where p.TypeProduit == comboBox3.Text.ToString() select p.prix_Fournisseur * p.quantité).Sum().ToString() + " Dhs";
            }
            else
            {
                var prod = from p in data.Produit join f in data.Fournisseur on p.NumFournisseur equals f.NumFournisseur select new { Num = p.NumProduit, Nom_Produit = p.nameProduit, Categorie_Produit = p.TypeProduit, p.quantité, Prix_Fournisseur = p.prix_Fournisseur, Prix_achat = p.prix_achat, Date_Commande = p.DateCommande, Nom_Fournisseur = f.nameFournisseur, Solde_De_Crédit = p.payment };
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = prod;
                textBox2.Text = (from p in data.Produit select p.quantité).Sum().ToString() + " Produits";
                textBox3.Text = (from p in data.Produit select p.prix_Fournisseur * p.quantité).Sum().ToString() + " Dhs";
            }
            this.indexProduit = -1;
            efacer_champ();
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void comboBox3_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && e.RowIndex > -1 && e.RowIndex < dataGridView1.Rows.Count)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                numericUpDown1.Value = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                numericUpDown3.Value = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                dateTimePicker1.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                comboBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString()) != 0)
                    radioButton2.Checked = true;
                else
                    radioButton1.Checked = true;

                numericUpDown4.Value = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());



                this.indexProduit = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            else
                this.indexProduit = -1;
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    
        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            
        
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                
                    e.Handled = true;
               
            }

            // If you want, you can allow decimal (float) numbers
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                
                e.Handled = true;
            }



        }
    }
}
