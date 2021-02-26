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
    public partial class Form2 : Form
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        public void afficher()
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            for (int i = 1; i <= int.Parse(DateTime.Today.Month.ToString()); i++)
            {
                string mounth = i.ToString();
                if (i < 10)
                {
                    mounth = "0" + i.ToString();
                }

                var mois_totale_prix = (from a in data.Achate where a.dateAchate.ToString().Contains(DateTime.Today.Year.ToString() + "-" + mounth + "-") select a.prixAchate * a.quantitéProduit).Sum();

                var totale_prix_Reparation_day = (from a in data.Reparation where a.Date_Reparation.ToString().Contains(DateTime.Today.Year.ToString() + "-" + mounth + "-") select a.Prix).Sum();
                if (totale_prix_Reparation_day.ToString() == "")
                {
                    totale_prix_Reparation_day = 0;

                }
                if (mois_totale_prix.ToString() == "")
                {
                    mois_totale_prix = 0;

                }


                chart1.Series[0].Points.Add(int.Parse(mois_totale_prix.ToString()) + int.Parse(totale_prix_Reparation_day.ToString()));
                chart1.Series[0].Points[i - 1].Label = mounth;







                mois_totale_prix = 0;

                totale_prix_Reparation_day = 0;



            }


            for (int i = 1; i <= int.Parse(DateTime.Today.Day.ToString()); i++)
            {
                string jour = i.ToString();
                if (i < 10)
                {
                    jour = "0" + i.ToString();
                }

                var mois_totale_prix = (from a in data.Achate where a.dateAchate == DateTime.Parse(jour + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString()) select a.prixAchate * a.quantitéProduit).Sum();
                var mois_totale_quantité = (from a in data.Achate where a.dateAchate == DateTime.Parse(jour + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString()) select a.quantitéProduit).Sum();


                var totale_prix_Reparation_mois = (from a in data.Reparation where a.Date_Reparation == DateTime.Parse(jour + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString()) select a.Prix).Sum();
                if (totale_prix_Reparation_mois.ToString() == "")
                {
                    totale_prix_Reparation_mois = 0;

                }
                if (mois_totale_quantité.ToString() == "")
                {
                    mois_totale_quantité = 0;

                }
                if (mois_totale_prix.ToString() == "")
                {
                    mois_totale_prix = 0;

                }


                chart2.Series[0].Points.Add(int.Parse(mois_totale_quantité.ToString())).Name = jour;
                chart2.Series[1].Points.Add(int.Parse(mois_totale_prix.ToString()) + int.Parse(totale_prix_Reparation_mois.ToString())).Name = jour;
                chart2.Series[0].Points[i - 1].Label = jour.ToString();
                chart2.Series[1].Points[i - 1].Label = jour.ToString();

                mois_totale_prix = 0;
                totale_prix_Reparation_mois = 0;
                mois_totale_quantité = 0;

            }



           
            var today_totale_prix_Reparation = (from a in data.Reparation where a.Date_Reparation == DateTime.Today select a.Prix).Sum();
            label4.Text = today_totale_prix_Reparation + " Dhs";
            if (today_totale_prix_Reparation.ToString() == "")
            {
                label4.Text = "0" + " Dhs";

            }

            var today_totale_prix_produit = (from a in data.Achate where a.dateAchate == DateTime.Today select a.prixAchate * a.quantitéProduit).Sum();
            label5.Text = today_totale_prix_produit + " Dhs";
            if (today_totale_prix_produit.ToString() == "")
            {
                label5.Text = "0" + " Dhs";

            }
        }
        
    
      
       
        
      
      
       
        public Form2()
        {
            InitializeComponent();
            Size = Screen.PrimaryScreen.WorkingArea.Size;
            chart2.Series[0].Name = "quantité";

            chart2.Series.Add("Montant Totale");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.IsMdiContainer = true;
            this.LayoutMdi(MdiLayout.TileHorizontal);
            chart1.Series[0].Name = "Montant Totale";
            afficher();

        }

        private void gestionDeAchatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form g = new Gestion_De_Achat();

            g.Size = Screen.PrimaryScreen.WorkingArea.Size;
            // g.MdiParent = this;
           
             
                g.Show();
             
            
            

        }

        private void rechercherDesAchatsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // f.MdiParent = this;

            Form f = new Rechercher_Des_Achats();
            f.Show();
               
        }
   
        private void gestionDeProduitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //   pr.MdiParent = this;
            Form pr = new Gestion_De_Produit();
            pr.Show();
               
        }

        private void gestionDesFournisseursToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //   forn.MdiParent = this;

            Form forn = new Gestion_des_Fournisseurs();
       


            forn.Show();
             
            
        }

        private void gestionDesVendeursToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //  forn.MdiParent = this;

            Form V = new Gestion_des_Vendeurs();
           

            V.Show();
             
            
        }

        private void gestionDeRéparationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //  forn.MdiParent = this;

            Form r = new Gestion_De_Réparation();
            r.Show();
           
            
                
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("vous le voulez Fermer programme ?", "attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

   

       
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string l = "https://www.linkedin.com/in/ayoub-elmarhraoui-a106041b4/";

            // Determine which link was clicked and set the appropriate url.


            // Set the visited property to True. This will change
            // the color of the link.
            e.Link.Visited = true;

            // Open Internet Explorer to the correct url.
            System.Diagnostics.Process.Start("chrome.exe", l);
        }
    }
}
