using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace checkedListBox
{
    class CheckedListBox : UserControl
    {
        private List<CheckedListBoxItem> Items = new List<CheckedListBoxItem>();

        private Int32 itemHeight = 15;
        private Int32 selectedItem = -1;

        private EventHandler onSelectedChange;

        public CheckedListBox()
        {
        }

        #region Индексатор

        public CheckedListBoxItem this[Int32 index]
        {
            get { return Items[index]; }

        }

        #endregion

        #region События

        public event EventHandler OnSelectedChange
        {
            add
            {
                onSelectedChange += value;
            }
            remove
            {
                onSelectedChange -= value;
            }
        }


        protected override void OnMouseDown(MouseEventArgs e)//если нажали
        {
            base.OnMouseDown(e);
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)//отпустили мышку
        {
            base.OnMouseUp(e);



            Int32 selectedItemOld = selectedItem;

            selectedItem = (Int32)e.Y / itemHeight;//находим тот,по которому кликнули
            if (selectedItem > Items.Count - 1)
            {
                selectedItem = -1;
            }

            if (selectedItemOld != selectedItem)
            {
                if (onSelectedChange != null)
                {
                    onSelectedChange(this, EventArgs.Empty);
                }
            }
            try
            {
                if (e.X >= 5 && e.X <= 18)//ищем клик по чекбоксу
                {
                    if (Items[(Int32)e.Y / itemHeight].IsChecked)//если галочка стояла
                        Items[(Int32)e.Y / itemHeight].IsChecked = false;//снимаем галочку
                    else Items[(Int32)e.Y / itemHeight].IsChecked = true;//если наоборот-ставим
                }
            }
            catch { }
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;

            graphics.DrawRectangle(Pens.Gray, 0, 0, this.Width - 1, this.Height - 1);//рисуем основу
            this.BackColor = Color.White;

            for (int i = 0; i < Items.Count; i++)//цикл по всем элементам
            {
                String itemText = this[i].Text;//текст данного элемента

                Single tw = graphics.MeasureString(itemText, Font).Width;//измеряем ширину текста
                Single th = graphics.MeasureString(itemText, Font).Height;//измеряем высоту текста

                if (i == selectedItem)//если наш элемент выбран
                {
                    graphics.FillRectangle(Brushes.Blue, 0, itemHeight * i + 4, Width, itemHeight);//рисуем синий треуголник на нем
                }

                while (tw > Width)//если строка не влазит
                {

                    itemText = itemText.Substring(0, itemText.Length - 4);//удаляем последние 4 символа
                    itemText += "...";//ставим точки
                    tw = graphics.MeasureString(itemText, Font).Width;
                }

                Int32 x = 22;
                Int32 y = (Int32)Math.Round((Single)((itemHeight - th) / 2)) + itemHeight * i;//рассчитываем высоту

                if (i != selectedItem)
                {
                    if (Items[i].IsChecked)//если "с галочкой"
                    {
                        graphics.DrawRectangle(Pens.Black, 5, y + 4, 13, 13);//рисуем квадрат для чекбокса
                        Font f = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                        graphics.DrawString("\u2714", f, Brushes.Black, 3, y);//рисуем галочку в квадрате
                    }

                    else// если без
                        graphics.DrawRectangle(Pens.Black, 5, y + 4, 13, 13);//рисуем просто квадрат

                    graphics.DrawString(itemText, Font, Brushes.Black, x, y + 4);//рисуем текст элемента


                }

                else
                {
                    if (Items[i].IsChecked)
                    {
                        graphics.DrawRectangle(Pens.Black, 5, y + 4, 13, 13);
                        Font f = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                        graphics.DrawString("\u2714", f, Brushes.Black, 3, y);
                    }
                    else
                        graphics.DrawRectangle(Pens.Black, 5, y + 4, 13, 13);

                   graphics.DrawString(itemText, Font, Brushes.White, x, y + 4);
                
                }
            }

        }

        #endregion

        #region Методы(Delete, Get, Add, Clear)

        public void DeleteItem(Int32 index)
        {
            Items.Remove(this[index]);
            Refresh();
        }

        public CheckedListBoxItem GetItem(Int32 index)
        {
            return this[index];
        }

        public void AddItem(CheckedListBoxItem value)
        {
            Items.Add(value);
            Refresh();
        }

        public void ClearItems()
        {
            Items.Clear();
            selectedItem = -1;
            Refresh();
        }

        #endregion



    }
    public class CheckedListBoxItem
    {
        public string Text;
        public bool IsChecked;

        public CheckedListBoxItem(string Text, bool IsChecked)
        {
            this.Text = Text;
            this.IsChecked = IsChecked;
        }

    }
}