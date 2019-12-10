using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RecepieList
{
    public partial class Recepie : Form
    {
        public Recepie(string fileName = null)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(fileName))
            {
                ingredients.Clear();

                FileStream inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                BinaryReader reader = new BinaryReader(inStream);

                bool moreContent = true;
                while (moreContent == true)
                {
                    try
                    {
                        Ingredient tmpIngredient = new Ingredient();

                        tmpIngredient.Type = reader.ReadString();
                        tmpIngredient.Amount = reader.ReadDouble();
                        tmpIngredient.Unit = reader.ReadString();

                        ingredients.Add(tmpIngredient);
                    }
                    catch
                    {
                        moreContent = false;
                    }
                }

                reader.Dispose();
                UpdateListbox();
            }
        }

        private List<Ingredient> ingredients = new List<Ingredient>();

        public class Ingredient
        {
            private string type;
            private double amount;
            private string unit;

            public Ingredient()
            {

            }

            public string Type
            {
                get
                {
                    return type;
                }
                set
                {
                    type = value;
                }
            }

            public double Amount
            {
                get
                {
                    return amount;
                }
                set
                {
                    amount = value;
                }
            }

            public string Unit
            {
                get
                {
                    return unit;
                }
                set
                {
                    unit = value;
                }
            }

            public override string ToString()
            {
                return type + ": " + amount.ToString() + " " + unit;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxIngredient.Text) && !string.IsNullOrEmpty(tbxUnit.Text) && double.TryParse(tbxAmount.Text, out double tmpAmount))
            {
                Ingredient tmpIngredient = new Ingredient();
                tmpIngredient.Type = tbxIngredient.Text;
                tmpIngredient.Amount = tmpAmount;
                tmpIngredient.Unit = tbxUnit.Text;

                ingredients.Add(tmpIngredient);
                UpdateListbox();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpen.ShowDialog();
            if (result == DialogResult.OK)
            {
                ingredients.Clear();

                FileStream inStream = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

                BinaryReader reader = new BinaryReader(inStream);

                bool moreContent = true;
                while (moreContent == true)
                {
                    try
                    {
                        Ingredient tmpIngredient = new Ingredient();

                        tmpIngredient.Type = reader.ReadString();
                        tmpIngredient.Amount = reader.ReadDouble();
                        tmpIngredient.Unit = reader.ReadString();

                        ingredients.Add(tmpIngredient);
                    }
                    catch
                    {
                        moreContent = false;
                    }
                }

                reader.Dispose();
                UpdateListbox();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgSave.ShowDialog();
            if (result == DialogResult.OK)
            {
                FileStream outStream = new FileStream(dlgSave.FileName, FileMode.OpenOrCreate, FileAccess.Write);

                BinaryWriter writer = new BinaryWriter(outStream);

                for (int i = 0; i < ingredients.Count; i++)
                {
                    writer.Write(ingredients[i].Type);
                    writer.Write(ingredients[i].Amount);
                    writer.Write(ingredients[i].Unit);
                }

                writer.Dispose();
            }
        }

        private void UpdateListbox ()
        {
            lbxIngredients.Items.Clear();
            for (int i = 0; i < ingredients.Count; i++)
            {
                lbxIngredients.Items.Add(ingredients[i].ToString());
            }
        }
    }
}
