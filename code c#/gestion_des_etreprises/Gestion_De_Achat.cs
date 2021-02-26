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
    public partial class Gestion_De_Achat : Form
    {
        private DataClasses1DataContext data = new DataClasses1DataContext();
        private int indexNumPruduit = -1;
        private int index_selected = -1;
        public void afficher()
        {

            var data_today = from a in data.Achate
                             join p in data.Produit on a.NumProduit equals p.NumProduit
                             join v in data.Vendeur on a.NumVendeur equals v.NumVendeur
                             where a.dateAchate == dateTimePicker2.Value
                             select new { Num = a.NumAchate, Nom_De_Produit = p.nameProduit, Quantité_Produit = a.quantitéProduit, Prix_Fournisseur = p.prix_Fournisseur, Prix_Achate = a.prixAchate, Nom_Vendeur = v.nameVendeur , Solde_De_Crédit = a.payment};
            var today_Quantité_Produit = (from a in data.Achate where a.dateAchate == dateTimePicker2.Value select a.quantitéProduit).Sum();
            var today_totale_prix = (from a in data.Achate where a.dateAchate == dateTimePicker2.Value select a.prixAchate * a.quantitéProduit).Sum();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = data_today;

            textBox1.Text = today_Quantité_Produit.ToString() + " Produit";
            textBox2.Text = today_totale_prix.ToString() + " DHs";
            if (today_Quantité_Produit.ToString() == "")
            {
                textBox1.Text = "0" + " Produit";

            }
            if (today_totale_prix.ToString() == "")
            {
                textBox2.Text = "0" + " DHs";
            }
            dataGridView1.ClearSelection();

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 400;

        }
        public void effacer_champ()
        {
            richTextBox1.Clear();
            numericUpDown1.Value = 1;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            this.indexNumPruduit = -1;
            this.index_selected = -1;
            TypesProduit.SelectedIndex = -1;
            dataGridView1.ClearSelection();
            Produits.Items.Clear();
            


        }
        public Gestion_De_Achat()
        {
            InitializeComponent();
        

        }

        private void Gestion_De_Achat_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox1.Text = "0" + " Produit";
            textBox2.Text = "0" + " DHs";
            checkBox1.Checked = true;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker2.Enabled = false;
            dateTimePicker2.MaxDate = DateTime.Today;

            numericUpDown3.Enabled = false;
            numericUpDown1.Maximum = numericUpDown2.Maximum = numericUpDown3.Maximum = 100000;
            radioButton1.Checked = true;


            var TypeProduit = (from t in data.Produit select (t.TypeProduit)).Distinct();
            var Ven = from v in data.Vendeur select v.nameVendeur;
            this.TypesProduit.Items.Clear();
            foreach (var a in TypeProduit)
                this.TypesProduit.Items.Add(a.ToString());
            foreach (var v in Ven)
                this.comboBox1.Items.Add(v.ToString());

            afficher();
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            effacer_champ();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown3.Value < 10000 && numericUpDown1.Value < 10000 && numericUpDown2.Value < 10000 && Produits.Text != "" && comboBox1.Text != "" && numericUpDown1.Value > 0 && numericUpDown3.Value < 10000  )
            {
              
                try
                {
                    var testQMoin = from t in data.Produit where t.NumProduit == indexNumPruduit select t;

                    if (int.Parse(testQMoin.Single().quantité.ToString()) - int.Parse(numericUpDown1.Value.ToString()) >= 0)
                    {
                        var a = new Achate();
                        a.NumProduit = indexNumPruduit;
                        a.NumVendeur = (from v in data.Vendeur where v.nameVendeur == comboBox1.Text select v.NumVendeur).Single();
                        a.prixAchate = int.Parse(numericUpDown2.Value.ToString());
                        a.quantitéProduit = int.Parse(numericUpDown1.Value.ToString());
                        a.dateAchate = dateTimePicker2.Value;
                        a.commenter = richTextBox1.Text;
                        a.payment= int.Parse(numericUpDown3.Value.ToString());
                        data.Achate.InsertOnSubmit(a);
                        testQMoin.Single().quantité = int.Parse(testQMoin.Single().quantité.ToString()) - int.Parse(numericUpDown1.Value.ToString()); ;
                        data.SubmitChanges();

                        afficher();
                        effacer_champ();
                        MessageBox.Show(" produit ajouter successful");

                    }
                    else
                        MessageBox.Show(" aucune quantité produite ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
                catch
                {
                    MessageBox.Show("error systeme ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


            }
            else
            {
                if (Produits.Text == "")
                    MessageBox.Show("incorrect ajouter  nom produit vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (numericUpDown1.Value == 0)
                    MessageBox.Show("impossible  Quantité Produit est 0 !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                else
                    MessageBox.Show("champ Nom Vendeur est vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.CurrentRow.Index >= 0 && dataGridView1.CurrentRow.Index < dataGridView1.Rows.Count && this.index_selected != -1)
            {
                try
                {

                    if (MessageBox.Show("vous le voulez bien Supprimé", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var testQplus = from t in data.Produit where t.NumProduit == indexNumPruduit select t;
                        var a = (from ach in data.Achate where ach.NumAchate == int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()) select ach).Single();
                        data.Achate.DeleteOnSubmit(a);
                        testQplus.Single().quantité = int.Parse(testQplus.Single().quantité.ToString()) + int.Parse(dataGridView1.Rows[this.index_selected].Cells[2].Value.ToString()); ;
                        data.SubmitChanges();
                        afficher();
                        effacer_champ();
                        MessageBox.Show("produit Supprimer successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                }
                catch
                {
                    MessageBox.Show("error systeme ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("incorrect Supprimer Num produit non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

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

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {

                this.index_selected = e.RowIndex;
                comboBox1.Text = dataGridView1.Rows[this.index_selected].Cells[5].Value.ToString();
                var produit = from p in data.Produit where p.nameProduit == dataGridView1.Rows[this.index_selected].Cells[1].Value.ToString() select p.TypeProduit;
                TypesProduit.Text = produit.Single().ToString();
                string NameProduits = dataGridView1.Rows[this.index_selected].Cells[1].Value.ToString();

                foreach (string itms in Produits.Items)
                {
                    string[] infos = itms.Split('=');
                    if (NameProduits + " " == infos[0])
                    {
                        Produits.Text = itms.ToString();
                        break;
                    }
                }
                richTextBox1.Text = (from a in data.Achate where a.NumAchate == int.Parse(dataGridView1.Rows[this.index_selected].Cells[0].Value.ToString()) select a.commenter).Single();

                numericUpDown1.Value = int.Parse(dataGridView1.Rows[this.index_selected].Cells[2].Value.ToString());
                numericUpDown2.Value = int.Parse(dataGridView1.Rows[this.index_selected].Cells[4].Value.ToString()); ;


            }
            else
                this.index_selected = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 10000 && numericUpDown2.Value < 10000 && this.index_selected != -1 && dataGridView1.Rows.Count > 0 && dataGridView1.CurrentRow.Index >= 0 && dataGridView1.CurrentRow.Index < dataGridView1.Rows.Count && Produits.Text != "" && comboBox1.Text != "" && numericUpDown3.Value < 10000 && numericUpDown1.Value > 0 )
            {
                try
                {
                    if (MessageBox.Show("vous le voulez bien Modifier", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        var ProdQ = from t in data.Produit where t.NumProduit == indexNumPruduit select t;

                        var a = (from ach in data.Achate where ach.NumAchate == int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString()) select ach).Single();
                        int testQPlusMoin = int.Parse(numericUpDown1.Value.ToString()) - int.Parse(a.quantitéProduit.ToString());




                        if (int.Parse(ProdQ.Single().quantité.ToString()) >= testQPlusMoin   )
                        {
                            if(ProdQ.Single().nameProduit.ToString()==dataGridView1.Rows[index_selected].Cells[1].Value.ToString())
                            {
                                if (testQPlusMoin > 0)
                                    ProdQ.Single().quantité = int.Parse(ProdQ.Single().quantité.ToString()) - testQPlusMoin;
                                else
                                    ProdQ.Single().quantité = int.Parse(ProdQ.Single().quantité.ToString()) + (testQPlusMoin * -1);

                                a.prixAchate = int.Parse(numericUpDown2.Value.ToString());
                                a.quantitéProduit = int.Parse(numericUpDown1.Value.ToString());
                                a.NumVendeur = (from v in data.Vendeur where v.nameVendeur == comboBox1.Text select v.NumVendeur).Single();
                                a.dateAchate = dateTimePicker2.Value;
                                a.commenter = richTextBox1.Text;
                                a.payment = int.Parse(numericUpDown3.Value.ToString());
                                MessageBox.Show(" produit Modifier successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            else
                            {

                                if (int.Parse(ProdQ.Single().quantité.ToString()) - int.Parse(numericUpDown1.Value.ToString()) >= 0)
                                { 
                                var raj = from p in data.Produit where p.NumProduit == a.NumProduit select p;
                                raj.Single().quantité = (int.Parse(raj.Single().quantité.ToString())) + int.Parse(a.quantitéProduit.ToString());
                                a.NumProduit = this.indexNumPruduit;
                                a.prixAchate = int.Parse(numericUpDown2.Value.ToString());
                                a.quantitéProduit = int.Parse(numericUpDown1.Value.ToString());
                                a.NumVendeur = (from v in data.Vendeur where v.nameVendeur == comboBox1.Text select v.NumVendeur).Single();
                                a.dateAchate = dateTimePicker2.Value;
                                a.commenter = richTextBox1.Text;
                                a.payment = int.Parse(numericUpDown3.Value.ToString());

                                    ProdQ.Single().quantité = int.Parse(ProdQ.Single().quantité.ToString()) - int.Parse(a.quantitéProduit.ToString());

                                    MessageBox.Show(" produit Modifier successful ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }
                                else
                                    MessageBox.Show(" aucune quantité produite ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                            }




                            data.SubmitChanges();
                            afficher();
                            effacer_champ();
                        }
                        else
                            MessageBox.Show(" aucune quantité produite ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    }

                }
                catch
                {
                    MessageBox.Show("error systeme ", "attention", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                if (Produits.Text == "")
                    MessageBox.Show("incorrect Modifier  nom produit vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (numericUpDown1.Value == 0)
                    MessageBox.Show("impossible  Quantité Produit est 0 !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (index_selected == -1)
                    MessageBox.Show("incorrect Supprimer Num produit non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else if (comboBox1.Text =="")
                    MessageBox.Show("champ Nom Vendeur est vide !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                    MessageBox.Show("incorrect Modifier Num produit non sélect a table !!", "attention", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void Gestion_De_Achat_Deactivate(object sender, EventArgs e)
        {

            Form2 formGlobal = (Form2)Application.OpenForms["Form2"];
            formGlobal.afficher();
           
           
        }

        private void TypesProduit_SelectedValueChanged(object sender, EventArgs e)
        {
            if (TypesProduit.Text.ToString() != "")
            {
                this.Produits.Items.Clear();
                var Produits = from p in data.Produit where p.TypeProduit == TypesProduit.Text.ToString() select new { p.nameProduit, p.quantité, p.prix_Fournisseur };
                foreach (var a in Produits)
                    this.Produits.Items.Add(a.nameProduit + " => " + "  | Quantité : " + a.quantité + "   | Prix :  " + a.prix_Fournisseur + " Dh");

            }
            else
                this.indexNumPruduit = -1;
        }

        private void Produits_SelectedValueChanged(object sender, EventArgs e)
        {
            if (TypesProduit.Text.ToString() != "")
            {
                string[] infos = Produits.Text.Split('=');
                var NumProduit = from p in data.Produit where p.nameProduit == infos[0].ToString() select p.NumProduit;
                if (NumProduit.Count() == 1)
                {
                    this.indexNumPruduit = NumProduit.Single();
                    numericUpDown2.Value = int.Parse((from p in data.Produit where p.NumProduit == this.indexNumPruduit select p.prix_achat).Single().ToString());

                }

            }
            else
                this.indexNumPruduit = -1;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                dateTimePicker2.Enabled = false;
                dateTimePicker2.Value = DateTime.Today;
            }
            else
                dateTimePicker2.Enabled = true;
        }

        private void dateTimePicker2_ValueChanged_1(object sender, EventArgs e)
        {
            afficher();
        }

        private void comboBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {

                this.index_selected = e.RowIndex;
                comboBox1.Text = dataGridView1.Rows[this.index_selected].Cells[5].Value.ToString();
                var produit = from p in data.Produit where p.nameProduit == dataGridView1.Rows[this.index_selected].Cells[1].Value.ToString() select p.TypeProduit;
                TypesProduit.Text = produit.Single().ToString();
                string NameProduits = dataGridView1.Rows[this.index_selected].Cells[1].Value.ToString();

                foreach (string itms in Produits.Items)
                {
                    string[] infos = itms.Split('=');
                    if (NameProduits + " " == infos[0])
                    {
                        Produits.Text = itms.ToString();
                        break;
                    }
                }
                richTextBox1.Text = (from a in data.Achate where a.NumAchate == int.Parse(dataGridView1.Rows[this.index_selected].Cells[0].Value.ToString()) select a.commenter).Single();

                numericUpDown1.Value = int.Parse(dataGridView1.Rows[this.index_selected].Cells[2].Value.ToString());
                numericUpDown2.Value = int.Parse(dataGridView1.Rows[this.index_selected].Cells[4].Value.ToString());

                if (int.Parse(dataGridView1.Rows[this.index_selected].Cells[6].Value.ToString()) == 0)
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;

                numericUpDown3.Value = int.Parse(dataGridView1.Rows[this.index_selected].Cells[6].Value.ToString());

            }
            else
                this.index_selected = -1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            numericUpDown3.Enabled = true;
            

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            numericUpDown3.Value = 0;
            numericUpDown3.Enabled = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
