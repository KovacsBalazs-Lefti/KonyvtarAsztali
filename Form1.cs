using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KonyvtarAsztali
{
    public partial class Form1 : Form
    {
        private BookService bookService;
        public Form1()
        {
            InitializeComponent();
            //előtte a grid column-nak a propertijébe meg kell adni az  oszlop nevét nagybetűvel
            //egész sort jelölje ki- grid beállítása-"SelectionMode" fullrowselect
            // multiselect false-ra állítása hogy ne lehessen több elemet kiválasztani
           BooksGrid.AutoGenerateColumns = false;
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                bookService = new BookService();
                //frissíti a datagridet az aktuális adatokkal
                RefreshBooksGrid();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message, "Hiba történt az adatbázis kapcsolat kialakításakor");
                this.Close();
            }
        }

        private void RefreshBooksGrid()
        {
            BooksGrid.DataSource = bookService.GetBooks();
            //törlése az auto selectnek
            BooksGrid.ClearSelection();

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
       
            if (BooksGrid.SelectedRows.Count == 0) 
            {
                MessageBox.Show("A törléshez válasszon ki könyvet!");
                return;
            }
            DialogResult result = MessageBox.Show("Biztos szeretné törölni a kiválasztott könyvet?", "Biztos", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) 
            {
                return;
            }


            try
            {
                Book selected = BooksGrid.SelectedRows[0].DataBoundItem as Book;
                if (bookService.DeleteBook(selected.Id))
                {
                    MessageBox.Show("Sikeres törlés");
                }
                else
                {
                    MessageBox.Show("Ez a könyv már korábban törlésre került", "Hiba történt a törlés során");
                }
                RefreshBooksGrid();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Hiba történt a törlés során");
                
            }
        }
    }
}
