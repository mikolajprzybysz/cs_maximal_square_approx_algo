using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ApproximationAlgorithm
{
    public partial class RectangleEditor : Form
    {
        public ApproximationAlgorithmForm _parent;
        private Rectangle _rectangle;
        public RectangleEditor(Rectangle rectangle, ApproximationAlgorithmForm parent)
        {
            InitializeComponent();
            _parent = parent;
            _rectangle = rectangle;
            _parent.Enabled = false;
            widthTextBox.Text = Convert.ToString(rectangle.getWidth());
            HeightTextBox.Text = Convert.ToString(rectangle.getHeight());
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            _parent.editRectangle(Convert.ToInt32(widthTextBox.Text), Convert.ToInt32(HeightTextBox.Text));
            _parent.Enabled = true;
            this.Dispose();
        }

        private void RectangleEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.Enabled = true;
        }
    }
}
